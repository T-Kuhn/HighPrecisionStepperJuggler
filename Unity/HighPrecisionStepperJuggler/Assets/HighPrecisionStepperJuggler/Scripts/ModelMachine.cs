using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class ModelMachine : InstructableMachine
    {
        [SerializeField] private MotorController _motorController;
        
        public override void Instruct(List<IKGInstruction> instructions)
        {
            
        }
    }
}
