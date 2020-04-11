using System.Collections.Generic;
using HighPrecisionStepperJuggler.MachineLearning;
using UnityEngine;

public class GradientDescentView : MonoBehaviour
{
    [SerializeField] private GameObject _dataPointPrefab;
    [SerializeField] private GameObject _coordinateSystem;
    [SerializeField] private GameObject _gradientDescentLine;

    public GradientDescent GradientDescent
    {
        set => _gradientDescent = value;
    }
    
    private GradientDescent _gradientDescent;
    
    private List<GameObject> _dataPointList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < _gradientDescent.TrainingSets.Length; i++)
        {
            _dataPointList.Add(Instantiate(_dataPointPrefab, _coordinateSystem.transform));
        }
    }

    private void Update()
    {
        var sets = _gradientDescent.TrainingSets;
        
        for (int i = 0; i < sets.Length; i++)
        {
            _dataPointList[i].transform.localPosition = new Vector3(sets[i].t_1 * 100f, sets[i].y / 2f, 0f);
        }

        _gradientDescentLine.transform.localPosition = Vector3.up * _gradientDescent.Hypothesis.Parameters.Theta_0 / 2f;
        var slope = _gradientDescent.Hypothesis.Parameters.Theta_1 / 2f;
        _gradientDescentLine.transform.localRotation =
            Quaternion.Euler(0f, 0f, Mathf.Atan2(slope, 100f) * Mathf.Rad2Deg);

    }
}
