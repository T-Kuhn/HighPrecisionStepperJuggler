using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    [ExecuteInEditMode]
    public class MotorController : MonoBehaviour
    {
        public float StartY;
        public float EndY;
        
        [SerializeField] private Motor _motor1;
        [SerializeField] private Motor _motor2;

        private void Update()
        {
            var startJointRotations = InverseKinematics.CalculateJointRotationsFromTargetY(StartY);
            var endJointRotations = InverseKinematics.CalculateJointRotationsFromTargetY(EndY);

            var totalRotation = endJointRotations.theta1 - startJointRotations.theta1;

            _motor1.ShaftRotation = startJointRotations.theta1 + totalRotation * (Mathf.Cos(Time.time * 2) + 1) / 2;
            _motor2.ShaftRotation = startJointRotations.theta1 + totalRotation * (Mathf.Cos(Time.time * 2) + 1) / 2;
        }
    }
}
