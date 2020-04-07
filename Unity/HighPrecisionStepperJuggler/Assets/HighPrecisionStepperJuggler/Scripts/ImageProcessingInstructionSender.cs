using System.Collections.Generic;
using HighPrecisionStepperJuggler.MachineLearning;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public class ImageProcessingInstructionSender : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private MachineController _machineController;
        [SerializeField] private BallPositionVisualizer _ballPositionVisualizer;
        [SerializeField] private VelocityDebugView _velocityDebugView;
        [SerializeField] private GradientDescentView _gradientDescentView;
        
        private GradientDescent _gradientDescent = new GradientDescent();

        private BallData _ballData;
        private int _currentStrategyIndex;
        [SerializeField] private bool _executeControlStrategies;

        private List<IBallControlStrategy> _ballControlStrategies = new List<IBallControlStrategy>();

        private void Awake()
        {
            _gradientDescentView.GradientDescent = _gradientDescent;
        }

        private void Start()
        {
            _ballData = new BallData(_velocityDebugView);
            
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.01f));
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.05f));
            for (int i = 0; i < 5; i++)
            {
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(5));
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncingStrong(1));
            }

            _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(10000));
            
            for (int i = 0; i < 5; i++)
            {
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(5));
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncingStrong(1));
                _ballControlStrategies.Add(BallControlStrategyFactory.BalancingAtHeight(0.05f, 10));
            }

            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.01f));
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.08f));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(0f, 0f), 15));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateCircleBalancing(40f, 100));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(0f, 0f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(-40f, 0f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(40f, 0f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(0, 40f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(0, -40f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(-35, -35f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(35, 35f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(35, -35f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(-35, 35f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.HighPlateBalancingAt(new Vector2(0f, 0f), 20));
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.01f));
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.05f));

            for (int i = 0; i < 5; i++)
            {
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(5));
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncingStrong(1));
            }

            _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(20));

            for (int i = 0; i < 5; i++)
            {
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(5));
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncingStrong(1));
                _ballControlStrategies.Add(BallControlStrategyFactory.BalancingAtHeight(0.05f, 10));
            }
            
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.01f));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _executeControlStrategies = !_executeControlStrategies;
            }
            
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

            _gradientDescent.AddTrainingSet(new TrainingSet(-Time.deltaTime * 1000f, _ballData.CurrentPositionVector.y / 2f));
            
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
