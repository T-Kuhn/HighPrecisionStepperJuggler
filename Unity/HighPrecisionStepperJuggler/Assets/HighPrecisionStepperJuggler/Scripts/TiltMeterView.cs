using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class TiltMeterView : MonoBehaviour
    {
        public float LineLength
        {
            set => transform.localScale = new Vector3(value, 1f, 1f);
        }
    }
}
