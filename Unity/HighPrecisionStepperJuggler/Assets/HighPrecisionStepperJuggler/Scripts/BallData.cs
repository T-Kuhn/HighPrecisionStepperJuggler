using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public sealed class BallData
    {
        private float _xDistanceAtReset;
        private float _yDistanceAtReset;
        private float _timeAtReset;
        private Vector3 _lastFramePositionVector;

        // current ball position in [mm]
        public Vector3 CurrentPositionVector => _currentPositionVector;
        
        // current ball velocity in [mm/s]
        public Vector2 CurrentVelocityVector => _currentVelocityVector;

        public Vector3 CurrentUnityPositionVector => new Vector3(
                                                         _currentPositionVector.x,
                                                         _currentPositionVector.z + Constants.BallHeightAtOrigin,
                                                         _currentPositionVector.y) / 1000f;

        private Vector3 _currentPositionVector = Vector3.zero;
        private Vector2 _currentVelocityVector = Vector2.zero;

        private VelocityDebugView _velocityDebugView;

        public BallData(VelocityDebugView velocityDebugView)
        {
            _velocityDebugView = velocityDebugView;
        }

        public void UpdateData(Vector3 positionVector, Vector2 velocityVector)
        {
            _currentPositionVector = positionVector;
            _currentVelocityVector = velocityVector;
            
            _velocityDebugView.Vx = velocityVector.x.ToString("0.000");
            _velocityDebugView.Vy = velocityVector.y.ToString("0.000");
        }
    }
}
