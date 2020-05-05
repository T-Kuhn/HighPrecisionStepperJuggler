using System.Collections.Generic;
using HighPrecisionStepperJuggler.MachineLearning;
using UnityEngine;
using UnityEngine.Serialization;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public class ImageProcessingInstructionSender : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private MachineController _machineController;
        [SerializeField] private BallPositionVisualizer _ballPositionVisualizer;
        [SerializeField] private BallDataDebugView _ballDataDebugView;
        [SerializeField] private PredictedPositionVisualizer _predictedPositionVisualizer;
        [SerializeField] private GradientDescentView _gradientDescentViewX;
        [SerializeField] private GradientDescentView _gradientDescentViewY;
        [SerializeField] private GradientDescentView _gradientDescentViewZ;

        private readonly GradientDescent _gradientDescentX = new GradientDescent(
            Constants.NumberOfTrainingSetsUsedForXYGD,
            Constants.NumberOfGDUpdateCyclesXY,
            Constants.AlphaXY
            );
        private readonly GradientDescent _gradientDescentY = new GradientDescent(
            Constants.NumberOfTrainingSetsUsedForXYGD,
            Constants.NumberOfGDUpdateCyclesXY,
            Constants.AlphaXY
            );
        
        private readonly GradientDescent _gradientDescentZ = new GradientDescent(
            Constants.NumberOfTrainingSetsUsedForHeightGD,
            Constants.NumberOfGDUpdateCyclesHeight,
            Constants.AlphaHeight
            );

        private BallData _ballData;
        private int _currentStrategyIndex;
        [SerializeField] private bool _executeControlStrategies;

        private List<IBallControlStrategy> _ballControlStrategies = new List<IBallControlStrategy>();

        private void Awake()
        {
            _gradientDescentViewX.GradientDescent = _gradientDescentX;
            _gradientDescentViewY.GradientDescent = _gradientDescentY;
            _gradientDescentViewZ.GradientDescent = _gradientDescentZ;
        }

        private void Start()
        {
            _ballData = new BallData(
                _ballDataDebugView,
                _predictedPositionVisualizer,
                _gradientDescentX, 
                _gradientDescentY, 
                _gradientDescentZ);

            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.01f));
            _ballControlStrategies.Add(BallControlStrategyFactory.GoTo(0.05f));
            
            for (int i = 0; i < 5; i++)
            {
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncing(5));
                _ballControlStrategies.Add(BallControlStrategyFactory.ContinuousBouncingStrong(1));
            }
            
            // DEBUG
            _ballControlStrategies.Add(BallControlStrategyFactory.Continuous2StepBouncingWithAnalyticalController(10000));

            _ballControlStrategies.Add(BallControlStrategyFactory.Continuous2StepBouncing(10000));

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
            
            var ballPosX = FOVCalculations.PixelPositionToDistanceFromCenter(ballRadiusAndPosition.PositionX, height);
            _gradientDescentX.Hypothesis.SetTheta_0To(ballPosX);
            _gradientDescentX.AddTrainingSet(new TrainingSet(0f, ballPosX));
            _gradientDescentX.UpdateHypothesis();
            
            var ballPosY = FOVCalculations.PixelPositionToDistanceFromCenter(ballRadiusAndPosition.PositionY, height);
            _gradientDescentY.Hypothesis.SetTheta_0To(ballPosY);
            _gradientDescentY.AddTrainingSet(new TrainingSet(0f, ballPosY));
            _gradientDescentY.UpdateHypothesis();
            
            _gradientDescentZ.Hypothesis.SetTheta_0To(height);
            _gradientDescentZ.AddTrainingSet(new TrainingSet(0f, height));
            _gradientDescentZ.UpdateHypothesis();

            _ballData.UpdateData(
                new Vector3(
                    _gradientDescentX.Hypothesis.Parameters.Theta_0,
                    _gradientDescentY.Hypothesis.Parameters.Theta_0,
                    height), 
                new Vector3(
                    _gradientDescentX.Hypothesis.Parameters.Theta_1,
                    _gradientDescentY.Hypothesis.Parameters.Theta_1,
                    _gradientDescentZ.Hypothesis.Parameters.Theta_1
                    ));

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
