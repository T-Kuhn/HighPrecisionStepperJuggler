using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class ModelMachine : InstructableMachine
    {
        [SerializeField] private ModelMachineView _modelMachineView;

        private LLMachineState _originMachineState;
        
        public override void Instruct(List<LLInstruction> instructions)
        {
            // TODO: do them one at a time.
            var instruction = instructions.First();

            _modelMachineView.SetMachineState(instruction.TargetMachineState);
        }

        public override void GoToOrigin()
        {
            
        }
    }
}
