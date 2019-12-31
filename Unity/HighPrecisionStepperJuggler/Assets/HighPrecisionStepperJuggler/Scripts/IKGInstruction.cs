namespace HighPrecisionStepperJuggler
{
    public struct IKGInstruction
    {
        public IKGMachineState TargetMachineState{ get; }
        public float MoveTime { get; }

        public IKGInstruction(IKGMachineState targetMachineState, float moveTime)
        {
            TargetMachineState = targetMachineState;
            MoveTime = moveTime;
        }
    }
}
