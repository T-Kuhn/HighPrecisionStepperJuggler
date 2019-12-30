namespace HighPrecisionStepperJuggler
{
    // High Level Instruction
    public struct HLInstruction
    {
        public float Height { get; }
        public float XTilt { get; }
        public float YTilt { get; }
        public float MoveTime { get; }

        public HLInstruction(float height, float xTilt, float yTilt, float moveTime)
        {
            Height = height;
            XTilt = xTilt;
            YTilt = yTilt;
            MoveTime = moveTime;
        }
    }
}
