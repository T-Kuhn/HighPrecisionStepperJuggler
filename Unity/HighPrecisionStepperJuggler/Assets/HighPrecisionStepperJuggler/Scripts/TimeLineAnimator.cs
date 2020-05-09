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

        private void Start()
        {
            SetupImageSourceSwitchThroughAnimation();

            CompositeDisposable compositeDisposable = new CompositeDisposable();
            _imageProcessingInstructionSender.ExecutingControlStrategies
                .Subscribe(isExecuting =>
                {
                    if (isExecuting)
                    {
                        Observable.Timer(TimeSpan.FromSeconds(3f))
                            //.DoOnCancel(() => _graphAnimator.Reset())
                            .Subscribe(_ =>
                            {
                                _graphAnimator.BeginFadeInGrid();
                            })
                            .AddTo(compositeDisposable);
                        
                        Observable.Timer(TimeSpan.FromSeconds(5f))
                            .Subscribe(_ =>
                            {
                                // TODO: Do next fade in here
                            })
                            .AddTo(compositeDisposable);
                    }
                    else
                    {
                        compositeDisposable.Clear();
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
    }
}
