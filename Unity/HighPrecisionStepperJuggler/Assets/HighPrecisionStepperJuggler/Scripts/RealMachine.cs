using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class RealMachine : InstructableMachine
    {
        [SerializeField] private SerialInterface _serial = null;
        
        protected override void SendInstruction(LLInstruction diffInstruction)
        {
            _serial.Send("11:" + diffInstruction.Serialize() + ":22:" + diffInstruction.Serialize() + "&");
        }

        public override void GoToOrigin()
        {
            _serial.Send(new LLInstruction(Constants.ZeroMachineState, 1f).Serialize());;
        }
    }
}
