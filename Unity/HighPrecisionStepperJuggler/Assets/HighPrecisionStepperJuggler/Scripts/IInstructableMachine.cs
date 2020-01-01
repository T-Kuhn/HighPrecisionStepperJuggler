using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public abstract class InstructableMachine : MonoBehaviour
    {
        public abstract void Instruct(List<LLInstruction> instructions);
        
        public abstract void GoToOrigin();
    }
}
