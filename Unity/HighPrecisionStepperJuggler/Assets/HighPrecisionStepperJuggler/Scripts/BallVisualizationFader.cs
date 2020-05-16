using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BallVisualizationFader : MonoBehaviour
    {
        private static readonly int UnlitColor = Shader.PropertyToID("_UnlitColor");

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private bool _isSmallBall;

        private float _countDown;
        private float _totalFadeTime;

        public void Reset()
        {
            _countDown = _isSmallBall
                ? Constants.SmallBallVisualizationFadeOutTime
                : Constants.BallVisualizationFadeOutTime;

            _totalFadeTime = _countDown;
        }

        private void Update()
        {
            _countDown -= Time.deltaTime;
            var alpha = _countDown <= 0.0f ? 0f : _countDown / _totalFadeTime;
            if (alpha <= 0f)
            {
                transform.position = Vector3.up * 1000f;
                return;
            }

            var color = _meshRenderer.material.GetColor(UnlitColor);
            color.a = alpha;
            _meshRenderer.material.SetColor(UnlitColor, color);
        }
    }
}
