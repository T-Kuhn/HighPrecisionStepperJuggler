using System;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class InverseKinematic : MonoBehaviour
    {
        public float TargetY;
        
        [SerializeField] private BasicJoint _joint1;
        [SerializeField] private BasicJoint _joint2;

        private const float L1 = 0.004f;
        private const float L2 = 0.005f;

        private void Update()
        {
            TargetY = (Mathf.Cos(Time.time) + 1)/2.0f * 0.004f + 0.003f;
            
            var theta1 = Mathf.Asin((TargetY * TargetY + L1 * L1 - L2 * L2) / (2f * L1 * TargetY));
            _joint1.JointRotation = theta1;

            var theta2 = Mathf.PI - Math.Acos(Mathf.Cos(theta1) * L1 / L2);
            _joint2.JointRotation = (float)theta2;
        }
    }
}
