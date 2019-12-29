using System;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class InverseKinematics
    {
        /// <summary>
        /// Get theta1 and theta2 with a given target height (tip of link2)
        /// </summary>
        /// <param name="targetY">The given target height (tip of link2)</param>
        /// <returns>theta1 (rotation of joint1) and theta2 (rotation of joint2)</returns>
        public static (float theta1, float theta2) CalculateJointRotationsFromTargetY(float targetY)
        {
            var theta1 = CalculateJoint1RotationFromTargetY(targetY);
            var theta2 = CalculateJoint2RotationFromJoint1Rotation(theta1);

            return (theta1: theta1, theta2: (float)theta2);
        }

        /// <summary>
        /// Get joint2 rotation with a given theta1 rotation
        /// </summary>
        /// <param name="theta1">The rotation of joint1</param>
        /// <returns>theta2 (rotation of joint2)</returns>
        public static float CalculateJoint2RotationFromJoint1Rotation(float theta1)
        {
            var q = 0.07f;
            
            return (float)(Mathf.PI - Math.Acos((Mathf.Cos(theta1) * c.L1 - q) / c.L2));
        }

        /// <summary>
        /// Get joint1 rotation with a given targetHeight
        /// </summary>
        /// <param name="targetY">The given target height (tip of link2)</param>
        /// <returns>theta1 (rotation of joint1)</returns>
        public static float CalculateJoint1RotationFromTargetY(float targetY)
        {
            //return Mathf.Asin((targetY * targetY + c.L1 * c.L1 - c.L2 * c.L2) / (2f * c.L1 * targetY));

            var q = 0.07f;
            
            // y = l_1 sin(x) + sqrt(l_2^2-(-l_1cos(x) + q)^2)
            //     ↓ solve for x (wolfram alpha) ↓
            //
            // x = -cos^(-1)((l_1 (-q) (-4 l_1^2 + 4 l_2^2 - 4 q^2 - 4 y^2)
            //     - 4 sqrt(-l_1^2 q^4 y^2 - 2 l_1^2 q^2 y^4 + 2 l_1^4 q^2 y^2 + 2 l_1^2 l_2^2 q^2 y^2
            //     - l_1^2 y^6 + 2 l_1^4 y^4 + 2 l_1^2 l_2^2 y^4 - l_1^6 y^2 - l_1^2 l_2^4 y^2 + 2 l_1^4 l_2^2 y^2))
            //     /(2 l_1^2 (4 q^2 + 4 y^2)))

            var a1 = 1f / (2f * c.L1 * c.L1 * (4f * q * q + 4f * targetY * targetY));
            var a2 = -c.L1 * q * (-4f * c.L1 * c.L1 + 4f * c.L2 * c.L2 - 4f * q * q - 4 * targetY * targetY);
            var a3 = -4f * Mathf.Sqrt(
                         -c.L1 * c.L1 * q * q * q * q * targetY * targetY
                         - 2f * c.L1 * c.L1 * q * q * targetY * targetY * targetY * targetY
                         + 2f * c.L1 * c.L1 * c.L1 * c.L1 * q * q * targetY * targetY
                         + 2f * c.L1 * c.L1 * c.L2 * c.L2 * q * q * targetY * targetY
                         - c.L1 * c.L1 * targetY * targetY * targetY * targetY * targetY * targetY
                         + 2f * c.L1 * c.L1 * c.L1 * c.L1 * targetY * targetY * targetY * targetY
                         + 2f * c.L1 * c.L1 * c.L2 * c.L2 * targetY * targetY * targetY * targetY
                         - c.L1 * c.L1 * c.L1 * c.L1 * c.L1 * c.L1 * targetY * targetY
                         - c.L1 * c.L1 * c.L2 * c.L2 * c.L2 * c.L2 * targetY * targetY
                         + 2f * c.L1 * c.L1 * c.L1 * c.L1 * c.L2 * c.L2 * targetY * targetY);

            return Mathf.Acos(a1 * (a2 - a3));
        }
    }
}
