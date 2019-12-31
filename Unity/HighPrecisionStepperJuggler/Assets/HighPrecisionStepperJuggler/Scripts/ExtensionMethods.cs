using ik = HighPrecisionStepperJuggler.InverseKinematics;

namespace HighPrecisionStepperJuggler
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Translates a high level instruction into a low level ik generated instruction
        /// </summary>
        /// <param name="hlInstruction"></param>
        public static IKGInstruction Translate(this HLInstruction hlInstruction)
        {
            var hlState = hlInstruction.TargetHLMachineState;

            var xHeightOffset = MiscMath.HeightDifferenceFromTilt(hlState.XTilt);
            var yHeightOffset = MiscMath.HeightDifferenceFromTilt(hlState.YTilt);
            
            var xWidthOffset = MiscMath.WidthDifferenceFromTilt(hlState.XTilt);
            var yWidthOffset = MiscMath.WidthDifferenceFromTilt(hlState.YTilt);
            
            var m1Rot = ik.CalculateJoint1RotationFromTargetY(hlState.Height + xHeightOffset, xWidthOffset);
            var m2Rot = ik.CalculateJoint1RotationFromTargetY(hlState.Height - xHeightOffset, xWidthOffset);
            var m3Rot = ik.CalculateJoint1RotationFromTargetY(hlState.Height + yHeightOffset, yWidthOffset);
            var m4Rot = ik.CalculateJoint1RotationFromTargetY(hlState.Height - yHeightOffset, yWidthOffset);

            var m1J2Rot = ik.CalculateJoint2RotationFromJoint1Rotation(m1Rot, xWidthOffset);
            var m2J2Rot = ik.CalculateJoint2RotationFromJoint1Rotation(m2Rot, xWidthOffset);
            var m3J2Rot = ik.CalculateJoint2RotationFromJoint1Rotation(m3Rot, yWidthOffset);
            var m4J2Rot = ik.CalculateJoint2RotationFromJoint1Rotation(m4Rot, yWidthOffset);

            var ikGeneratedState = new IKGMachineState(m1Rot, m2Rot, m3Rot, m4Rot, m1J2Rot, m2J2Rot, m3J2Rot, m4J2Rot);
            
            return new IKGInstruction(ikGeneratedState, hlInstruction.MoveTime);
        }
    }
}
