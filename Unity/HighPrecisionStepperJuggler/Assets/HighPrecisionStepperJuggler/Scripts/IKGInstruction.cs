namespace HighPrecisionStepperJuggler
{
    // IK Generated Instruction
    public readonly struct IKGInstruction
    {
        public float Motor1Rotation { get; }
        public float Motor2Rotation { get; }
        public float Motor3Rotation { get; }
        public float Motor4Rotation { get; }
        public float MoveTime { get; }
        public float Arm1Joint2Rotation { get; }
        public float Arm2Joint2Rotation { get; }
        public float Arm3Joint2Rotation { get; }
        public float Arm4Joint2Rotation { get; }

        public IKGInstruction(float m1Rot, float m2Rot, float m3Rot, float m4Rot, float moveTime,
            float arm1J2Rot, float arm2J2Rot, float arm3J2Rot, float arm4J2Rot)
        {
            Motor1Rotation = m1Rot;
            Motor2Rotation = m2Rot;
            Motor3Rotation = m3Rot;
            Motor4Rotation = m4Rot;
            MoveTime = moveTime;
            Arm1Joint2Rotation = arm1J2Rot;
            Arm2Joint2Rotation = arm2J2Rot;
            Arm3Joint2Rotation = arm3J2Rot;
            Arm4Joint2Rotation = arm4J2Rot;
        }
    }
}
