using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public struct TrainingSet
    {
        public readonly float T_0;
        public readonly float T_1;
        public readonly float Y;

        public TrainingSet(float t1, float y)
        {
            // NOTE: x_0 is always 1
            T_0 = 1f;
            T_1 = t1;
            Y = y;
        }
    }

    public struct Parameters
    {
        public float Theta_0;
        public float Theta_1;

        public Parameters(float theta0, float theta1)
        {
            Theta_0 = theta0;
            Theta_1 = theta1;
        }
    }

    public class Hypothesis
    {
        public float SlopeOfHypothesis => _parameters.Theta_1;

        private Parameters _parameters;
        private float _alpha;

        public Hypothesis(Parameters initialParameters, float alpha)
        {
            _parameters = initialParameters;
            _alpha = alpha;
        }

        public float ExecuteHypothesis(TrainingSet set)
        {
            return _parameters.Theta_0 * set.T_0 + _parameters.Theta_1 * set.T_1;
        }

        public void Update(TrainingSet[] trainingSets)
        {
            var total = 0f;
            foreach (var set in trainingSets)
            {
                total += (ExecuteHypothesis(set) - set.Y) * set.T_0;
            }

            _parameters.Theta_0 -= _alpha * total;
            
            total = 0f;
            foreach (var set in trainingSets)
            {
                total += (ExecuteHypothesis(set) - set.Y) * set.T_1;
            }

            _parameters.Theta_1 -= _alpha * total;
            
            Debug.Log("updated Theta_0 is: " + _parameters.Theta_0);
            Debug.Log("updated Theta_1 is: " + _parameters.Theta_1);
        }
    }

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

            // DEBUG:
            AddTrainingSet(new TrainingSet(-4f, 0f));
            AddTrainingSet(new TrainingSet(-3f, 1f));
            AddTrainingSet(new TrainingSet(-2f, 2f));
            AddTrainingSet(new TrainingSet(-1f, 3f));
            AddTrainingSet(new TrainingSet(0f, 4f));

            for (int i = 0; i < 100; i++)
            {
                UpdateHypothesis();
            }
        }

        public void AddTrainingSet(TrainingSet set)
        {
            for (int i = _trainingSets.Length - 1; i > 0; i--)
            {
                _trainingSets[i] = _trainingSets[i - 1];
            }

            _trainingSets[0] = set;
        }

        public void UpdateHypothesis()
        {
            _hypothesis.Update(_trainingSets);
        }
            
        // public AddTrainingSet
        // public GetEstimateForXEquals(float x);
    }
}

