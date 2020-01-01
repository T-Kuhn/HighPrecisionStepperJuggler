using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class RealMachine : InstructableMachine
    {
        [SerializeField] private SerialInterface _serial = null;
        
        protected override void SendInstruction(LLMachineState diffMachineState)
        {
            var a = diffMachineState.ToString();
            Debug.Log("a " + a);
            
            _serial.Send("12:23&");
        }

        public override void GoToOrigin()
        {
            //_serial.Send(Constants.ZeroState);
        }
    }
}
