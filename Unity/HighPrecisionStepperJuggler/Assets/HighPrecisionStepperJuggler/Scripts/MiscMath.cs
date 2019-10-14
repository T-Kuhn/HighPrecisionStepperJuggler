using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class MiscMath
    {
        /// <summary>
        /// Undo the impact the Scale of the root GameObject has on child object world positions
        /// </summary>
        public static float UnScale(float value)
        {
            return value / c.RootObjectScale;
        }

        /// <summary>
        /// Returns the tilt given the heights of two opposing joints of the plate
        /// </summary>
        public static float TiltFromOpposingPositions(Vector3 position1, Vector3 position2)
        {
            var h = c.PlateWidth / 2f;
            var o = (UnScale(position1.y) - UnScale(position2.y)) / 2f;
            return Mathf.Asin(o / h);
        }

        public static float HeightDifferenceFromTilt(float tilt)
        {
            var h = c.PlateWidth;
            var o = Mathf.Sin(tilt) * h;
            return o;
        }
    }
}
