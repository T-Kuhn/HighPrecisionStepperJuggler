using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public abstract class InstructableMachine : MonoBehaviour
    {
        private LLMachineState _levelingOffset;

        public void Instruct(List<LLInstruction> instructions)
        {
            var levelingInstructions = instructions.Where(instruction => instruction.IsLevelingInstruction);

            // NOTE: The current Max amount of instructions which can be sent in one go is 10.
            var diffInstructionList = instructions
                .Take(10)
                .Select(instruction =>
                    new LLInstruction(
                        instruction.TargetMachineState - Constants.OriginMachineState + _levelingOffset,
                        instruction.MoveTime))
                .ToList();

            foreach (var instruction in levelingInstructions)
            {
                var levelingOnlyState = instruction.TargetMachineState -
                                        new HLInstruction(0.01f, 0f, 0f, 0.2f).Translate().TargetMachineState;
                _levelingOffset += levelingOnlyState;
            }

            SendInstructions(diffInstructionList);
        }

        protected abstract void SendInstructions(List<LLInstruction> diffInstructions);

        public abstract void GoToOrigin();
    }
}
