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

        private GradientDescent _gradientDescentX;
        private GradientDescent _gradientDescentY;

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

        private VelocityDebugView _velocityDebugView;

        public BallData(VelocityDebugView velocityDebugView, 
            GradientDescent gradientDescentX, GradientDescent gradientDescentY)
        {
            _velocityDebugView = velocityDebugView;
            
            _gradientDescentX = gradientDescentX;
            _gradientDescentY = gradientDescentY;
        }

        public void UpdateData(Vector3 positionVector, Vector3 velocityVector)
        {
            _currentPositionVector = positionVector;
            _currentVelocityVector = velocityVector;

            _velocityDebugView.Vx = velocityVector.x.ToString("0.000");
            _velocityDebugView.Vy = velocityVector.y.ToString("0.000");
        }
    }
}
