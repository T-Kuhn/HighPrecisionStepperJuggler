using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public abstract class InstructableMachine : MonoBehaviour
    {
        public virtual void Instruct(List<LLInstruction> instructions)
        {
            // TODO: do them one at a time.
            var instruction = instructions.First();
            
            var diffMachineState = instruction.TargetMachineState - Constants.OriginMachineState;
            SendInstruction(new LLInstruction(diffMachineState, instruction.MoveTime));
        }

        protected abstract void SendInstruction(LLInstruction diffInstruction);

        public abstract void GoToOrigin();
    }
}
