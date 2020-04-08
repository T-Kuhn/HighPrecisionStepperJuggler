
using UnityEngine;

namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class Hypothesis
    {
        public Parameters Parameters => _parameters;
        private Parameters _parameters;

        // alpha is what is often referred to as the "learning rate"
        private float _alpha;
        private int _numberOfTrainingSetsUsed;

        public Hypothesis(Parameters initialParameters, float alpha, int numberOfTrainingSetsUsedForGD)
        {
            _parameters = initialParameters;
            _alpha = alpha;
            _numberOfTrainingSetsUsed = numberOfTrainingSetsUsedForGD;
        }

        public float Execute(TrainingSet set)
        {
            return _parameters.Theta_0 * set.t_0 + _parameters.Theta_1 * set.t_1;
        }

        public void Update(TrainingSet[] trainingSets)
        {
            var total = 0f;

            for (int i = 0; i < _numberOfTrainingSetsUsed; i++)
            {
                total += (Execute(trainingSets[i]) - trainingSets[i].y) * trainingSets[i].t_0;
            }

            _parameters.Theta_0 -= _alpha * total;

            total = 0f;
            for (int i = 0; i < _numberOfTrainingSetsUsed; i++)
            {
                total += (Execute(trainingSets[i]) - trainingSets[i].y) * trainingSets[i].t_1;
            }

            _parameters.Theta_1 -= _alpha * total;

            //Debug.Log("updated Theta_0 is: " + _parameters.Theta_0);
            //Debug.Log("updated Theta_1 is: " + _parameters.Theta_1);
        }
    }
}
