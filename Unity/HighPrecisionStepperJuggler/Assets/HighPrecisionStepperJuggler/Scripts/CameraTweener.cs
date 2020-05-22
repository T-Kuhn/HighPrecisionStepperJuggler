using DG.Tweening;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class CameraTweener : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform1;
        [SerializeField] private Transform _targetTransform2;
        [SerializeField] private Transform _targetTransform3;

        public void GoToPosition1() => GoToPosition(_targetTransform1);
        public void GoToPosition2() => GoToPosition(_targetTransform2);
        public void GoToPosition3() => GoToPosition(_targetTransform3);
        
        private void GoToPosition(Transform t)
        {
            DOTween.To(() => transform.position, position => transform.position = position, t.position , 3f);
            DOTween.To(() => transform.rotation, rotation => transform.rotation = rotation, t.rotation.eulerAngles, 3f);
        }
    }
}
