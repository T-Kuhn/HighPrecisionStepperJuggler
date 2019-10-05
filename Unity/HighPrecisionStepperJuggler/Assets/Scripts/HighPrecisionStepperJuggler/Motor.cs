using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    // Motor is given a shaftRotation (in radians) and turns both joint1 and joint2
    // Joint1 with the input shaftRotation and joint2 by looking up the InverseKinematics
    [ExecuteInEditMode]
    public class Motor : MonoBehaviour
    {
        public float ShaftRotation;
        
        [SerializeField] private BasicJoint _joint1;
        [SerializeField] private BasicJoint _joint2;

        private void Update()
        {
            _joint1.JointRotation = ShaftRotation;
            _joint2.JointRotation = InverseKinematics.CalculateJoint2RotationFromJoint1Rotation(ShaftRotation);
            
            _joint1.UpdatePositionAndRotation();
            _joint2.UpdatePositionAndRotation();
        }
    }
}
