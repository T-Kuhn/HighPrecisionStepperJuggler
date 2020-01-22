using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class FOVCalculations
    {
        public static float RadiusToDistance(float radius)
        {
            return c.RadiusOfPingPongBall /
                   Mathf.Tan((radius / c.CameraResolutionWidth) * c.CameraFOVInDegrees * Mathf.Deg2Rad) -
                   c.BallHeightAtOrigin;
        }
    }
}
