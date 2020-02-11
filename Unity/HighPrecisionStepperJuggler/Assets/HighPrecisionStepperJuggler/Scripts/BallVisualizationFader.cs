using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BallVisualizationFader : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private float _countDown;

        private void Start()
        {
            _countDown = Constants.BallVisualizationFadeOutTime;
        }

        private void Update()
        {
            _countDown -= Time.deltaTime;
            if (_countDown <= 0.0f)
            {
                Destroy(gameObject);
                return;
            }
            
            var alpha = _countDown / Constants.BallVisualizationFadeOutTime;
            var color = _meshRenderer.material.GetColor("_UnlitColor");
            color.a = alpha;
            _meshRenderer.material.SetColor("_UnlitColor", color);
        }
    }
}
