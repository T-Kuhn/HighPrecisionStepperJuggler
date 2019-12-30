using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BasicRotationalJoint : MonoBehaviour
    {
        public float Rotation
        {
            set => transform.localRotation = Quaternion.Euler(value * Mathf.Rad2Deg, 0f, 0f);
        }
    }
}
