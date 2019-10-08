using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MotorController : MonoBehaviour
    {
        public Heights Motor1Heights;
        public Heights Motor2Heights;
        public Heights Motor3Heights;
        public Heights Motor4Heights;
        
        [SerializeField] private Motor _motor1 = null;
        [SerializeField] private Motor _motor2 = null;
        [SerializeField] private Motor _motor3 = null;
        [SerializeField] private Motor _motor4 = null;

        private void Update()
        {
            (float startRot, float totalRot) RotationsFromHeights(Heights heights)
            {
                var startRotation = InverseKinematics.CalculateJoint1RotationFromTargetY(heights.Start);
                var endRotation = InverseKinematics.CalculateJoint1RotationFromTargetY(heights.End);

                var totalRotation = endRotation - startRotation;

                return (startRot: startRotation, totalRot: totalRotation);
            }

            void UpdateMotor(Motor m, (float startRot, float totalRot) r)
            {
                var cosineFromZeroToOne = ((Mathf.Cos(Time.time * 2) + 1) / 2f);
                
                m.ShaftRotation = r.startRot + r.totalRot * cosineFromZeroToOne;
                m.UpdateMotor();
            }

            UpdateMotor(_motor1, RotationsFromHeights(Motor1Heights));
            UpdateMotor(_motor2, RotationsFromHeights(Motor2Heights));
            UpdateMotor(_motor3, RotationsFromHeights(Motor3Heights));
            UpdateMotor(_motor4, RotationsFromHeights(Motor4Heights));
        }
    }

    [System.Serializable]
    public struct Heights
    {
        public float Start;
        public float End;
    }
}
