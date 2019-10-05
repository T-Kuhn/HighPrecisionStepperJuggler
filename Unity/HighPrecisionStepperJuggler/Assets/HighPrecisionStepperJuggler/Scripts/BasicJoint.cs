using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    [ExecuteInEditMode]
    public class BasicJoint : MonoBehaviour
    {
        [SerializeField] private Transform _link;
        [SerializeField] private Transform _tip;
        [SerializeField] private Transform _attachToTip;

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
