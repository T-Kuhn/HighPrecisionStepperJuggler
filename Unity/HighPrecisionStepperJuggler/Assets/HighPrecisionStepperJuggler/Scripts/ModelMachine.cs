using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class ModelMachine : InstructableMachine
    {
        [SerializeField] private MotorController _motorController;
        
        public override void Instruct(List<IKGInstruction> instructions)
        {
            // TODO: do them one at a time.
            var instruction = instructions.First();

            _motorController.SetMachineState(instruction.TargetMachineState);
        }
    }
}
