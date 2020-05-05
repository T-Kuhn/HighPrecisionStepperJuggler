
using UnityEngine;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public static class TiltControlling
    {
        public static (float xTilt, float yTilt) AnalyticallyDeterminedTilt(
            Vector2 position,
            Vector2 velocity,
            Vector2 target,
            BallData ballData)
        {
            float AngleBetween(Vector2 vector1, Vector2 vector2)
            {
                var sin = vector1.x * vector2.y - vector2.x * vector1.y;
                var cos = vector1.x * vector2.x + vector1.y * vector2.y;

                return Mathf.Atan2(sin, cos);
            }

            var v_i = new Vector3(
                velocity.x,
                velocity.y,
                -ballData.CalculatedOnBounceDownwardsVelocity);

            // FIXME: we assume that the control target is in the center of the plate,
            //        change this code in such a way that the target can be anywhere on the plate.
            var v_o = new Vector3(
                (target.x - position.x) / ballData.AirborneTime,
                (target.y - position.y) / ballData.AirborneTime,
                ballData.CalculatedOnBounceDownwardsVelocity);

            var v_c = -v_i.normalized + v_o.normalized;

            var tiltX = -AngleBetween(Vector2.up, new Vector2(v_c.x, v_c.z).normalized) * Mathf.Rad2Deg;
            var tiltY = AngleBetween(Vector2.up, new Vector2(v_c.y, v_c.z).normalized) * Mathf.Rad2Deg;

            return (tiltX, tiltY);
        }
    }
}
