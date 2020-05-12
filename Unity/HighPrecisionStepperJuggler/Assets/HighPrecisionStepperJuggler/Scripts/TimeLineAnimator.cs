using UnityEngine;
using System;
using UniRx;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class TimeLineAnimator : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private ImageProcessingInstructionSender _imageProcessingInstructionSender;
        [SerializeField] private GraphAnimator _graphAnimator;
        [SerializeField] private GraphTimeScaleAnimator _graphTimeScaleAnimator;

        private void Start()
        {
            SetupImageSourceSwitchThroughAnimation();

            CompositeDisposable cd = new CompositeDisposable();
            
            _imageProcessingInstructionSender.GetBallBouncingStarted
                .Subscribe(_ => ShowAndAnimateHeightData(cd));
            
            _imageProcessingInstructionSender.OnExecutingControlStrategies
                .Where(isExecuting => !isExecuting)
                .Subscribe(_ =>
                {
                    cd.Clear();
                    _graphAnimator.Reset();
                }).AddTo(this);
        }

        private void SetupImageSourceSwitchThroughAnimation()
        {
            int counter = 0;
            Observable.Timer(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
                .WithLatestFrom(_imageProcessingInstructionSender.OnExecutingControlStrategies,
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

            _imageProcessingInstructionSender.OnExecutingControlStrategies
                .Where(isExecuting => !isExecuting)
                .Subscribe(_ =>
                {
                    counter = 0;
                    _cameraPlugin.ImgMode = Constants.ImgMode.Src;
                }).AddTo(this);
        }

        private void InvokeActionAtTime(Action action, float time, CompositeDisposable cd)
        {
            Observable.Timer(TimeSpan.FromSeconds(time))
                .Subscribe(_ => action())
                .AddTo(cd);
        }

        private void ShowAndAnimateHeightData(CompositeDisposable cd)
        {
            InvokeActionAtTime(() => _graphAnimator.FadeInVerticalLine(), 1f, cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInDottedHorizontalBottomLine(), 1.5f, cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentDataPointZ(), 2f, cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentLineZ(), 2.5f, cd);
            
            InvokeActionAtTime(() => _graphTimeScaleAnimator.SetZTimeScale(100f, 50f), 5.5f, cd);
            InvokeActionAtTime(() => _graphTimeScaleAnimator.SetZTimeScale(50f, 100f), 15.5f, cd);
        }
    }
}
