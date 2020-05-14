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

            _imageProcessingInstructionSender.OnCheckPointPassed
                .Where(checkPoint => checkPoint == 1)
                .Subscribe(_ => ShowHeightData(cd))
                .AddTo(this);

            _imageProcessingInstructionSender.OnCheckPointPassed
                .Where(checkPoint => checkPoint == 2)
                .Subscribe(_ => ShowXYData(cd))
                .AddTo(this);

            _imageProcessingInstructionSender.OnCheckPointPassed
                .Where(checkPoint => checkPoint == 3)
                .Subscribe(_ => ShowTarget(cd))
                .AddTo(this);

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

        private void ShowHeightData(CompositeDisposable cd)
        {
            InvokeActionAtTime(() => _graphAnimator.FadeInVerticalLine(), 1f, cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInDottedHorizontalBottomLine(), 1.5f, cd);

            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentDataPointZ(), 2f, cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentLineZ(), 2.0f, cd);
        }

        private void ShowXYData(CompositeDisposable cd)
        {
            var time = 0.5f;
            float GetTime(bool increment = true) => time += increment ? 0.5f : 0f;

            InvokeActionAtTime(() => _graphAnimator.FadeOutGradientDescentDataPointZ(), GetTime(), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeOutGradientDescentLineZ(), GetTime(false), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeOutDottedHorizontalBottomLine(), GetTime(), cd);

            InvokeActionAtTime(() => _graphAnimator.FadeInHorizontalLine(), GetTime(), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInDottedHorizontalTopLine(), GetTime(), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInDottedVerticalLine(), GetTime(), cd);

            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentDataPointX(), GetTime(), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentLineX(), GetTime(false), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInPredictedHitPositionX(), GetTime(false), cd);

            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentDataPointY(), GetTime(), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInGradientDescentLineY(), GetTime(false), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInPredictedHitPositionY(), GetTime(false), cd);
            
            InvokeActionAtTime(() => _graphAnimator.FadeInTiltX(), GetTime(), cd);
            InvokeActionAtTime(() => _graphAnimator.FadeInTiltY(), GetTime(false), cd);
        }

        private void ShowTarget(CompositeDisposable cd)
        {
            InvokeActionAtTime(() => _graphAnimator.FadeInTarget(), 1f, cd);
        }
    }
}
