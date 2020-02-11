using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BallPositionVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _cameraHeight;

        public void SpawnPositionPoint(Vector3 position)
        {
            var go = Instantiate(_prefab);
            go.transform.position = new Vector3(-position.x, position.y + _cameraHeight.transform.position.y + 0.02f,
                -position.z);
        }
    }
}
