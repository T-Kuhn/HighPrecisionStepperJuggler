using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    [ExecuteInEditMode]
    public class MotorController : MonoBehaviour
    {
        [SerializeField] private Motor _motor1;

        private void Update()
        {
            //InverseKinematics.CalculateJointRotationsFromTargetY()
            _motor1.ShaftRotation = Time.time;

        }
    }
}
