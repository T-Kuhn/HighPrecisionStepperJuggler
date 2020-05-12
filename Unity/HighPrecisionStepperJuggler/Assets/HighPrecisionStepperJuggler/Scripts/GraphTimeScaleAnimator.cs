using DG.Tweening;
using UnityEngine;

public class GraphTimeScaleAnimator : MonoBehaviour
{
    [SerializeField] private GradientDescentView _gradientDescentViewX;
    [SerializeField] private GradientDescentView _gradientDescentViewY;
    [SerializeField] private GradientDescentView _gradientDescentViewZ;

    public void SetXYTimeScale(float from, float to)
    {
        DOTween.To(() => from, x => _gradientDescentViewX.TimeScaler = x, to, 1f);
        DOTween.To(() => from, x => _gradientDescentViewY.TimeScaler = x, to, 1f);
    }

    public void SetZTimeScale(float from, float to)
    {
        DOTween.To(() => from, x => _gradientDescentViewZ.TimeScaler = x, to, 1f);
    }
}
