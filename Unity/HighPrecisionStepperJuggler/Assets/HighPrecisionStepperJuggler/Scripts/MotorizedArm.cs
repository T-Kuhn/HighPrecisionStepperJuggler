using UnityEngine;
using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    public class MotorizedArm : MonoBehaviour
    {
        [SerializeField] private BasicJoint _joint1 = null;
        [SerializeField] private BasicJoint _joint2 = null;

        public void UpdateState(float shaftRotation, float joint2Rotation)
        {
            _joint1.UpdatePositionAndRotation(shaftRotation);
            _joint2.UpdatePositionAndRotation(joint2Rotation);
        }
    }
}
