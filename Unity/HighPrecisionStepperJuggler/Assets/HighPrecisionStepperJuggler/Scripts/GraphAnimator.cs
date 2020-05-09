using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class GraphAnimator : MonoBehaviour
    {
        private static readonly int UnlitColor = Shader.PropertyToID("_UnlitColor");
        
        private TweenerCore<float, float, FloatOptions> _gridTweener;
        [SerializeField] private Material _gridSharedMaterial;

        private void Awake()
        {
            SetGridToInvisible();
        }

        public void BeginFadeInGrid()
        {
            var color = _gridSharedMaterial.GetColor(UnlitColor);
            color.a = 0f;
            _gridTweener = DOTween.To(() => color.a, x =>
            {
                color.a = x;
                _gridSharedMaterial.SetColor(UnlitColor, color);
                
            }, 1f, 1f);
        }

        private void SetGridToInvisible()
        {
            var color= _gridSharedMaterial.GetColor(UnlitColor);
            color.a = 0f;
            _gridSharedMaterial.SetColor(UnlitColor, color);
        }

        public void Reset()
        {
            _gridTweener?.Kill();
            SetGridToInvisible();
        }
    }
}
