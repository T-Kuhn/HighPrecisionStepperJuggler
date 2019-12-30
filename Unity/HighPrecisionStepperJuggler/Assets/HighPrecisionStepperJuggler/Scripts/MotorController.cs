using UnityEngine;
using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    public class MotorController : MonoBehaviour
    {
        public Heights CentreHeigths;
        
        [Range(0.0f, 0.14f)]
        public float StartStopHeight;
        
        [Range(-0.2f, 0.2f)]
        public float XTiltInRadians;
        
        [Range(-0.2f, 0.2f)]
        public float YTiltInRadians;
        
        [SerializeField] private MotorizedArm _motor1 = null;
        [SerializeField] private MotorizedArm _motor2 = null;
        [SerializeField] private MotorizedArm _motor3 = null;
        [SerializeField] private MotorizedArm _motor4 = null;

        private void Update()
        {
            (float startRot, float totalRot) RotationsFromHeights(Heights heights, float offsetQ)
            {
                var startRotation = ik.CalculateJoint1RotationFromTargetY(heights.Start, offsetQ);
                var endRotation = ik.CalculateJoint1RotationFromTargetY(heights.End, offsetQ);

                var totalRotation = endRotation - startRotation;

                return (startRot: startRotation, totalRot: totalRotation);
            }

            void UpdateMotor(MotorizedArm m, (float startRot, float totalRot) r, float offsetQ)
            {
                var cosineFromZeroToOne = ((Mathf.Cos(Time.time * 2) + 1) / 2f);
                
                var shaftRotation = r.startRot + r.totalRot * cosineFromZeroToOne;
                m.UpdateArm(shaftRotation, offsetQ);
            }
            
            var xHeightDiff = MiscMath.HeightDifferenceFromTilt(XTiltInRadians);
            var yHeightDiff = MiscMath.HeightDifferenceFromTilt(YTiltInRadians);

            if (Mathf.Abs(StartStopHeight) > 0f)
            {
                CentreHeigths.Start = StartStopHeight;
                CentreHeigths.End = StartStopHeight;
            }

            {
                var q = MiscMath.WidthDifferenceFromTilt(XTiltInRadians);
                UpdateMotor(_motor1, RotationsFromHeights(new Heights()
                {
                    Start = CentreHeigths.Start + xHeightDiff / 2,
                    End = CentreHeigths.End + xHeightDiff / 2
                }, q), q);
            }

            {
                var q = MiscMath.WidthDifferenceFromTilt(XTiltInRadians);
                UpdateMotor(_motor2, RotationsFromHeights(new Heights()
                {
                    Start = CentreHeigths.Start - xHeightDiff / 2,
                    End = CentreHeigths.End - xHeightDiff / 2
                }, q), q);
            }

            {
                var q = MiscMath.WidthDifferenceFromTilt(YTiltInRadians);
                UpdateMotor(_motor3, RotationsFromHeights(new Heights()
                {
                    Start = CentreHeigths.Start + yHeightDiff / 2,
                    End = CentreHeigths.End + yHeightDiff / 2
                }, q), q);
            }

            {
                var q = MiscMath.WidthDifferenceFromTilt(YTiltInRadians);
                UpdateMotor(_motor4, RotationsFromHeights(new Heights()
                {
                    Start = CentreHeigths.Start - yHeightDiff / 2,
                    End = CentreHeigths.End - yHeightDiff / 2
                }, q), q);
            }
        }
    }

    [System.Serializable]
    public struct Heights
    {
        public float Start;
        public float End;
    }
}
