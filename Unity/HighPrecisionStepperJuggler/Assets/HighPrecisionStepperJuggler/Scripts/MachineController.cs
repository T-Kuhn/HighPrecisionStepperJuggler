using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MachineController : MonoBehaviour
    {
        [SerializeField] private InstructableMachine _realMachine = null;
        [SerializeField] private InstructableMachine _modelMachine = null;

        private enum MachineEndPoint
        {
            Model,
            Real
        }

        [SerializeField] private MachineEndPoint _machineEndPoint;

        public void SendSingleInstruction(HLInstruction instruction)
        {
            var instructions = new List<IKGInstruction>() {instruction.Translate()};

            if (_machineEndPoint == MachineEndPoint.Model)
            {
                _modelMachine.Instruct(instructions);
            }
            else
            {
                _realMachine.Instruct(instructions);
            }
        }
    }
}
