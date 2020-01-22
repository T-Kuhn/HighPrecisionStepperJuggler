using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class FOVCalculations
    {
        public static float RadiusToDistance(float radius)
        {
            // NOTE: Distance is zero at machines origin position
            return c.RadiusOfPingPongBall /
                   Mathf.Tan((radius / c.CameraResolutionWidth) * c.CameraFOVInDegrees * Mathf.Deg2Rad) -
                   c.BallHeightAtOrigin;
        }
    }
}
