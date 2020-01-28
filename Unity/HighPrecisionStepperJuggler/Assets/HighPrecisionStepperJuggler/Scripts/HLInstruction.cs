using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    // High Level Instruction
    public struct HLInstruction
    {
        public HLMachineState TargetHLMachineState { get; }
        public float MoveTime { get; }

        public HLInstruction(HLMachineState targetMachineState, float moveTime)
        {
            TargetHLMachineState = targetMachineState;
            MoveTime = moveTime;
        }

        public HLInstruction(float height, float xTilt, float yTilt, float moveTime)
        {
            TargetHLMachineState = new HLMachineState(
                Mathf.Clamp(height, c.MinPlateHeight, c.MaxPlateHeight), 
                Mathf.Clamp(xTilt, c.MinTiltAngle, c.MaxTiltAngle) ,
                Mathf.Clamp(yTilt, c.MinTiltAngle, c.MaxTiltAngle));
            MoveTime = moveTime;
        }
    }
}
