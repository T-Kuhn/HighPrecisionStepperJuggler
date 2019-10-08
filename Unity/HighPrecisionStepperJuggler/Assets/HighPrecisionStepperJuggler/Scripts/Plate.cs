using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class Plate : MonoBehaviour
    {
        private static readonly float _plateWidth = 0.08f;
        
        [SerializeField] private Transform _motor1Joint2Tip = null;
        [SerializeField] private Transform _motor2Joint2Tip = null;
        [SerializeField] private Transform _motor3Joint2Tip = null;
        [SerializeField] private Transform _motor4Joint2Tip = null;

        void LateUpdate()
        {
            // set height
            var meanHeight = (_motor1Joint2Tip.position.y + _motor2Joint2Tip.position.y) / 2f;
            transform.position = Vector3.up * meanHeight;

            float TiltFromOpposingPositions(Vector3 position1, Vector3 position2)
            {
                var h = _plateWidth / 2f;
                var o = (position1.y / 30f - position2.y / 30.0f) / 2f;
                return Mathf.Asin(o / h);
            }

            var gamma = TiltFromOpposingPositions(_motor1Joint2Tip.position, _motor2Joint2Tip.position);
            var beta = TiltFromOpposingPositions(_motor3Joint2Tip.position, _motor4Joint2Tip.position);


            transform.localRotation = Quaternion.Euler(beta * Mathf.Rad2Deg, 0f, gamma * Mathf.Rad2Deg);
        }
    }
}
