using UnityEngine;
using System;
using UniRx;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class TimeLineAnimator : MonoBehaviour
    {
        private static readonly float StartVerticalLineFadeInTime = 1f;
        private static readonly float StartDottedHorizontalBottomLineFadeInTime = 2f;
        
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private ImageProcessingInstructionSender _imageProcessingInstructionSender;
        [SerializeField] private GraphAnimator _graphAnimator;

        private void Start()
        {
            SetupImageSourceSwitchThroughAnimation();

            CompositeDisposable cd = new CompositeDisposable();
            _imageProcessingInstructionSender.ExecutingControlStrategies
                .CombineLatest(_imageProcessingInstructionSender.GetBallBouncingStarted, 
                    (isExecuting, GetBouncingStarted) => (isExecuting, GetBouncingStarted))
                .Subscribe(tuple =>
                {
                    if (tuple.isExecuting && tuple.GetBouncingStarted)
                    {
                        SetupGridLineTimers(cd);
                    }
                    else
                    {
                        cd.Clear();
                        _graphAnimator.Reset();
                    }
                }).AddTo(this);
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

        private void SetupGridLineTimers(CompositeDisposable cd)
        {
            Observable.Timer(TimeSpan.FromSeconds(StartVerticalLineFadeInTime))
                .Subscribe(_ => _graphAnimator.BeginFadeInVerticalLine())
                .AddTo(cd);

            Observable.Timer(TimeSpan.FromSeconds(StartDottedHorizontalBottomLineFadeInTime))
                .Subscribe(_ => _graphAnimator.BeginFadeInDottedHorizontalBottomLine())
                .AddTo(cd);
        }
    }
}
