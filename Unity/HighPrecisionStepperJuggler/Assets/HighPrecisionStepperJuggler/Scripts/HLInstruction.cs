using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    // High Level Instruction
    public struct HLInstruction
    {
        public HLMachineState TargetHLMachineState { get; }
        public float MoveTime { get; }
        public bool IsLevelingInstruction;

        public HLInstruction(HLMachineState targetMachineState, float moveTime, bool isLevelingInstruction = false)
        {
            TargetHLMachineState = targetMachineState;
            MoveTime = moveTime;
            IsLevelingInstruction = isLevelingInstruction;
        }

        public HLInstruction(float height, float xTilt, float yTilt, float moveTime, bool isLevelingInstruction = false)
        {
            TargetHLMachineState = new HLMachineState(
                Mathf.Clamp(height, c.MinPlateHeight, c.MaxPlateHeight), 
                Mathf.Clamp(xTilt, c.MinTiltAngle, c.MaxTiltAngle) ,
                Mathf.Clamp(yTilt, c.MinTiltAngle, c.MaxTiltAngle));
            MoveTime = moveTime;
            IsLevelingInstruction = isLevelingInstruction;
        }
    }
}
