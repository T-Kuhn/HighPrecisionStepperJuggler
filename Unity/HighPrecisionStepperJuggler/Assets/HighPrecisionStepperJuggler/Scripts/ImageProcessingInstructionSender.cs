using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public class ImageProcessingInstructionSender : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private MachineController _machineController;

        private BallData _ballData = new BallData();
        private int _currentStrategyIndex;

        private List<IBallControlStrategy> _ballControlStrategies = new List<IBallControlStrategy>();

        private void Start()
        {
            _ballControlStrategies.Add(
                new BallControlStrategy((ballData, machineController, instructionCount) =>
                {
                    var moveTime = 0.1f;
                    _machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.01f, 0f, 0f, 0.5f),
                        new HLInstruction(0.05f, 0f, 0f, 0.5f),
                        new HLInstruction(0.02f, 0f, 0f, moveTime),
                        new HLInstruction(0.05f, 0f, 0f, moveTime),
                        new HLInstruction(0.02f, 0f, 0f, moveTime),
                        new HLInstruction(0.05f, 0f, 0f, moveTime),
                        new HLInstruction(0.02f, 0f, 0f, moveTime),
                        new HLInstruction(0.05f, 0f, 0f, moveTime),
                    });
                    _ballData.ResetVelocityAccumulation();
                    return true;
                }, 1));

            _ballControlStrategies.Add(
                new BallControlStrategy((ballData, machineController, instructionCount) =>
                {
                    if (_ballData.CurrentPositionVector.z < 150f)
                    {
                        // distance away from plate:
                        var p_x = -_ballData.CurrentPositionVector.x * c.k_p;
                        var p_y = _ballData.CurrentPositionVector.y * c.k_p;

                        // mean velocity of ball:
                        var velocityVector = _ballData.GetVelocityVector();
                        var d_x = -velocityVector.x * c.k_d;
                        var d_y = velocityVector.y * c.k_d;

                        var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                        var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                        var moveTime = 0.1f;
                        _machineController.SendInstructions(new List<HLInstruction>()
                        {
                            new HLInstruction(0.06f, xCorrection, yCorrection, moveTime),
                            new HLInstruction(0.05f, 0f, 0f, moveTime),
                        });

                        _ballData.ResetVelocityAccumulation();
                        return true;
                    }

                    return false;
                }, 100));
            _ballControlStrategies.Add(
                new BallControlStrategy((ballData, machineController, instructionCount) =>
                {
                    if (instructionCount == 0)
                    {
                        var moveTime = 0.1f;
                        _machineController.SendInstructions(new List<HLInstruction>()
                        {
                            new HLInstruction(0.04f, 0f, 0f, moveTime),
                        });
                        _ballData.ResetVelocityAccumulation();
                        return true;
                    }

                    if (_ballData.CurrentPositionVector.z < 140f)
                    {
                        // distance away from plate:
                        var p_x = -_ballData.CurrentPositionVector.x * c.k_p;
                        var p_y = _ballData.CurrentPositionVector.y * c.k_p;

                        // mean velocity of ball:
                        var velocityVector = _ballData.GetVelocityVector();
                        var d_x = -velocityVector.x * c.k_d;
                        var d_y = velocityVector.y * c.k_d;

                        var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                        var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                        var moveTime = 0.1f;
                        _machineController.SendInstructions(new List<HLInstruction>()
                        {
                            new HLInstruction(0.05f, xCorrection, yCorrection, moveTime),
                            new HLInstruction(0.04f, 0f, 0f, moveTime),
                        });

                        _ballData.ResetVelocityAccumulation();
                        return true;
                    }

                    return false;
                }, 100));
        }

        private void Update()
        {
            var ballRadiusAndPosition = _cameraPlugin.UpdateImageProcessing();
            var height = FOVCalculations.RadiusToDistance(ballRadiusAndPosition.Radius);

            if (height >= float.MaxValue)
            {
                // couldn't find ball in image
                return;
            }

            _ballData.UpdateData(
                new Vector3(
                    FOVCalculations.PixelPositionToDistanceFromCenter(ballRadiusAndPosition.PositionX, height),
                    FOVCalculations.PixelPositionToDistanceFromCenter(ballRadiusAndPosition.PositionY, height),
                    height),
                _machineController.IsReadyForNextInstruction);

            if (_machineController.IsReadyForNextInstruction)
            {
                var nextStrategyRequested =
                    _ballControlStrategies[_currentStrategyIndex].Execute(_ballData, _machineController);

                if (nextStrategyRequested)
                {
                    _ballControlStrategies[_currentStrategyIndex].Reset();

                    if (_currentStrategyIndex < _ballControlStrategies.Count - 1)
                    {
                        _currentStrategyIndex++;
                    }
                }
            }
        }
    }
}
