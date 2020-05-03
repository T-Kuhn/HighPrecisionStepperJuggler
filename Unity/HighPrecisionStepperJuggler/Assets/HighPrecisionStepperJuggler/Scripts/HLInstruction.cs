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

        public HLInstruction(float height, float xTilt, float yTilt, float moveTime, bool isLevelingInstruction = false,
            bool isFlexInstruction = false)
        {
            var tiltMaxMultiplier = isFlexInstruction ? 2f : 1f;
            Debug.Log("timestamp: " + Time.realtimeSinceStartup + ", xTilt: " + xTilt.ToString("0.00"));

            TargetHLMachineState = new HLMachineState(
                Mathf.Clamp(height, c.MinPlateHeight, c.MaxPlateHeight),
                Mathf.Clamp(xTilt, c.MinTiltAngle * tiltMaxMultiplier, c.MaxTiltAngle * tiltMaxMultiplier),
                Mathf.Clamp(yTilt, c.MinTiltAngle * tiltMaxMultiplier, c.MaxTiltAngle * tiltMaxMultiplier));
            MoveTime = moveTime;
            IsLevelingInstruction = isLevelingInstruction;
        }

        public static HLInstruction operator +(HLInstruction a, HLInstruction b)
        {
            return new HLInstruction(
                a.TargetHLMachineState + b.TargetHLMachineState,
                a.MoveTime + b.MoveTime,
                a.IsLevelingInstruction && b.IsLevelingInstruction);
        }

        public static HLInstruction operator -(HLInstruction a, HLInstruction b)
        {
            return new HLInstruction(
                a.TargetHLMachineState - b.TargetHLMachineState,
                a.MoveTime - b.MoveTime,
                a.IsLevelingInstruction && b.IsLevelingInstruction);
        }
    }
}
