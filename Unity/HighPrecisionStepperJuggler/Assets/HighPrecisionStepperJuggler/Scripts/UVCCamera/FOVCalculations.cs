using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class FOVCalculations
    {
        // Returns distance from plate in [mm]
        public static float RadiusToDistance(float radius)
        {
            // NOTE: Distance (height) from plate is zero at machines origin position
            return c.RadiusOfPingPongBall /
                   Mathf.Tan((radius / c.CameraResolutionWidth) * c.CameraFOVInDegrees * Mathf.Deg2Rad) -
                   c.BallHeightAtOrigin;
        }

        // Returns distance from center of place in the x direction in [mm]
        public static float XPixelPositionToXDistance(float xPixelPosition, float distanceFromPlate)
        {
            // NOTE: XDistance is zero at center of plate
            var distanceFromCamera = distanceFromPlate + c.BallHeightAtOrigin;
            var phi = c.CameraFOVInDegrees / 2f * xPixelPosition / (c.CameraResolutionWidth / 2f);
            Debug.Log("phi: " + phi);
            return Mathf.Tan(phi * Mathf.Deg2Rad) * distanceFromCamera;
        }
    }
}
