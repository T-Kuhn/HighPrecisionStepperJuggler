using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class PositionLogger : MonoBehaviour
    {
        private void Update()
        {
            Debug.Log("time: " + Time.time + " + position: " + transform.position.ToString("0.000"));
        }
    }
}
