
namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class Hypothesis
    {
        public Parameters Parameters => _parameters;
        private Parameters _parameters;
        private int _numberOfTrainingSetsUsedForGD;
        private float _alpha;

        public Hypothesis(
            Parameters initialParameters,
            int numberOfTrainingSetsUsedForGD,
            float alpha)
        {
            _parameters = initialParameters;
            _numberOfTrainingSetsUsedForGD = numberOfTrainingSetsUsedForGD;
            _alpha = alpha;
        }

        public float Execute(TrainingSet set)
        {
            return _parameters.Theta_0 * set.t_0 + _parameters.Theta_1 * set.t_1;
        }

        public void SetTheta_0To(float theta_0)
        {
            _parameters.Theta_0 = theta_0;
        }

        public void Update(TrainingSet[] trainingSets)
        {
            var total = 0f;

            for (int i = 0; i < _numberOfTrainingSetsUsedForGD; i++)
            {
                total += (Execute(trainingSets[i]) - trainingSets[i].y) * trainingSets[i].t_0;
            }

            _parameters.Theta_0 -= _alpha * total;

            total = 0f;
            for (int i = 0; i < _numberOfTrainingSetsUsedForGD; i++)
            {
                total += (Execute(trainingSets[i]) - trainingSets[i].y) * trainingSets[i].t_1;
            }

            _parameters.Theta_1 -= _alpha * total;
        }
    }
}
