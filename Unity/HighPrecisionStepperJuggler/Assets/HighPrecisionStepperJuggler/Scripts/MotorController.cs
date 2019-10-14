using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MotorController : MonoBehaviour
    {
        public Heights CentreHeigths;
        public float XTilt;
        public float YTilt;
        
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

            var xHeightDiff = MiscMath.HeightDifferenceFromTilt(XTilt);
            var yHeightDiff = MiscMath.HeightDifferenceFromTilt(YTilt);

            UpdateMotor(_motor1, RotationsFromHeights(new Heights()
            {
                Start = CentreHeigths.Start + xHeightDiff / 2, 
                End = CentreHeigths.End + xHeightDiff / 2
            }));
            UpdateMotor(_motor2, RotationsFromHeights(new Heights()
            {
                Start = CentreHeigths.Start - xHeightDiff / 2,
                End = CentreHeigths.End - xHeightDiff / 2
            }));
            UpdateMotor(_motor3, RotationsFromHeights(new Heights()
            {
                Start = CentreHeigths.Start + yHeightDiff / 2,
                End = CentreHeigths.End + yHeightDiff / 2
            }));
            UpdateMotor(_motor4, RotationsFromHeights(new Heights()
            {
                Start = CentreHeigths.Start - yHeightDiff / 2,
                End = CentreHeigths.End - yHeightDiff / 2
            }));
        }
    }

    [System.Serializable]
    public struct Heights
    {
        public float Start;
        public float End;
    }
}
