using UnityEngine;
using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    // Motor is given a shaftRotation (in radians) and turns both joint1 and joint2
    // Joint1 with the input shaftRotation and joint2 by looking up the InverseKinematics
    public class Motor : MonoBehaviour
    {
        public float ShaftRotation;
        public bool InverseEllbow;
            
        [SerializeField] private BasicJoint _joint1 = null;
        [SerializeField] private BasicJoint _joint2 = null;
        
        public void UpdateMotor(float offsetQ)
        {
            if (InverseEllbow)
            {
                _joint1.Rotation = Mathf.PI - ShaftRotation;
                _joint2.Rotation = Mathf.PI - ik.CalculateJoint2RotationFromJoint1Rotation(ShaftRotation, offsetQ);
            }
            else
            {
                _joint1.Rotation = ShaftRotation;
                _joint2.Rotation = ik.CalculateJoint2RotationFromJoint1Rotation(ShaftRotation, offsetQ);
            }
            
            _joint1.UpdatePositionAndRotation();
            _joint2.UpdatePositionAndRotation();
        }
    }
}
