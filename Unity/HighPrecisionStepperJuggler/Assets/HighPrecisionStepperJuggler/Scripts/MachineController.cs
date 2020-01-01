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
            Real, 
            ModelAndReal
        }

        [SerializeField] private MachineEndPoint _machineEndPoint = MachineEndPoint.Model;

        public void SendSingleInstruction(HLInstruction instruction)
        {
            var instructions = new List<LLInstruction>() {instruction.Translate()};

            switch (_machineEndPoint)
            {
                case MachineEndPoint.Model:
                    _modelMachine.Instruct(instructions);
                    break;
                
                case MachineEndPoint.Real:
                    _realMachine.Instruct(instructions);
                    break;
                
                case MachineEndPoint.ModelAndReal:
                    _modelMachine.Instruct(instructions);
                    _realMachine.Instruct(instructions);
                    break;
            }
        }

        public void GoToOrigin()
        {
            switch (_machineEndPoint)
            {
                case MachineEndPoint.Model:
                    _modelMachine.GoToOrigin();
                    break;

                case MachineEndPoint.Real:
                    _realMachine.GoToOrigin();
                    break;

                case MachineEndPoint.ModelAndReal:
                    _modelMachine.GoToOrigin();
                    _realMachine.GoToOrigin();
                    break;
            }

        }
    }
}
