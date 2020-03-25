using TMPro;
using UnityEngine;

public class FPSDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fps;

    private int _frameCounter;
    private float _deltaTimeCounter;
    
    void Update()
    {
        _frameCounter++;
        _deltaTimeCounter += Time.deltaTime;

        if (_frameCounter % 5 == 0 && _frameCounter >= 5)
        {
            // we're in here when the _frameCounter is at 5, 10, 15, 20, 25, ...

            var fps = _frameCounter / _deltaTimeCounter;
            _fps.text = "FPS: " + fps.ToString("0.");
        }

        if (_frameCounter >= 60)
        {
            _frameCounter = 0;
            _deltaTimeCounter = 0f;
        }
    }
}
