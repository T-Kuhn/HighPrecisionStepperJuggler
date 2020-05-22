using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class OverlayActivator : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private Canvas _imageProcessingCaptionCanvas;
        [SerializeField] private Canvas _airBorneTimeCanvas;
        [SerializeField] private Canvas _fpsCanvas;
        [SerializeField] private Canvas _machineStateAndControlTypeCanvas;

        public void SetVideoOverlayWithTextsTo(bool flag)
        {
            foreach (var c in _volume.profile.components)
            {
                if (c is OverlayComponent oc)
                {
                    oc.overlayParameter.overrideState = flag;
                }
            }

            _imageProcessingCaptionCanvas.enabled = flag;
            _fpsCanvas.enabled = flag;
            _airBorneTimeCanvas.enabled = flag;
        }

        public void SetGraphOverlayWithTextsTo(bool flag)
        {
            foreach (var c in _volume.profile.components)
            {
                if (c is OverlayComponent oc)
                {
                    oc.secondOverlayParameter.overrideState = flag;
                }
            }
            _machineStateAndControlTypeCanvas.enabled = flag;
        }
    }
}
