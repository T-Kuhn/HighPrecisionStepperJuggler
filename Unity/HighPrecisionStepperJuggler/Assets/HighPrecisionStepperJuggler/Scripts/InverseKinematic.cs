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
            return (float)(Mathf.PI - Math.Acos(Mathf.Cos(theta1) * c.L1 / c.L2));
        }
        
        /// <summary>
        /// Get joint1 rotation with a given theta1 rotation
        /// </summary>
        /// <param name="targetY">The given target height (tip of link2)</param>
        /// <returns>theta1 (rotation of joint1)</returns>
        public static float CalculateJoint1RotationFromTargetY(float targetY)
        {
            return Mathf.Asin((targetY * targetY + c.L1 * c.L1 - c.L2 * c.L2) / (2f * c.L1 * targetY));
        }
    }
}
