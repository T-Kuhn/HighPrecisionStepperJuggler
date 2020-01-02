using System;
using System.Collections.Generic;
using UnityEngine;
using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    public class ModelMachineView : MonoBehaviour
    {
        [SerializeField] private MotorizedArm _motorizedArm1 = null;
        [SerializeField] private MotorizedArm _motorizedArm2 = null;
        [SerializeField] private MotorizedArm _motorizedArm3 = null;
        [SerializeField] private MotorizedArm _motorizedArm4 = null;

        private Stack<LLInstruction> _instructionStack = new Stack<LLInstruction>();
        
        private enum MachineState {Idle, DoingControlledMoves}
        private MachineState _machineState = MachineState.Idle;
        private float _animationTime;
        private LLInstruction _currentInstruction;

        public void AddToOriginState(LLMachineState diffState)
        {
            // NOTE: We are using setting the state via diffState because the real machine will only
            //       work with diffStates and the ModelMachine has to behave exactly the same way as the real machine.
            var state = Constants.OriginMachineState + diffState;
            
            _motorizedArm1.UpdateState(state.Motor1Rotation, state.Arm1Joint2Rotation);
            _motorizedArm2.UpdateState(state.Motor2Rotation, state.Arm2Joint2Rotation);
            _motorizedArm3.UpdateState(state.Motor3Rotation, state.Arm3Joint2Rotation);
            _motorizedArm4.UpdateState(state.Motor4Rotation, state.Arm4Joint2Rotation);
        }

        public void AddToOriginStateAnimated(List<LLInstruction> diffInstructions)
        {
            _instructionStack.Clear();
            
            diffInstructions.Reverse();
            foreach (var diffInstruction in diffInstructions)
            {
                _instructionStack.Push(diffInstruction);
            }
        }

        private void Update()
        {
            switch (_machineState)
            {
                case MachineState.Idle:
                    if (_instructionStack.Count > 0)
                    {
                        _currentInstruction = _instructionStack.Pop();
                        _animationTime = 0f;
                        _machineState = MachineState.DoingControlledMoves;
                    }
                    break;
                
                case MachineState.DoingControlledMoves:
                    _animationTime += Time.deltaTime;
                    AddToOriginState(_currentInstruction.TargetMachineState);
                    
                    if (_animationTime > _currentInstruction.MoveTime)
                    {
                        _machineState = MachineState.Idle;
                    }
                    break;
            }
        }
    }
}
