using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public sealed class PredictedPositionVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _predictedPositionX;
        [SerializeField] private GameObject _predictedPositionY;
        
        public void Visualize(Vector2 predictedBallPositionOnHit)
        {
            _predictedPositionX.transform.localPosition = Vector3.right * (predictedBallPositionOnHit.x / 2f);
            _predictedPositionY.transform.localPosition = Vector3.up * (predictedBallPositionOnHit.y / 2f);
        }

    }
}
