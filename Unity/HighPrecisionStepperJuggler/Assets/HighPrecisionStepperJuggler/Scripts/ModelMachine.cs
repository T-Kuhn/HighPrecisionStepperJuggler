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
        
        protected override void SendInstruction(LLMachineState diffMachineState)
        {
            _modelMachineView.AddToOriginState(diffMachineState);
        }

        public override void GoToOrigin()
        {
            _modelMachineView.AddToOriginState(Constants.ZeroMachineState);
        }
    }
}
