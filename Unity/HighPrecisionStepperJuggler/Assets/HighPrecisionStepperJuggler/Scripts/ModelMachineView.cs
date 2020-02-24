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

        private enum MachineState
        {
            Idle,
            DoingControlledMoves
        }

        private MachineState _machineState = MachineState.Idle;
        private float _animationTime;
        private LLInstruction _currentInstruction;
        private LLMachineState _lastTargetMachineState;

        public void AddToOriginState(LLMachineState diffState)
        {
            // NOTE: We are using setting the state via diffState because the real machine will only
            //       work with diffStates and the ModelMachine has to behave exactly the same way as the real machine.
            var state = Constants.OriginMachineState + diffState;

            _motorizedArm1.UpdateState(state.Motor1Rotation, InverseKinematics.CalculateJoint2RotationFromJoint1Rotation(state.Motor1Rotation, 0f));
            _motorizedArm2.UpdateState(state.Motor2Rotation, InverseKinematics.CalculateJoint2RotationFromJoint1Rotation(state.Motor2Rotation, 0f));
            _motorizedArm3.UpdateState(state.Motor3Rotation, InverseKinematics.CalculateJoint2RotationFromJoint1Rotation(state.Motor3Rotation, 0f));
            _motorizedArm4.UpdateState(state.Motor4Rotation, InverseKinematics.CalculateJoint2RotationFromJoint1Rotation(state.Motor4Rotation, 0f));
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
                        _lastTargetMachineState = _currentInstruction.TargetMachineState;
                        _currentInstruction = _instructionStack.Pop();
                        _animationTime = 0f;
                        _machineState = MachineState.DoingControlledMoves;
                    }

                    break;

                case MachineState.DoingControlledMoves:
                    _animationTime += Time.deltaTime;

                    var normalizedAnimationValue = _animationTime / _currentInstruction.MoveTime;
                    var animatedMachineState = _lastTargetMachineState.LerpTowards(
                        _currentInstruction.TargetMachineState,
                        (1 + Mathf.Cos((1 - normalizedAnimationValue) * Mathf.PI)) / 2f);

                    AddToOriginState(animatedMachineState);

                    if (_animationTime > _currentInstruction.MoveTime)
                    {
                        _machineState = MachineState.Idle;
                    }

                    break;
            }
        }
    }
}
