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

        private HLMachineState _levelingOffset = new HLMachineState(0f, 0f, 0f);

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
                SendSingleInstruction(new HLInstruction(0.01f, 0f, 0.05f, 0.2f, true));
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, 0f, -0.05f, 0.2f, true));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, 0.05f, 0f, 0.2f, true));
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SendSingleInstruction(new HLInstruction(0.01f, -0.05f, 0f, 0.2f, true));
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                GoToOrigin();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SendSingleInstruction(new HLInstruction(0.01f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SendSingleInstruction(new HLInstruction(0.02f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SendSingleInstruction(new HLInstruction(0.03f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SendSingleInstruction(new HLInstruction(0.04f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SendSingleInstruction(new HLInstruction(0.05f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SendSingleInstruction(new HLInstruction(0.06f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SendSingleInstruction(new HLInstruction(0.07f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SendSingleInstruction(new HLInstruction(0.08f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SendSingleInstruction(new HLInstruction(0.09f, 0f, 0f, 0.25f));
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SendSingleInstruction(new HLInstruction(0.002f, 0f, 0f, 0.25f));
                
                /*
                var moveTime = 0.3f;
                var tilt = 0.06694f;
                SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.09f, 0.0f, 0.0f, 0.5f),
                    new HLInstruction(0.01f, 0.0f, 0.0f, 0.5f),
                    new HLInstruction(0.02f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.03f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.04f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.05f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.06f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.07f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.09f, 0f, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, -tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, -tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, -tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, -tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, -tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, -tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0.0f, 0.0f, 0.5f),
                    new HLInstruction(0.05f, 0.0f, 0.0f, 0.5f),
                });
                */
            }
        }

        public void SendInstructions(List<HLInstruction> instructions)
        {
            if (_elapsedTime < _totalMoveTime)
            {
                return;
            }
            
            // add tilt as HighLevelInstruction in here
            var levelingInstructions = instructions.Where(instruction => instruction.IsLevelingInstruction);

            foreach (var instruction in levelingInstructions)
            {
                var levelingOnlyState = instruction.TargetHLMachineState -
                                        new HLInstruction(0.01f, 0f, 0f, 0.2f).TargetHLMachineState;
                
                _levelingOffset += levelingOnlyState;
            }

            var llInstructions = instructions.Select(instruction =>
            {
                var i = instruction + new HLInstruction(_levelingOffset, 0f, instruction.IsLevelingInstruction);
                return i.Translate();
            }).ToList();

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
