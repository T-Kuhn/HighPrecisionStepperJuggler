using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class Plate : MonoBehaviour
    {
        private static readonly float _plateWidth = 0.08f;
        
        [SerializeField] private Transform _motor1Joint2Tip = null;
        [SerializeField] private Transform _motor2Joint2Tip = null;

        void LateUpdate()
        {
            // set height
            var meanHeight = (_motor1Joint2Tip.position.y + _motor2Joint2Tip.position.y) / 2f;
            transform.position = Vector3.up * meanHeight;

            // set x-axis tilt
            var h = _plateWidth / 2f;
            var o = (_motor1Joint2Tip.position.y / 30f - _motor2Joint2Tip.position.y / 30.0f)/2f;
            var gamma = Mathf.Asin(o / h);
            transform.localRotation = Quaternion.Euler(0f, 0f, gamma * Mathf.Rad2Deg);
        }
    }
}
