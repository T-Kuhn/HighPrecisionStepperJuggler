using System.Collections.Generic;
using System.Linq;
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
            SendInstructions(new List<HLInstruction>() {instruction});
        }

        public void SendInstructions(List<HLInstruction> instructions)
        {
            var llInstructions = instructions.Select(instruction => instruction.Translate()).ToList();
            
            switch (_machineEndPoint)
            {
                case MachineEndPoint.Model:
                    _modelMachine.Instruct(llInstructions);
                    break;

                case MachineEndPoint.Real:
                    _realMachine.Instruct(llInstructions);
                    break;

                case MachineEndPoint.ModelAndReal:
                    _modelMachine.Instruct(llInstructions);
                    _realMachine.Instruct(llInstructions);
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
