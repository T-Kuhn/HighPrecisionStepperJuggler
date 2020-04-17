using System.Collections.Generic;

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

        public static float BallHeightAtOrigin = 78f;

        public const float HeightOrigin = 0.0566f;
        public const int BaudRate = 115200;

        public static float CameraFOVInDegrees = 61.6f;
        public static float RadiusOfPingPongBall = 20.0f;

        public static int CameraResolutionWidth = 640;
        public static int CameraResolutionHeight = 480;

        public static readonly LLMachineState OriginMachineState = new HLMachineState(0f, 0f, 0f).Translate();
        public static readonly LLMachineState ZeroMachineState = new LLMachineState(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

        public static float MaxTiltAngle = 0.05f;
        public static float MinTiltAngle = -0.05f;
        public static float MaxPlateHeight = 90f;
        public static float MinPlateHeight = 0f;

        // PD Controller
        public static float k_p = 0.001f;
        public static float k_d = 0.0001f;

        public static float BallVisualizationFadeOutTime = 0.2f;
        public static float SmallBallVisualizationFadeOutTime = 5f;
        
        // GradientDescent
        public static int MaxNumberOfTrainingSets = 70;
        public static int NumberOfTrainingSetsUsedForGD = 10;
        // alpha is what is often referred to as the "learning rate"
        public static float Alpha = 0.1f;

        public enum ImgMode
        {
            Src,
            Red,
            Green,
            Blue,
            Normalgray,
            Customgray,
            CustomgrayWithCirclesOverlayed
        }

        public static List<string> Captions = new List<string>()
        {
            "RGB",
            "Red Channel",
            "Green Channel",
            "Blue Channel",
            "Normal Grayscale",
            "Orange to Grayscale",
            "Hough Transform Circle Detection"
        };
    }
}
