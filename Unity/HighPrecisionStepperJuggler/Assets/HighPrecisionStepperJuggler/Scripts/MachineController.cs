using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MachineController : MonoBehaviour
    {
        [SerializeField] private InstructableMachine _realMachine = null;
        [SerializeField] private InstructableMachine _modelMachine = null;

        private float _elapsedTime;
        private float _totalMoveTime;

        public bool IsReadyForNextInstruction => _elapsedTime > _totalMoveTime;

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

        private void Awake()
        {
            _elapsedTime = 0f;
            _totalMoveTime = 0f;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, 0f, 0.001f, 0.2f, true));
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, 0f, -0.001f, 0.2f, true));
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, 0.001f, 0f, 0.2f, true));
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, -0.001f, 0f, 0.2f, true));
            }

            
        }

        public void SendInstructions(List<HLInstruction> instructions)
        {
            if (_elapsedTime < _totalMoveTime)
            {
                return;
            }
                
            var llInstructions = instructions.Select(instruction => instruction.Translate()).ToList();

            _totalMoveTime = 0f;
            _elapsedTime = 0f;
            foreach (var instruction in instructions)
            {
                _totalMoveTime += instruction.MoveTime;
            }
            _totalMoveTime *= 1.1f;
            
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
