using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class CrossVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject originLineX;
        [SerializeField] private GameObject middleLineX;
        [SerializeField] private GameObject originLineY;
        [SerializeField] private GameObject middleLineY;

        public void UpdateCrossPosition(Vector2 target)
        {
            var x = target.x / 2f;
            var y = target.y / 2f;
            originLineX.transform.localPosition = new Vector3(0f, -x, 0f);
            middleLineX.transform.localPosition = new Vector3(y, -x, 0f);

            originLineY.transform.localPosition = new Vector3(0f, y, 0f);
            middleLineY.transform.localPosition = new Vector3(x, y, 0f);
        }
    }
}
