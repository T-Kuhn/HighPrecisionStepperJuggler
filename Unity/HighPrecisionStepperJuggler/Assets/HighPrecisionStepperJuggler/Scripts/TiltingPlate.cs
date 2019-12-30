using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class TiltingPlate : MonoBehaviour
    {
        [SerializeField] private Transform _motor1Joint2Tip = null;
        [SerializeField] private Transform _motor2Joint2Tip = null;
        [SerializeField] private Transform _motor3Joint2Tip = null;
        [SerializeField] private Transform _motor4Joint2Tip = null;

        [SerializeField] private BasicRotationalJoint _upperMostRotationalJoint1;
        [SerializeField] private BasicRotationalJoint _upperMostRotationalJoint2;
        [SerializeField] private BasicRotationalJoint _upperMostRotationalJoint3;
        [SerializeField] private BasicRotationalJoint _upperMostRotationalJoint4;

        void LateUpdate()
        {
            var meanHeight = (_motor1Joint2Tip.position.y + _motor2Joint2Tip.position.y) / 2f;
            transform.position = Vector3.up * meanHeight;

            var gamma = MiscMath.TiltFromOpposingPositions(_motor1Joint2Tip.position, _motor2Joint2Tip.position);
            var beta = MiscMath.TiltFromOpposingPositions(_motor3Joint2Tip.position, _motor4Joint2Tip.position);

            transform.localRotation = Quaternion.Euler(beta * Mathf.Rad2Deg, -gamma * Mathf.Rad2Deg, 0f);

            _upperMostRotationalJoint1.Rotation = -beta;
            _upperMostRotationalJoint2.Rotation = beta;
            _upperMostRotationalJoint3.Rotation = gamma;
            _upperMostRotationalJoint4.Rotation = -gamma;
        }
    }
}
