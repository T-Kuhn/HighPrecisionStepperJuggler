
namespace HighPrecisionStepperJuggler.MachineLearning
{
    public class Hypothesis
    {
        public Parameters Parameters => _parameters;
        private Parameters _parameters;

        public Hypothesis(Parameters initialParameters)
        {
            _parameters = initialParameters;
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

            for (int i = 0; i < Constants.NumberOfTrainingSetsUsedForGD; i++)
            {
                total += (Execute(trainingSets[i]) - trainingSets[i].y) * trainingSets[i].t_0;
            }

            _parameters.Theta_0 -= Constants.Alpha * total;

            total = 0f;
            for (int i = 0; i < Constants.NumberOfTrainingSetsUsedForGD; i++)
            {
                total += (Execute(trainingSets[i]) - trainingSets[i].y) * trainingSets[i].t_1;
            }

            _parameters.Theta_1 -= Constants.Alpha * total;

            //Debug.Log("updated Theta_0 is: " + _parameters.Theta_0);
            //Debug.Log("updated Theta_1 is: " + _parameters.Theta_1);
        }
    }
}
