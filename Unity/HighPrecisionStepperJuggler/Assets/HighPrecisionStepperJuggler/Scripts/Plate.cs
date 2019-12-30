using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class Plate : MonoBehaviour
    {
        [SerializeField] private Transform _motor1Joint2Tip = null;
        [SerializeField] private Transform _motor2Joint2Tip = null;
        [SerializeField] private Transform _motor3Joint2Tip = null;
        [SerializeField] private Transform _motor4Joint2Tip = null;

        void LateUpdate()
        {
            // set height
            var meanHeight = (_motor1Joint2Tip.position.y + _motor2Joint2Tip.position.y) / 2f;
            transform.position = Vector3.up * meanHeight;

            var gamma = MiscMath.TiltFromOpposingPositions(_motor1Joint2Tip.position, _motor2Joint2Tip.position);
            var beta = MiscMath.TiltFromOpposingPositions(_motor3Joint2Tip.position, _motor4Joint2Tip.position);

            transform.localRotation = Quaternion.Euler(beta * Mathf.Rad2Deg, -gamma * Mathf.Rad2Deg, 0f);
        }
    }
}
