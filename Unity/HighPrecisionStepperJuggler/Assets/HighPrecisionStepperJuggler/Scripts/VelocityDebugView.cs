using TMPro;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class VelocityDebugView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _vx;
        [SerializeField] private TextMeshProUGUI _vy;

        public string Vx
        {
            set => _vx.text = "velocity x: " + value;
        }

        public string Vy
        {
            set => _vy.text = "velocity y: " + value;
        }
    }
}
