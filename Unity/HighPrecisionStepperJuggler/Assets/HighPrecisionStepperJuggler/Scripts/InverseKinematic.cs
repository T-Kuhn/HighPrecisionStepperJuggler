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
            var q = -0.04f;
            
            return (float)(Mathf.PI - Math.Acos((Mathf.Cos(theta1) * c.L1 + q) / c.L2));
        }

        /// <summary>
        /// Get joint1 rotation with a given targetHeight
        /// </summary>
        /// <param name="targetY">The given target height (tip of link2)</param>
        /// <returns>theta1 (rotation of joint1)</returns>
        public static float CalculateJoint1RotationFromTargetY(float targetY)
        {
            //return Mathf.Asin((targetY * targetY + c.L1 * c.L1 - c.L2 * c.L2) / (2f * c.L1 * targetY));

            var q = -0.04f;
            
            var a1 = 1f / (2f * (c.L1 * c.L1 * q * q + c.L1 * c.L1 * targetY * targetY));
            var a2 = c.L2 * c.L2 * c.L1 * q;
            var a3 = Mathf.Sqrt(-Mathf.Pow(c.L2, 4f) * c.L1 * c.L1 * targetY * targetY
                                + 2f * c.L2 * c.L2 * Mathf.Pow(c.L1, 4f) * targetY * targetY
                                + 2f * c.L2 * c.L2 * c.L1 * c.L1 * q * q * targetY * targetY
                                + 2f * c.L1 * c.L1 * c.L2 * c.L2 * Mathf.Pow(targetY, 4f)
                                + Mathf.Pow(c.L1, 6f) * -(targetY * targetY)
                                + 2f * Mathf.Pow(c.L1, 4f) * q * q * targetY * targetY
                                + 2f * Mathf.Pow(c.L1, 4f) * Mathf.Pow(targetY, 4f)
                                - c.L1 * c.L1 * Mathf.Pow(q, 4f) * targetY * targetY
                                - 2f * c.L1 * c.L1 * q * q * Mathf.Pow(targetY, 4f)
                                - c.L1 * c.L1 * Mathf.Pow(targetY, 6f));
            var a4 = Mathf.Pow(c.L1, 3f) * q + c.L1 * Mathf.Pow(q, 3f) + c.L1 * q * targetY*targetY;
            
            // NOTE: k is c.L2
            //cos^(-1)((-k^2 l q +/- sqrt(-k^4 l^2 y^2 + 2 k^2 l^4 y^2 + 2 k^2 l^2 q^2 y^2 + 2 k^2 l^2 y^4
            //    + l^6 (-y^2) + 2 l^4 q^2 y^2 + 2 l^4 y^4 - l^2 q^4 y^2 - 2 l^2 q^2 y^4 - l^2 y^6)
            //    + l^3 q + l q^3 + l q y^2)/(2 (l^2 q^2 + l^2 y^2)))
            return Mathf.Acos(a1 * (-a2 + a3 + a4));
            //return Mathf.Acos(a1 * (-a2 - a3 + a4));
        }
    }
}
