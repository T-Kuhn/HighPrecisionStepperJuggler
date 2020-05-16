using System;
using System.Collections.Generic;
using HighPrecisionStepperJuggler.MachineLearning;
using UniRx;
using UnityEngine;
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
        [SerializeField] private TargetVisualizer _targetVisualizer;

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

        private ReactiveProperty<bool> _isExecuteControlStrategies = new ReactiveProperty<bool>();
        public IObservable<bool> OnExecutingControlStrategies => _isExecuteControlStrategies;

        private ReplaySubject<int> _onCheckPointPassedSubject = new ReplaySubject<int>();
        public IObservable<int> OnCheckPointPassed => _onCheckPointPassedSubject;

        private List<IBallControlStrategy> _strategies = new List<IBallControlStrategy>();

        private void Awake()
        {
            _gradientDescentViewX.GradientDescent = _gradientDescentX;
            _gradientDescentViewY.GradientDescent = _gradientDescentY;
            _gradientDescentViewZ.GradientDescent = _gradientDescentZ;

            AnalyticalTiltController.Instance.TargetVisualizer = _targetVisualizer;
            PIDTiltController.Instance.TargetVisualizer = _targetVisualizer;

            _onCheckPointPassedSubject.OnNext(0);
        }

        private void Start()
        {
            _ballData = new BallData(
                _ballDataDebugView,
                _predictedPositionVisualizer,
                _gradientDescentX,
                _gradientDescentY,
                _gradientDescentZ);

            _strategies.Add(BallControlStrategyFactory.GoTo(0.01f));
            _strategies.Add(BallControlStrategyFactory.GoTo(0.05f));

            _strategies.Add(BallControlStrategyFactory.GoToWhenBallOnPlate(0.01f));
            _strategies.Add(BallControlStrategyFactory.GoToWhenBallOnPlate(0.05f));

            GetBallBouncing(() => _onCheckPointPassedSubject.OnNext(1));
            

            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance));

            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(40, AnalyticalTiltController.Instance,
                action: () => _onCheckPointPassedSubject.OnNext(2)));
            
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                action: () => _onCheckPointPassedSubject.OnNext(3)));
            
            CircleBouncing(5);

            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(40f, 0f)));
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(0f, 0f)));
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(-40f, 0f)));
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(0f, 0f)));
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(40f, 0f)));
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(0f, 0f)));
            _strategies.Add(BallControlStrategyFactory.TwoStepBouncing(20, AnalyticalTiltController.Instance,
                new Vector2(-40f, 0f)));

            for (int i = 0; i < 5; i++)
            {
                _strategies.Add(
                    BallControlStrategyFactory.Bouncing(5, AnalyticalTiltController.Instance));
                _strategies.Add(
                    BallControlStrategyFactory.BouncingStrong(1, AnalyticalTiltController.Instance));
                _strategies.Add(BallControlStrategyFactory.Balancing(0.05f, 8, Vector2.zero,
                    AnalyticalTiltController.Instance));
            }

            _strategies.Add(BallControlStrategyFactory.GoTo(0.01f));
            _strategies.Add(BallControlStrategyFactory.GoTo(0.08f));
            _strategies.Add(BallControlStrategyFactory.GoTo(0.05f));

            GetBallBouncing();

            _strategies.Add(BallControlStrategyFactory.Bouncing(20, AnalyticalTiltController.Instance));

            for (int i = 0; i < 5; i++)
            {
                _strategies.Add(
                    BallControlStrategyFactory.Bouncing(5, AnalyticalTiltController.Instance));
                _strategies.Add(
                    BallControlStrategyFactory.BouncingStrong(1, AnalyticalTiltController.Instance));
                _strategies.Add(
                    BallControlStrategyFactory.Balancing(0.05f, 8, Vector2.zero, AnalyticalTiltController.Instance));
            }

            _strategies.Add(BallControlStrategyFactory.GoTo(0.01f));
        }

        private void GetBallBouncing(Action action = null)
        {
            _strategies.Add(
                BallControlStrategyFactory.Bouncing(5, PIDTiltController.Instance, action: action));
            _strategies.Add(
                BallControlStrategyFactory.BouncingStrong(1, PIDTiltController.Instance));

            for (int i = 0; i < 4; i++)
            {
                _strategies.Add(BallControlStrategyFactory.Bouncing(5, PIDTiltController.Instance));
                _strategies.Add(BallControlStrategyFactory.BouncingStrong(1, PIDTiltController.Instance));
            }
        }

        private void CircleBouncing(int iterations = 1)
        {
            var angle = 0f;
            var radius = 30f;
            var numberOfSlices = 15;
            var target = Vector2.zero;

            for (int j = 0; j < iterations; j++)
            {
                for (int i = 0; i < numberOfSlices; i++)
                {
                    angle = Mathf.PI * 2 * i / numberOfSlices;
                    target.x = Mathf.Cos(angle) * radius;
                    target.y = Mathf.Sin(angle) * radius;
                    _strategies.Add(
                        BallControlStrategyFactory.StepBouncing_Down(AnalyticalTiltController.Instance, target));
                    _strategies.Add(
                        BallControlStrategyFactory.StepBouncing_Up(AnalyticalTiltController.Instance, target));
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isExecuteControlStrategies.Value = !_isExecuteControlStrategies.Value;
            }

            var ballRadiusAndPosition = _cameraPlugin.UpdateImageProcessing();
            var height = FOVCalculations.RadiusToDistance(ballRadiusAndPosition.Radius);

            if (!_isExecuteControlStrategies.Value)
            {
                foreach (var strategy in _strategies)
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

            if (_machineController.IsReadyForNextInstruction && _isExecuteControlStrategies.Value)
            {
                var isRequestingNextStrategy =
                    _strategies[_currentStrategyIndex].Execute(_ballData, _machineController);

                if (isRequestingNextStrategy)
                {
                    _strategies[_currentStrategyIndex].Reset();

                    if (_currentStrategyIndex < _strategies.Count - 1)
                    {
                        _currentStrategyIndex++;

                        _predictedPositionVisualizer
                            .SetActive(_strategies[_currentStrategyIndex].UsesBallPositionPrediction);

                        _strategies[_currentStrategyIndex].OnStrategyExecutionStart?.Invoke();
                    }
                }
            }
        }
    }
}
