using UnityEngine;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class TimeLineAnimator : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;

        public bool ExecuteTimeLineAnimation { set; private get; }

        private void Update()
        {
            if (ExecuteTimeLineAnimation)
            {
                _cameraPlugin.IncrementImgMode();
            }
        }
    }
}
