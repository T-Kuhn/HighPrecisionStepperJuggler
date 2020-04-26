using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public abstract class InstructableMachine : MonoBehaviour
    {
        public void Instruct(List<LLInstruction> instructions)
        {
            // NOTE: The current Max amount of instructions which can be sent in one go is 100.
            var diffInstructionList = instructions
                .Take(100)
                .Select(instruction =>
                    new LLInstruction(
                        instruction.TargetMachineState - Constants.OriginMachineState,
                        instruction.MoveTime))
                .ToList();
            
            SendInstructions(diffInstructionList);
        }

        protected abstract void SendInstructions(List<LLInstruction> diffInstructions);

        public abstract void GoToOrigin();
    }
}
