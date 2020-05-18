using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MachineStateView : MonoBehaviour
    {
        public enum TiltControlType { None, AnalyticalTiltControl, PIDTiltController};
        
        [SerializeField] private TextMeshProUGUI _currentStrategyTmp;
        [SerializeField] private TextMeshProUGUI _pidTiltControlTmp;
        [SerializeField] private TextMeshProUGUI _analyticalTiltControlTmp;

        private void Awake()
        {
            _analyticalTiltControlTmp.alpha = 0f;
            _pidTiltControlTmp.alpha = 0f;

            _currentStrategyTmp.alpha = 0f;
        }

        public void Set(string state = "", TiltControlType tiltControlType = TiltControlType.None)
        {
            DOTween.To(() => _currentStrategyTmp.alpha, 
                x => _currentStrategyTmp.alpha = x, state == "" ? 0f : 0.5f, 1f);

            if (state != "")
            {
                _currentStrategyTmp.text = state;
            }

            switch (tiltControlType)
            {
                case TiltControlType.AnalyticalTiltControl:
                {
                    DOTween.To(() => _analyticalTiltControlTmp.alpha,
                        x => _analyticalTiltControlTmp.alpha = x, 0.5f, 1f);
                    DOTween.To(() => _pidTiltControlTmp.alpha,
                        x => _pidTiltControlTmp.alpha = x, 0f, 1f);
                    break;
                }
                case TiltControlType.PIDTiltController:
                {
                    DOTween.To(() => _analyticalTiltControlTmp.alpha,
                        x => _analyticalTiltControlTmp.alpha = x, 0f, 1f);
                    DOTween.To(() => _pidTiltControlTmp.alpha,
                        x => _pidTiltControlTmp.alpha = x, 0.5f, 1f);
                    break;
                }
                case TiltControlType.None:
                {
                    DOTween.To(() => _analyticalTiltControlTmp.alpha,
                        x => _analyticalTiltControlTmp.alpha = x, 0f, 1f);
                    DOTween.To(() => _pidTiltControlTmp.alpha,
                        x => _pidTiltControlTmp.alpha = x, 0f, 1f);
                    break;
                }
            }
        }
    }
}
