using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class ModelMachine : InstructableMachine
    {
        [SerializeField] private ModelMachineView _modelMachineView = null;

        public void Start()
        {
            GoToOrigin();
        }
        
        public override void Instruct(List<LLInstruction> instructions)
        {
            // TODO: do them one at a time.
            var instruction = instructions.First();

            var diffMachineState = instruction.TargetMachineState - Constants.OriginMachineState;
            _modelMachineView.AddToOriginState(diffMachineState);
        }

        public override void GoToOrigin()
        {
            _modelMachineView.GoToOriginState();
        }
    }
}
