namespace HighPrecisionStepperJuggler
{
    public static class Constants
    {
        // Width of plate (in localSpace) from joint to joint
        public const float PlateWidth = 0.299f;

        // Offset from motor axis to upper Joint
        // public static readonly float Q = 0.069023f;
        public const float Q = 0.070023f;

        // Length of Links
        public const float L1 = 0.089f;
        public const float L2 = 0.080f;

        public const float HeightOrigin = 0.0566f;
        public const int BaudRate = 115200;

        public static readonly LLMachineState OriginMachineState = new HLMachineState(0f, 0f, 0f).Translate();
        public static readonly LLMachineState ZeroMachineState = new LLMachineState(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

        public enum ImgMode
        {
            Src, Red, Green, Blue, Normalgray, Customgray
        }
    }
}
