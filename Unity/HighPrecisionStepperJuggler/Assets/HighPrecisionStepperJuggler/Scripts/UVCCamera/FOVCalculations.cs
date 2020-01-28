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

        // Returns distance from center of plate in either X or Y direction (depending whether the provided)
        // pixelPosition is in the X or Y direction[mm]
        public static float PixelPositionToDistanceFromCenter(float pixelPosition, float distanceFromPlate)
        {
            // NOTE: distance is zero at center of plate
            var distanceFromCamera = distanceFromPlate + c.BallHeightAtOrigin;
            var phi = c.CameraFOVInDegrees / 2f * pixelPosition / (c.CameraResolutionWidth / 2f);
            return Mathf.Tan(phi * Mathf.Deg2Rad) * distanceFromCamera;
        }
    }
}
