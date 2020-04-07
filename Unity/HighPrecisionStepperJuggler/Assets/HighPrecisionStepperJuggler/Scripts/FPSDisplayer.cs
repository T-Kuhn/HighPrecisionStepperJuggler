using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FPSDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fps;
    private Queue<float> _fpsQueue = new Queue<float>();
    private const int MaxValuesInQueue = 40;

    void Update()
    {
        _fpsQueue.Enqueue(1f / Time.deltaTime);
        
        if (_fpsQueue.Count > MaxValuesInQueue)
        {
            _fpsQueue.Dequeue();
        }

        var fps = _fpsQueue.Sum() / _fpsQueue.Count;
        _fps.text = "FPS: " + Mathf.Round(fps).ToString("0.");
    }
}
