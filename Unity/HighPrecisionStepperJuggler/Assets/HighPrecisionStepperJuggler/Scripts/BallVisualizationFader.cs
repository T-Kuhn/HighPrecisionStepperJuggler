using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BallVisualizationFader : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private bool _isSmallBall;

        private float _countDown;
        private float _totalFadeTime;

        private void Start()
        {
            if (_isSmallBall)
            {
                _countDown = Constants.SmallBallVisualizationFadeOutTime;
            }
            else
            {
                _countDown = Constants.BallVisualizationFadeOutTime;
            }
            
            _totalFadeTime = _countDown;
        }

        private void Update()
        {
            _countDown -= Time.deltaTime;
            if (_countDown <= 0.0f)
            {
                Destroy(gameObject);
                return;
            }
            
            var alpha = _countDown / _totalFadeTime;
            var color = _meshRenderer.material.GetColor("_UnlitColor");
            color.a = alpha;
            _meshRenderer.material.SetColor("_UnlitColor", color);
        }
    }
}
