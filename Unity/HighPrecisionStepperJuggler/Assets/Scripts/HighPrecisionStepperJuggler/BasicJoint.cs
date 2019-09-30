using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BasicJoint : MonoBehaviour
    {
        [SerializeField] private Transform _link;
        [SerializeField] private Transform _tip;
        [SerializeField] private Transform _attachToTip;

        public float JointRotation;

        private void Update()
        {
            if (_attachToTip != null)
            {
                _attachToTip.position = _tip.position;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, JointRotation * Mathf.Rad2Deg);
        }
    }
}
