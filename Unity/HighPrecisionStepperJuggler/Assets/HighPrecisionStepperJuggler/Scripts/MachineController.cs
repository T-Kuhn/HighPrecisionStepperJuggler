using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MachineController : MonoBehaviour
    {
        [SerializeField] private InstructableMachine _realMachine = null;
        [SerializeField] private InstructableMachine _modelMachine;
        
        private enum MachineEndPoint {Model, Real}

        [SerializeField] private MachineEndPoint _machineEndPoint;

        public void SendTestInstructions()
        {
            if (_machineEndPoint == MachineEndPoint.Model)
            {
                _modelMachine.Instruct(new List<IKGInstruction>() {new IKGInstruction()});
            }
            else
            {
                _realMachine.Instruct(new List<IKGInstruction>(){new IKGInstruction()});
            }
        }
    }
}
