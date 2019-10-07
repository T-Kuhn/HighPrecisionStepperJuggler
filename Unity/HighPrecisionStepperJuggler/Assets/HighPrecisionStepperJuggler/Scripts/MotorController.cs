using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MotorController : MonoBehaviour
    {
        public float StartY;
        public float EndY;
        
        [SerializeField] private Motor _motor1 = null;
        [SerializeField] private Motor _motor2 = null;

        private void Update()
        {
            var startJointRotations = InverseKinematics.CalculateJointRotationsFromTargetY(StartY);
            var endJointRotations = InverseKinematics.CalculateJointRotationsFromTargetY(EndY);

            var totalRotation = endJointRotations.theta1 - startJointRotations.theta1;

            _motor1.ShaftRotation = startJointRotations.theta1 + totalRotation / 10 * ((Mathf.Cos(Time.time * 2) + 1) / 2f);
            _motor2.ShaftRotation = startJointRotations.theta1 + totalRotation  * ((Mathf.Cos(Time.time * 2) + 1) / 2f);
            
            _motor1.UpdateMotor();
            _motor2.UpdateMotor();
        }
    }
}
