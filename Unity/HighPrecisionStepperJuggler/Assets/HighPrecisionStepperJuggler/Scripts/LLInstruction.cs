namespace HighPrecisionStepperJuggler
{
    // Low Level Instruction
    public struct LLInstruction
    {
        public LLMachineState TargetMachineState{ get; }
        public float MoveTime { get; }
        public bool IsLevelingInstruction;

        public LLInstruction(LLMachineState targetMachineState, float moveTime, bool isLevelingInstruction = false)
        {
            TargetMachineState = targetMachineState;
            MoveTime = moveTime;
            IsLevelingInstruction = isLevelingInstruction;
        }
    }
}
