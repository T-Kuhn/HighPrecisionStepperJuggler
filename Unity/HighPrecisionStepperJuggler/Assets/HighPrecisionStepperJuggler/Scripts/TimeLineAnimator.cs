using UnityEngine;
using System;
using UniRx;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class TimeLineAnimator : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private ImageProcessingInstructionSender _imageProcessingInstructionSender;

        private void Start()
        {
            SetupImageSourceSwitchThroughAnimation();
        }

        private void SetupImageSourceSwitchThroughAnimation()
        {
            int counter = 0;
            Observable.Timer(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
                .WithLatestFrom(_imageProcessingInstructionSender.ExecutingControlStrategies,
                    (timerValue, isExecuting) => (timerValue, isExecuting))
                .Where(tuple => tuple.isExecuting)
                .Subscribe(_ =>
                {
                    counter++;
                    if (counter < Enum.GetNames(typeof(Constants.ImgMode)).Length)
                    {
                        _cameraPlugin.IncrementImgMode();
                    }
                }).AddTo(this);

            _imageProcessingInstructionSender.ExecutingControlStrategies
                .Where(isExecuting => !isExecuting)
                .Subscribe(_ =>
                {
                    counter = 0;
                    _cameraPlugin.ImgMode = Constants.ImgMode.Src;
                }).AddTo(this);
        }
    }
}
