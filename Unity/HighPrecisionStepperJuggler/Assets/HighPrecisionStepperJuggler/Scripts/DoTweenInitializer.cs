using UnityEngine;
using DG.Tweening;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class DoTweenInitializer : MonoBehaviour
    {
        private void Awake()
        {
            DOTween.Init();
        }
    }
}
