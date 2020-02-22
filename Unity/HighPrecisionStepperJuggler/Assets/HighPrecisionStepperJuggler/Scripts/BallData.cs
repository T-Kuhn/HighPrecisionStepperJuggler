using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public sealed class BallData
    {
        private float _xDistanceAtReset;
        private float _yDistanceAtReset;
        private float _timeAtReset;
        private Vector3 _lastFramePositionVector;

        public Vector3 CurrentPositionVector => _currentPositionVector;
        public Vector3 CurrentUnityPositionVector => new Vector3(_currentPositionVector.x, _currentPositionVector.z + Constants.BallHeightAtOrigin, _currentPositionVector.y) / 1000f;
        private Vector3 _currentPositionVector = Vector3.zero;

        public BallData()
        {
        }

        public void UpdateData(Vector3 positionVector, bool machineReadyForNextMove)
        {
            _currentPositionVector = positionVector;
        }

        public Vector2 GetVelocityVector()
        {
            return new Vector2(
                (CurrentPositionVector.x - _xDistanceAtReset) / (Time.realtimeSinceStartup - _timeAtReset),
                (CurrentPositionVector.y - _yDistanceAtReset) / (Time.realtimeSinceStartup - _timeAtReset));
        }

        public void ResetVelocityAccumulation()
        {
            _xDistanceAtReset = CurrentPositionVector.x;
            _yDistanceAtReset = CurrentPositionVector.y;
            _timeAtReset = Time.realtimeSinceStartup;
        }
    }
}
