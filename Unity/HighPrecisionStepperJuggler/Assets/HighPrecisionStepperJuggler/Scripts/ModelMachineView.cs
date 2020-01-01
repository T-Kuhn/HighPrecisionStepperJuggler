using UnityEngine;
using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    public class ModelMachineView : MonoBehaviour
    {
        [SerializeField] private MotorizedArm _motorizedArm1 = null;
        [SerializeField] private MotorizedArm _motorizedArm2 = null;
        [SerializeField] private MotorizedArm _motorizedArm3 = null;
        [SerializeField] private MotorizedArm _motorizedArm4 = null;

        public void SetMachineState(LLMachineState state)
        {
            _motorizedArm1.UpdateState(state.Motor1Rotation, state.Arm1Joint2Rotation);
            _motorizedArm2.UpdateState(state.Motor2Rotation, state.Arm2Joint2Rotation);
            _motorizedArm3.UpdateState(state.Motor3Rotation, state.Arm3Joint2Rotation);
            _motorizedArm4.UpdateState(state.Motor4Rotation, state.Arm4Joint2Rotation);
        }
    }
}
