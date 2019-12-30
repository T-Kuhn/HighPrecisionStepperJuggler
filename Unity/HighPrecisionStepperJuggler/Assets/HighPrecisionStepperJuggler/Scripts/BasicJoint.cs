using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BasicJoint : MonoBehaviour
    {
        private enum RotationalAxis
        {
            X,
            Y,
            Z
        }

        [SerializeField] private RotationalAxis _rotAxis;

        [SerializeField] private Transform _tip = null;
        [SerializeField] private Transform _attachToTip = null;

        public float Rotation;

        private Quaternion _startRotation;

        private void Awake()
        {
            _startRotation = transform.localRotation;
        }

        public void UpdatePositionAndRotation()
        {
            switch (_rotAxis)
            {
                case RotationalAxis.X:
                    transform.localRotation = Quaternion.Euler(-Rotation * Mathf.Rad2Deg, 0f, 0f) * _startRotation;
                    break;
                case RotationalAxis.Y:
                    transform.localRotation = Quaternion.Euler(0f, -Rotation * Mathf.Rad2Deg, 0f) * _startRotation;
                    break;
                case RotationalAxis.Z:
                    transform.localRotation = Quaternion.Euler(0f, 0f, -Rotation * Mathf.Rad2Deg) * _startRotation;
                    break;
            }

            if (_attachToTip != null)
            {
                _attachToTip.position = _tip.position;
            }
        }
    }
}
