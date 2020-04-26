using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class TiltMeterPresenter : MonoBehaviour
    {
        [SerializeField] private TiltingPlate _tiltingPlate;

        [SerializeField] private TiltMeterView _xTiltMeter;
        [SerializeField] private TiltMeterView _yTiltMeter;

        private float _xLineLength;
        private float _yLineLength;

        private Vector2 _offset = Vector2.zero;

        private void Start()
        {
            _tiltingPlate.OnTiltUpdate += (xTilt, yTilt) =>
            {
                _xLineLength = xTilt * 20f;
                _xTiltMeter.LineLength = _xLineLength - _offset.x;

                _yLineLength = yTilt * 20f;
                _yTiltMeter.LineLength = _yLineLength - _offset.y;
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _offset.x = _xLineLength;
                _offset.y = _yLineLength;
            }
        }
    }
}
