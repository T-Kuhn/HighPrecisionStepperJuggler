using HighPrecisionStepperJuggler.MachineLearning;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public sealed class BallData
    {
        private float _xDistanceAtReset;
        private float _yDistanceAtReset;
        private float _timeAtReset;
        private Vector3 _lastFramePositionVector;

        public bool BallIsMovingUp => _ballIsMovingUp;
        private bool _ballIsMovingUp;
        
        public float AirborneTime => Mathf.Clamp(_airborneTime, 0.1f, 2f);
        private float _airborneTime;

        public float TimeSinceLastBounce => (Time.realtimeSinceStartup - _collisionTime);
        public float PredictedTimeTillNextHit => _airborneTime - TimeSinceLastBounce;
        
        private float _collisionTime;

        private GradientDescent _gradientDescentX;
        private GradientDescent _gradientDescentY;
        private GradientDescent _gradientDescentZ;

        // current ball position in [mm]
        public Vector3 CurrentPositionVector => _currentPositionVector;

        // current ball velocity in [mm/s]
        public Vector3 CurrentVelocityVector => _currentVelocityVector;


        private Vector2 _predictedPositionVectorOnHit = Vector3.zero;
        public Vector2 PredictedPositionVectorOnHit
        {
            get
            {
                _predictedPositionVisualizer.Visualize(_predictedPositionVectorOnHit);
                return _predictedPositionVectorOnHit;
            }
        }

        // NOTE: the units are [mm/s] 
        public float CalculatedOnBounceDownwardsVelocity => (AirborneTime * 9.81f) / 2f * 1000f;

        public Vector3 CurrentUnityPositionVector => new Vector3(
                                                         _currentPositionVector.x,
                                                         _currentPositionVector.z + Constants.BallHeightAtOrigin,
                                                         _currentPositionVector.y) / 1000f;

        private Vector3 _currentPositionVector = Vector3.zero;
        private Vector3 _currentVelocityVector = Vector3.zero;

        private BallDataDebugView _ballDataDebugView;
        private PredictedPositionVisualizer _predictedPositionVisualizer;

        public BallData(
            BallDataDebugView ballDataDebugView, 
            PredictedPositionVisualizer predictedPositionVisualizer,
            GradientDescent gradientDescentX, 
            GradientDescent gradientDescentY, 
            GradientDescent gradientDescentZ)
        {
            _ballDataDebugView = ballDataDebugView;
            _predictedPositionVisualizer = predictedPositionVisualizer;
            
            _gradientDescentX = gradientDescentX;
            _gradientDescentY = gradientDescentY;
            _gradientDescentZ = gradientDescentZ;
        }

        public void UpdateData(Vector3 positionVector, Vector3 velocityVector)
        {
            _currentPositionVector = positionVector;
            _currentVelocityVector = velocityVector;

            var ballWasMovingUpLastFrame = _ballIsMovingUp;
            _ballIsMovingUp = _gradientDescentZ.Hypothesis.Parameters.Theta_1 > 0.0f;

            if (!ballWasMovingUpLastFrame && _ballIsMovingUp)
            {
                // ball has hit the plate and is now moving up again
                _airborneTime = Time.realtimeSinceStartup - _collisionTime;

                _collisionTime = Time.realtimeSinceStartup;
            }

            _ballDataDebugView.AirborneTime = AirborneTime.ToString("0.00");
            _ballDataDebugView.TimeTillNextHit = PredictedTimeTillNextHit.ToString("0.00");

            _predictedPositionVectorOnHit = new Vector2(
                _currentPositionVector.x + _gradientDescentX.Hypothesis.Parameters.Theta_1 * PredictedTimeTillNextHit,
                _currentPositionVector.y + _gradientDescentY.Hypothesis.Parameters.Theta_1 * PredictedTimeTillNextHit
            );
        }
    }
}
