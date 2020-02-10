using System;

namespace HighPrecisionStepperJuggler
{
    public sealed class BallControlStrategy : IBallControlStrategy
    {
        private readonly Func<BallData, MachineController, int, bool> _executeFunc;
        private readonly int _duration;
        private int _instructionsSentCount;
        
        public BallControlStrategy(Func<BallData, MachineController,int, bool> executeFunc, int duration)
        {
            _duration = duration;
            _executeFunc = executeFunc;
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
        void Reset();
    }
}
