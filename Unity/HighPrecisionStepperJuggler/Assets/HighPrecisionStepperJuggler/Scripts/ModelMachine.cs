using System.Collections.Generic;
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

        protected override void SendInstructions(List<LLInstruction> diffInstructions)
        {
            _modelMachineView.AddToOriginStateAnimated(diffInstructions);
        }

        public override void GoToOrigin()
        {
            _modelMachineView.AddToOriginStateAnimated(
                new List<LLInstruction>() {new LLInstruction(Constants.ZeroMachineState, 1f)});
        }
    }
}
