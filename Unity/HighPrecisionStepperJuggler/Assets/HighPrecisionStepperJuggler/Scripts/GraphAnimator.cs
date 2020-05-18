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

        private List<TweenerCore<float, float, FloatOptions>> _tweeners =
            new List<TweenerCore<float, float, FloatOptions>>();

        [SerializeField] private Material _horizontalLine;
        [SerializeField] private Material _verticalLine;
        [SerializeField] private Material _dottedHorizontalTopLine;
        [SerializeField] private Material _dottedHorizontalBottomLine;
        [SerializeField] private Material _dottedVerticalLine;
        
        [SerializeField] private Material _gradientDescentDataPointX;
        [SerializeField] private Material _gradientDescentDataPointY;
        [SerializeField] private Material _gradientDescentDataPointZ;
        
        [SerializeField] private Material _gradientDescentLineX;
        [SerializeField] private Material _gradientDescentLineY;
        [SerializeField] private Material _gradientDescentLineZ;
        
        [SerializeField] private Material _tiltX;
        [SerializeField] private Material _tiltY;
        
        [SerializeField] private Material _targetCross;
        [SerializeField] private Material _currentPositionCross;
        
        [SerializeField] private Material _predictedHitPositionX;
        [SerializeField] private Material _predictedHitPositionY;

        private Material[] _allMaterials;
        
        private void Awake()
        {
            _allMaterials = new[]
            {
                _horizontalLine,
                _verticalLine,
                _dottedHorizontalTopLine,
                _dottedHorizontalBottomLine,
                _dottedVerticalLine,
                _gradientDescentDataPointX,
                _gradientDescentDataPointY,
                _gradientDescentDataPointZ,
                _gradientDescentLineX,
                _gradientDescentLineY,
                _gradientDescentLineZ,
                _tiltX,
                _tiltY,
                _targetCross,
                _predictedHitPositionX,
                _predictedHitPositionY,
                _currentPositionCross
            };
            
            SetGridToInvisible();
        }

        private void SetAlpha(Material material, float alpha)
        {
            var color = material.GetColor(UnlitColor);
            color.a = alpha;
            material.SetColor(UnlitColor, color);
        }
        
        private void FadeInMaterial(Material material) => 
            _tweeners.Add(DOTween.To(() => 0f, x => SetAlpha(material, x), 1f, 1f));
        
        private void FadeOutMaterial(Material material) => 
            _tweeners.Add(DOTween.To(() => 1f, x => SetAlpha(material, x), 0f, 1f));

        public void FadeInHorizontalLine() => FadeInMaterial(_horizontalLine);
        public void FadeInVerticalLine() => FadeInMaterial(_verticalLine);

        public void FadeInDottedHorizontalTopLine() => FadeInMaterial(_dottedHorizontalTopLine);
        public void FadeInDottedHorizontalBottomLine() => FadeInMaterial(_dottedHorizontalBottomLine);
        public void FadeInDottedVerticalLine() => FadeInMaterial(_dottedVerticalLine);
        public void FadeOutDottedHorizontalTopLine() => FadeOutMaterial(_dottedHorizontalTopLine);
        public void FadeOutDottedHorizontalBottomLine() => FadeOutMaterial(_dottedHorizontalBottomLine);
        public void FadeOutDottedVerticalLine() => FadeOutMaterial(_dottedVerticalLine);

        public void FadeInGradientDescentDataPointX() => FadeInMaterial(_gradientDescentDataPointX);
        public void FadeInGradientDescentDataPointY() => FadeInMaterial(_gradientDescentDataPointY);
        public void FadeInGradientDescentDataPointZ() => FadeInMaterial(_gradientDescentDataPointZ);
        public void FadeOutGradientDescentDataPointX() => FadeOutMaterial(_gradientDescentDataPointX);
        public void FadeOutGradientDescentDataPointY() => FadeOutMaterial(_gradientDescentDataPointY);
        public void FadeOutGradientDescentDataPointZ() => FadeOutMaterial(_gradientDescentDataPointZ);
        
        public void FadeInGradientDescentLineX() => FadeInMaterial(_gradientDescentLineX);
        public void FadeInGradientDescentLineY() => FadeInMaterial(_gradientDescentLineY);
        public void FadeInGradientDescentLineZ() => FadeInMaterial(_gradientDescentLineZ);
        public void FadeOutGradientDescentLineX() => FadeOutMaterial(_gradientDescentLineX);
        public void FadeOutGradientDescentLineY() => FadeOutMaterial(_gradientDescentLineY);
        public void FadeOutGradientDescentLineZ() => FadeOutMaterial(_gradientDescentLineZ);

        public void FadeInTiltX() => FadeInMaterial(_tiltX);
        public void FadeInTiltY() => FadeInMaterial(_tiltY);
        public void FadeOutTiltX() => FadeOutMaterial(_tiltX);
        public void FadeOutTiltY() => FadeOutMaterial(_tiltY);
        
        public void FadeInTargetCross() => FadeInMaterial(_targetCross);
        public void FadeOutTargetCross() => FadeOutMaterial(_targetCross);
        
        public void FadeInCurrentPositionCross() => FadeInMaterial(_currentPositionCross);
        public void FadeOutCurrentPositionCross() => FadeOutMaterial(_currentPositionCross);
        
        public void FadeInPredictedHitPositionX() => FadeInMaterial(_predictedHitPositionX);
        public void FadeInPredictedHitPositionY() => FadeInMaterial(_predictedHitPositionY);
        public void FadeOutPredictedHitPositionX() => FadeOutMaterial(_predictedHitPositionX);
        public void FadeOutPredictedHitPositionY() => FadeOutMaterial(_predictedHitPositionY);
        
        private void SetGridToInvisible()
        {
            foreach (var material in _allMaterials)
            {
                SetAlpha(material, 0f);
            }
        }

        public void Reset()
        {
            foreach (var tweener in _tweeners)
            {
                tweener?.Kill();
            }

            _tweeners.Clear();

            SetGridToInvisible();
        }
    }
}
