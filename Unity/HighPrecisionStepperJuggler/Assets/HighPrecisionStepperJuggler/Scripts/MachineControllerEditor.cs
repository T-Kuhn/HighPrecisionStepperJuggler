using System.Collections.Generic;
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

            if (GUILayout.Button("Go to origin", GUILayout.Width(200)))
            {
                script.GoToOrigin();
            }

            if (GUILayout.Button("Go to height: 10mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.01f, 0f, 0f, 0.3f));
            }

            if (GUILayout.Button("Go to height: 20mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.02f, 0f, 0f, 0.3f));
            }

            if (GUILayout.Button("Go to height: 30mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.03f, 0f, 0f, 0.3f));
            }

            if (GUILayout.Button("Go to height: 40mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.04f, 0f, 0f, 0.3f));
            }

            if (GUILayout.Button("Go to 10mm then 20mm", GUILayout.Width(200)))
            {
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.01f, 0f, 0f, 0.3f),
                    new HLInstruction(0.02f, 0f, 0f, 0.3f)
                });
            }

            if (GUILayout.Button("20mm tilt right left", GUILayout.Width(200)))
            {
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.02f, 0.1f, 0f, 0.3f),
                    new HLInstruction(0.02f, -0.1f, 0f, 0.3f)
                });
            }

            if (GUILayout.Button("20mm tilt front back", GUILayout.Width(200)))
            {
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.02f, 0.0f, 0.1f, 0.3f),
                    new HLInstruction(0.02f, 0.0f, -0.1f, 0.3f)
                });
            }
        }
    }
}
