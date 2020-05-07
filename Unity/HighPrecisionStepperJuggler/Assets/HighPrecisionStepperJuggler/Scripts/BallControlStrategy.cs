using System;

namespace HighPrecisionStepperJuggler
{
    public sealed class BallControlStrategy : IBallControlStrategy
    {
        private readonly Func<BallData, MachineController, int, bool> _executeFunc;
        private readonly int _duration;
        private int _instructionsSentCount;
        private bool _usesBallPositionPrediction;

        public bool UsesBallPositionPrediction => _usesBallPositionPrediction;
        
        public BallControlStrategy(Func<BallData, MachineController, int, bool> executeFunc,
            int duration, bool usesBallPositionPrediction = false)
        {
            _duration = duration;
            _executeFunc = executeFunc;
            _usesBallPositionPrediction = usesBallPositionPrediction;
        }

        public bool Execute(BallData ballData, MachineController machineController)
        {
            var instructionsSent = _executeFunc(ballData, machineController, _instructionsSentCount);
            if (instructionsSent)
            {
                _instructionsSentCount++;
            }

            return _instructionsSentCount >= _duration;
        }

        public void Reset()
        {
            _instructionsSentCount = 0;
        }
    }

    public interface IBallControlStrategy
    {
        bool Execute(BallData ballData, MachineController machineController);
        bool UsesBallPositionPrediction { get; }
        void Reset();
    }
}
