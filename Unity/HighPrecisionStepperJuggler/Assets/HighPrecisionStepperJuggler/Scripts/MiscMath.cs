using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class MiscMath
    {
        /// <summary>
        /// Scale according to RootObjectScale
        /// </summary>
        private static float ToObjectScale(this float value)
        {
            return value * c.RootObjectScale;
        }

        /// <summary>
        /// Returns the tilt given the heights of two opposing joints of the plate
        /// </summary>
        public static float TiltFromOpposingPositions(Vector3 position1, Vector3 position2)
        {
            var h = c.PlateWidth / 2f;
            var o = (position1.y.ToObjectScale() - position2.y.ToObjectScale()) / 2f;
            return Mathf.Asin(o / h);
        }

        public static float HeightDifferenceFromTilt(float tilt)
        {
            var h = c.PlateWidth;
            var o = Mathf.Sin(tilt) * h;
            return o;
        }

        /*public static float WidthCorrectionFromTilt(float tilt)
        {
            
        }
        */
    }
}
