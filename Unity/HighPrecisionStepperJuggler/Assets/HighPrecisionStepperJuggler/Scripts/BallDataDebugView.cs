using TMPro;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BallDataDebugView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _upperTmp;
        [SerializeField] private TextMeshProUGUI _lowerTmp;

        public string AirborneTime
        {
            set => _upperTmp.text = "Airborne Time: " + value;
        }

        public string TimeTillNextHit
        {
            set => _lowerTmp.text = "Time Till Next Hit: " + value;
        }
    }
}
