using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class BallPositionVisualizer : MonoBehaviour
    {
        [SerializeField] private BallVisualizationFader _prefab;
        [SerializeField] private BallVisualizationFader _trailPrefab;
        [SerializeField] private GameObject _cameraHeight;

        public int NumberOfGameObectsInPool;
        public int NumberOfTrailGameObectsInPool;

        private List<BallVisualizationFader> _gameObjectPool;
        private List<BallVisualizationFader> _trailGameObjectPool;
        private bool _visualizeTrail;
        private int _currentIndex;
        private int _currentTrailIndex;

        private void Start()
        {
            _gameObjectPool = new List<BallVisualizationFader>();
            _trailGameObjectPool = new List<BallVisualizationFader>();

            for (int i = 0; i < NumberOfGameObectsInPool; i++)
            {
                _gameObjectPool.Add(Instantiate(_prefab, transform));
            }

            for (int i = 0; i < NumberOfTrailGameObectsInPool; i++)
            {
                _trailGameObjectPool.Add(Instantiate(_trailPrefab, transform));
            }
        }

        public void VisualizePositionPoint(Vector3 position)
        {
            _gameObjectPool[_currentIndex].Reset(); 
            _gameObjectPool[_currentIndex].transform.position = 
                new Vector3(-position.x, position.y + _cameraHeight.transform.position.y + 0.02f, -position.z);

            if (_visualizeTrail)
            {
                _trailGameObjectPool[_currentTrailIndex].Reset();
                _trailGameObjectPool[_currentTrailIndex].transform.position = 
                    new Vector3(-position.x, position.y + _cameraHeight.transform.position.y + 0.02f, -position.z);
            }

            _currentIndex++;
            if (_currentIndex >= NumberOfGameObectsInPool)
            {
                _currentIndex = 0;
            }

            _currentTrailIndex++;
            if (_currentTrailIndex >= NumberOfTrailGameObectsInPool)
            {
                _currentTrailIndex = 0;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _visualizeTrail = !_visualizeTrail;
            }
        }
    }
}
