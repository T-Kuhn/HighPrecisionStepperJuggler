using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class RealMachine : InstructableMachine
    {
        [SerializeField] private SerialInterface _serial = null;
        
        protected override void SendInstructions(List<LLInstruction> diffInstructions)
        {
            var builder = new StringBuilder();
            int i = 1;

            foreach (var diffInstruction in diffInstructions)
            {
                if (i >= 2) builder.Append(":");

                builder.Append((11f * i++).ToString("0.00000"));
                builder.Append(":");
                builder.Append(diffInstruction.Serialize());
            }

            builder.Append('\n');

            _serial.Send(builder.ToString());
        }

        public override void GoToOrigin()
        {
            SendInstructions(new List<LLInstruction>() {new LLInstruction(Constants.ZeroMachineState, 1f)});
        }
    }
}
