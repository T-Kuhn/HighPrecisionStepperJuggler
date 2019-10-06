using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    [ExecuteInEditMode]
    public class Plate : MonoBehaviour
    {
        [SerializeField] private Transform _motor1Joint2Tip;
        [SerializeField] private Transform _motor2Joint2Tip;

        void Update()
        {
            // set height
            var meanHeight = (_motor1Joint2Tip.position.y + _motor2Joint2Tip.position.y) / 2f;
            transform.position = Vector3.up * meanHeight;

            // set tilt

        }
    }
}
