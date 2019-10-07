using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BasicJoint : MonoBehaviour
    {
        [SerializeField] private Transform _tip = null;
        [SerializeField] private Transform _attachToTip = null;

        public float JointRotation;

        public void UpdatePositionAndRotation()
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, JointRotation * Mathf.Rad2Deg);
            
            if (_attachToTip != null)
            {
                _attachToTip.position = _tip.position;
            }
        }
    }
}
