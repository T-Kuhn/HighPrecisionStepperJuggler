using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class RealMachine : InstructableMachine
    {
        [SerializeField] private SerialInterface _serial = null;
        
        protected override void SendInstruction(LLInstruction diffInstruction)
        {
            _serial.Send(diffInstruction.Serialize());
        }

        public override void GoToOrigin()
        {
            //_serial.Send(Constants.ZeroState);
        }
    }
}
