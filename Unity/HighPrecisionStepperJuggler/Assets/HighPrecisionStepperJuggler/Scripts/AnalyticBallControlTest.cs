using HighPrecisionStepperJuggler.MachineLearning;
using UnityEngine;

public class AnalyticBallControlTest : MonoBehaviour
{
    void Start()
    {
        var tilt = AnalyticalTiltController.Instance.CalculateTilt(
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(0f, 0f),
            1f,
            1f);

        Debug.Log("tiltX: " + tilt.xTilt);
        Debug.Log("tiltY: " + tilt.yTilt);
    }
}
