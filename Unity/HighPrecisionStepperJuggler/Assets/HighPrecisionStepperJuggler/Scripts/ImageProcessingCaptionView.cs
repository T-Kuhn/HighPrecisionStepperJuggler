using TMPro;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class ImageProcessingCaptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;

        private float _showTextTime;
        private float _fadeOutTime;

        public void SetText(string text)
        {
            _tmp.text = text;
            _tmp.color = new Color(0.9f, 0.9f, 0.9f, 0.5f);
            _showTextTime = 3f;
            _fadeOutTime = 1f;
        }

        void Update()
        {
            _showTextTime -= Time.deltaTime;
            if (_showTextTime < 0f)
            {
                _fadeOutTime -= Time.deltaTime;

                var col = _tmp.color;
                col.a = _fadeOutTime * 0.5f;
                _tmp.color = col;
            }
        }
    }
}
