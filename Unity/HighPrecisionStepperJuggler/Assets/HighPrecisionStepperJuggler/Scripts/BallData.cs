using UnityEngine;
using HighPrecisionStepperJuggler.MachineLearning;

namespace HighPrecisionStepperJuggler
{
    public sealed class BallData
    {
        private float _xDistanceAtReset;
        private float _yDistanceAtReset;
        private float _timeAtReset;
        private Vector3 _lastFramePositionVector;
        private GradientDescent _gradientDescent = new GradientDescent();

        // current ball position in [mm]
        public Vector3 CurrentPositionVector => _currentPositionVector;

        public Vector3 CurrentUnityPositionVector => new Vector3(
                                                         _currentPositionVector.x,
                                                         _currentPositionVector.z + Constants.BallHeightAtOrigin,
                                                         _currentPositionVector.y) / 1000f;

        private Vector3 _currentPositionVector = Vector3.zero;

        private VelocityDebugView _velocityDebugView;

        public BallData(VelocityDebugView velocityDebugView)
        {
            _velocityDebugView = velocityDebugView;
        }

        public void UpdateData(Vector3 positionVector, bool machineReadyForNextMove)
        {
            _currentPositionVector = positionVector;
        }

        public Vector2 GetVelocityVector()
        {
            var v = new Vector2(
                (CurrentPositionVector.x - _xDistanceAtReset) / (Time.realtimeSinceStartup - _timeAtReset),
                (CurrentPositionVector.y - _yDistanceAtReset) / (Time.realtimeSinceStartup - _timeAtReset));

            _velocityDebugView.Vx = v.x.ToString("0.000");
            _velocityDebugView.Vy = v.y.ToString("0.000");

            return v;
        }

        public void ResetVelocityAccumulation()
        {
            _xDistanceAtReset = CurrentPositionVector.x;
            _yDistanceAtReset = CurrentPositionVector.y;
            _timeAtReset = Time.realtimeSinceStartup;
        }
    }
}
