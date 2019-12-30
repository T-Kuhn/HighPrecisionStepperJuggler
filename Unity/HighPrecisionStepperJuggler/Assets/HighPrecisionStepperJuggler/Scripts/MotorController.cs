using UnityEngine;
using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    public class MotorController : MonoBehaviour
    {
        public Heights CentreHeigths;
        
        [Range(0.0f, 0.14f)]
        public float StartStopHeight;
        
        [Range(-0.4f, 0.4f)]
        public float XTiltInRadians;
        [Range(-0.4f, 0.4f)]
        public float YTiltInRadians;
        
        [SerializeField] private Motor _motor1 = null;
        [SerializeField] private Motor _motor2 = null;
        [SerializeField] private Motor _motor3 = null;
        [SerializeField] private Motor _motor4 = null;

        private void Update()
        {
            (float startRot, float totalRot) RotationsFromHeights(Heights heights)
            {
                var startRotation = ik.CalculateJoint1RotationFromTargetY(heights.Start);
                var endRotation = ik.CalculateJoint1RotationFromTargetY(heights.End);

                var totalRotation = endRotation - startRotation;

                return (startRot: startRotation, totalRot: totalRotation);
            }

            void UpdateMotor(Motor m, (float startRot, float totalRot) r)
            {
                var cosineFromZeroToOne = ((Mathf.Cos(Time.time * 2) + 1) / 2f);
                
                m.ShaftRotation = r.startRot + r.totalRot * cosineFromZeroToOne;
                m.UpdateMotor();
            }
            
            // Circle tilting
            //XTiltInRadians = Mathf.Sin(Time.time * 3) * 1f;
            //YTiltInRadians = Mathf.Cos(Time.time * 3) * 1f;

            var xHeightDiff = MiscMath.HeightDifferenceFromTilt(XTiltInRadians);
            var yHeightDiff = MiscMath.HeightDifferenceFromTilt(YTiltInRadians);

            if (Mathf.Abs(StartStopHeight) > 0f)
            {
                CentreHeigths.Start = StartStopHeight;
                CentreHeigths.End = StartStopHeight;
            }
            
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
