using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class RealMachine : InstructableMachine
    {
        [SerializeField] private SerialInterface _serial = null;
        
        public override void Instruct(List<LLInstruction> instructions)
        {
            Debug.Log("serial.");
            _serial.Send("12:34&");
        }
        
        public override void GoToOrigin()
        {
        }
    }
}
