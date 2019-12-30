using UnityEditor;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    [CustomEditor(typeof(MachineController))]
    public class MachineControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MachineController script = (MachineController) target;
            if (GUILayout.Button("Send Test Instructions", GUILayout.Width(200)))
            {
                script.SendTestInstructions();
            }
        }
    }
}
