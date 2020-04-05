
namespace HighPrecisionStepperJuggler.MachineLearning
{
    // This class implements Gradient Descent as explained by Andrew Ng in this video:
    // https://www.youtube.com/watch?v=5u4G23_OohI
    public class GradientDescent
    {
        public float SlopeOfHypothesis => _hypothesis.SlopeOfHypothesis;

        private TrainingSet[] _trainingSets;
        private Hypothesis _hypothesis;

        public GradientDescent(int maxNumberOfTrainingSets = 5, float alpha = 0.01f)
        {
            // NOTE: Newest training set at index 0
            _trainingSets = new TrainingSet[maxNumberOfTrainingSets];
            _hypothesis = new Hypothesis(new Parameters(0f, 0f), alpha);

            for (int i = 0; i < maxNumberOfTrainingSets; i++)
            {
                _trainingSets[i] = new TrainingSet(0f, 0f);
            }

            // DEBUG START
            AddTrainingSet(new TrainingSet(-10f, 0f));
            AddTrainingSet(new TrainingSet(-10f, 1f));
            AddTrainingSet(new TrainingSet(-10f, 2f));
            AddTrainingSet(new TrainingSet(-10f, 3f));
            AddTrainingSet(new TrainingSet(-10f, 4f));

            for (int i = 0; i < 10; i++)
            {
                UpdateHypothesis();
            }
            // DEBUG END
        }

        public void AddTrainingSet(TrainingSet set)
        {
            for (int i = _trainingSets.Length - 1; i > 0; i--)
            {
                _trainingSets[i - 1].t_1 += set.t_1;
                _trainingSets[i] = _trainingSets[i - 1];
            }

            _trainingSets[0] = set;
        }

        public void UpdateHypothesis()
        {
            _hypothesis.Update(_trainingSets);
        }

        // public GetEstimateForXEquals(float x);
    }
}

