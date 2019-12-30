using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class MachineController : MonoBehaviour
    {
        [SerializeField] private IInstructableMachine _realMachine;
        [SerializeField] private IInstructableMachine _modelMachine;
    }
}
