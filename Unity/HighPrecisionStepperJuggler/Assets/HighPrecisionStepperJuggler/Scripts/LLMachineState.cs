namespace HighPrecisionStepperJuggler
{
    // Low Level Machine State
    public readonly struct LLMachineState
    {
        public float Motor1Rotation { get; }
        public float Motor2Rotation { get; }
        public float Motor3Rotation { get; }
        public float Motor4Rotation { get; }
        public float Arm1Joint2Rotation { get; }
        public float Arm2Joint2Rotation { get; }
        public float Arm3Joint2Rotation { get; }
        public float Arm4Joint2Rotation { get; }

        public LLMachineState(float m1Rot, float m2Rot, float m3Rot, float m4Rot,
            float arm1J2Rot, float arm2J2Rot, float arm3J2Rot, float arm4J2Rot)
        {
            Motor1Rotation = m1Rot;
            Motor2Rotation = m2Rot;
            Motor3Rotation = m3Rot;
            Motor4Rotation = m4Rot;
            Arm1Joint2Rotation = arm1J2Rot;
            Arm2Joint2Rotation = arm2J2Rot;
            Arm3Joint2Rotation = arm3J2Rot;
            Arm4Joint2Rotation = arm4J2Rot;
        }

        public static LLMachineState operator +(LLMachineState a, LLMachineState b)
        {
            return new LLMachineState(
                a.Motor1Rotation + b.Motor1Rotation,
                a.Motor2Rotation + b.Motor2Rotation,
                a.Motor3Rotation + b.Motor3Rotation,
                a.Motor4Rotation + b.Motor4Rotation,
                a.Arm1Joint2Rotation + b.Arm1Joint2Rotation,
                a.Arm2Joint2Rotation + b.Arm2Joint2Rotation,
                a.Arm3Joint2Rotation + b.Arm3Joint2Rotation,
                a.Arm4Joint2Rotation + b.Arm4Joint2Rotation);
        }

        public static LLMachineState operator -(LLMachineState a, LLMachineState b)
        {
            return new LLMachineState(
                a.Motor1Rotation - b.Motor1Rotation,
                a.Motor2Rotation - b.Motor2Rotation,
                a.Motor3Rotation - b.Motor3Rotation,
                a.Motor4Rotation - b.Motor4Rotation,
                a.Arm1Joint2Rotation - b.Arm1Joint2Rotation,
                a.Arm2Joint2Rotation - b.Arm2Joint2Rotation,
                a.Arm3Joint2Rotation - b.Arm3Joint2Rotation,
                a.Arm4Joint2Rotation - b.Arm4Joint2Rotation);
        }
    }
}
