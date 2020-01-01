namespace HighPrecisionStepperJuggler
{
    // Low Level Instruction
    public struct LLInstruction
    {
        public LLMachineState TargetMachineState{ get; }
        public float MoveTime { get; }

        public LLInstruction(LLMachineState targetMachineState, float moveTime)
        {
            TargetMachineState = targetMachineState;
            MoveTime = moveTime;
        }
    }
}
