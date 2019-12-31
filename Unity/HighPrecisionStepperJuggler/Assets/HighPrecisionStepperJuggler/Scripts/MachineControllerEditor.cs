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

            var script = (MachineController) target;
            
            if (GUILayout.Button("Go to height: 70", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.08f, 0f, 0f, 1f));
            }

            if (GUILayout.Button("Go to rest position", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.0575f, 0f, 0f, 1f));
            }
        }
    }
}
