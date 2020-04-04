
namespace HighPrecisionStepperJuggler.MachineLearning
{
    public struct TrainingSet
    {
        public readonly float t_0;
        public float t_1;
        public readonly float y;

        public TrainingSet(float t1, float y)
        {
            // NOTE: t_0 is always 1 per definition
            t_0 = 1f;
            t_1 = t1;
            this.y = y;
        }
    }
}
