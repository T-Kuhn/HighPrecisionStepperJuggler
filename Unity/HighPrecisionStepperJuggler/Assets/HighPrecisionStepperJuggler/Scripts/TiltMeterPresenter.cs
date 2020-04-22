using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class TiltMeterPresenter : MonoBehaviour
    {
        [SerializeField] private TiltingPlate _tiltingPlate;

        [SerializeField] private TiltMeterView _xTiltMeter;
        [SerializeField] private TiltMeterView _yTiltMeter;

        private void Start()
        {
            _tiltingPlate.OnTiltUpdate += (xTilt, yTilt) =>
            {
                _xTiltMeter.LineLength = xTilt * 20f;
                _yTiltMeter.LineLength = yTilt * 20f;
            };
        }
    }
}
