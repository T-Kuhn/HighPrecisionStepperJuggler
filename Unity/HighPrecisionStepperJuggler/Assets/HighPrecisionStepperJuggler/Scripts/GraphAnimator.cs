using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class GraphAnimator : MonoBehaviour
    {
        private static readonly int UnlitColor = Shader.PropertyToID("_UnlitColor");

        private List<TweenerCore<float, float, FloatOptions>> _tweenerList =
            new List<TweenerCore<float, float, FloatOptions>>();

        [SerializeField] private Material _horizontalLine;
        [SerializeField] private Material _verticalLine;
        [SerializeField] private Material _dottedHorizontalTopLine;
        [SerializeField] private Material _dottedHorizontalBottomLine;
        [SerializeField] private Material _dottedVerticalLine;

        private void Awake()
        {
            SetGridToInvisible();
        }

        private void SetAlphaOnMaterial(Material material, float alpha)
        {
            var color = material.GetColor(UnlitColor);
            color.a = alpha;
            material.SetColor(UnlitColor, color);
        }

        public void BeginFadeInVerticalLine() =>
            _tweenerList.Add(DOTween.To(() => 0f, x => SetAlphaOnMaterial(_verticalLine, x), 1f, 1f));

        public void BeginFadeInDottedHorizontalBottomLine() =>
            _tweenerList.Add(DOTween.To(() => 0f, x => SetAlphaOnMaterial(_dottedHorizontalBottomLine, x), 1f, 1f));

        public void BeginFadeInHorizontalLine() =>
            _tweenerList.Add(DOTween.To(() => 0f, x => SetAlphaOnMaterial(_horizontalLine, x), 1f, 1f));

        private void SetGridToInvisible()
        {
            SetAlphaOnMaterial(_horizontalLine, 0f);
            SetAlphaOnMaterial(_verticalLine, 0f);
            SetAlphaOnMaterial(_dottedVerticalLine, 0f);
            SetAlphaOnMaterial(_dottedHorizontalTopLine, 0f);
            SetAlphaOnMaterial(_dottedHorizontalBottomLine, 0f);
        }

        public void Reset()
        {
            foreach (var tweener in _tweenerList)
            {
                tweener?.Kill();
            }

            _tweenerList.Clear();

            SetGridToInvisible();
        }
    }
}
