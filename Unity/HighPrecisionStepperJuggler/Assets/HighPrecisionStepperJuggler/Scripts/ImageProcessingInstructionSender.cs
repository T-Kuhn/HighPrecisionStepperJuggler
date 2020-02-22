using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public class ImageProcessingInstructionSender : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private MachineController _machineController;
        [SerializeField] private BallPositionVisualizer _ballPositionVisualizer;

        private BallData _ballData = new BallData();
        private int _currentStrategyIndex;
        [SerializeField] private bool _executeControlStrategies;

        private List<IBallControlStrategy> _ballControlStrategies = new List<IBallControlStrategy>();

        private void Start()
        {
            _ballControlStrategies.Add(BallControlStrategyFactory.GoHighPlate());
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancing(1000));
            _ballControlStrategies.Add(BallControlStrategyFactory.CreateGetBouncing());
            _ballControlStrategies.Add(BallControlStrategyFactory.CreateContinuousBouncing(100));
        }

        private void Update()
        {
            var ballRadiusAndPosition = _cameraPlugin.UpdateImageProcessing();
            var height = FOVCalculations.RadiusToDistance(ballRadiusAndPosition.Radius);

            if (!_executeControlStrategies)
            {
                foreach (var strategy in _ballControlStrategies)
                {
                    strategy.Reset();
                }

                _currentStrategyIndex = 0;
            }

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
            
            _ballPositionVisualizer.SpawnPositionPoint(_ballData.CurrentUnityPositionVector);

            if (_machineController.IsReadyForNextInstruction && _executeControlStrategies)
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
