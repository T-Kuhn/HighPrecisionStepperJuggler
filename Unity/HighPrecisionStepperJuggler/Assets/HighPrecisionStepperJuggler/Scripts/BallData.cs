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
        
        public float AirborneTime => _airborneTime;
        private float _airborneTime;

        public float TimeSinceLastBounce => (Time.realtimeSinceStartup - _collisionTime);
        public float PredictedTimeTillNextBounce => _airborneTime - TimeSinceLastBounce;
        
        private float _collisionTime;

        private GradientDescent _gradientDescentX;
        private GradientDescent _gradientDescentY;
        private GradientDescent _gradientDescentZ;

        // current ball position in [mm]
        public Vector3 CurrentPositionVector => _currentPositionVector;

        // current ball velocity in [mm/s]
        public Vector3 CurrentVelocityVector => _currentVelocityVector;

        public Vector3 CurrentUnityPositionVector => new Vector3(
                                                         _currentPositionVector.x,
                                                         _currentPositionVector.z + Constants.BallHeightAtOrigin,
                                                         _currentPositionVector.y) / 1000f;

        private Vector3 _currentPositionVector = Vector3.zero;
        private Vector3 _currentVelocityVector = Vector3.zero;

        private BallDataDebugView _ballDataDebugView;

        public BallData(BallDataDebugView ballDataDebugView, 
            GradientDescent gradientDescentX, 
            GradientDescent gradientDescentY, 
            GradientDescent gradientDescentZ)
        {
            _ballDataDebugView = ballDataDebugView;
            
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
            _ballDataDebugView.TimeTillNextHit = PredictedTimeTillNextBounce.ToString("0.00");
        }
    }
}
