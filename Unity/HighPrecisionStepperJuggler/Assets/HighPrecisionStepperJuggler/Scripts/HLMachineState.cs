namespace HighPrecisionStepperJuggler
{
    // High Level Machine State
    public struct HLMachineState
    {
        public float Height { get; }
        public float XTilt { get; }
        public float YTilt { get; }

        public HLMachineState(float height, float xTilt, float yTilt)
        {
            Height = height;
            XTilt = xTilt;
            YTilt = yTilt;
        }

        public static HLMachineState operator +(HLMachineState a, HLMachineState b)
        {
            return new HLMachineState(
                a.Height + b.Height,
                a.XTilt + b.XTilt,
                a.YTilt + b.YTilt);
        }

        public static HLMachineState operator -(HLMachineState a, HLMachineState b)
        {
            return new HLMachineState(
                a.Height - b.Height,
                a.XTilt - b.XTilt,
                a.YTilt - b.YTilt);
        }
    }
}
