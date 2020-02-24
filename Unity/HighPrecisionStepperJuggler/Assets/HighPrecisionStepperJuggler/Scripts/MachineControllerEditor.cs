using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
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

            if (GUILayout.Button("leveling at 10mm: right", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.01f, 0.001f, 0f, 0.2f, true));
            }

            if (GUILayout.Button("leveling at 10mm: left", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.01f, -0.001f, 0f, 0.2f, true));
            }

            if (GUILayout.Button("leveling at 10mm: front", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.01f, 0f, 0.001f, 0.2f, true));
            }

            if (GUILayout.Button("leveling at 10mm: back", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.01f, 0f, -0.001f, 0.2f, true));
            }

            if (GUILayout.Button("Go to height: 10mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.01f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 20mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.02f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 30mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.03f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 40mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.04f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 50mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.05f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 60mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.06f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 70mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.07f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 80mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.08f, 0f, 0f, 0.15f));
            }

            if (GUILayout.Button("Go to height: 90mm", GUILayout.Width(200)))
            {
                script.SendSingleInstruction(new HLInstruction(0.09f, 0f, 0f, 0.15f));
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

            if (GUILayout.Button("demo", GUILayout.Width(200)))
            {
                var moveTime = 0.3f;
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.02f, 0.0f, 0.1f, moveTime),
                    new HLInstruction(0.02f, 0.0f, -0.1f, moveTime),
                    new HLInstruction(0.02f, 0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, -0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, 0.0f, 0.1f, moveTime),
                    new HLInstruction(0.02f, 0.0f, -0.1f, moveTime),
                    new HLInstruction(0.02f, 0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, -0.1f, 0f, moveTime),
                    new HLInstruction(0.03f, 0.0f, 0f, moveTime),
                    new HLInstruction(0.04f, 0.0f, 0f, moveTime),
                });
            }

            if (GUILayout.Button("demo 0.2", GUILayout.Width(200)))
            {
                var moveTime = 0.2f;
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.02f, 0.0f, 0.1f, moveTime),
                    new HLInstruction(0.02f, 0.0f, -0.1f, moveTime),
                    new HLInstruction(0.02f, 0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, -0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, 0.0f, 0.1f, moveTime),
                    new HLInstruction(0.02f, 0.0f, -0.1f, moveTime),
                    new HLInstruction(0.02f, 0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, -0.1f, 0f, moveTime),
                    new HLInstruction(0.03f, 0.0f, 0f, moveTime),
                    new HLInstruction(0.04f, 0.0f, 0f, moveTime),
                });
            }

            if (GUILayout.Button("demo 0.1", GUILayout.Width(200)))
            {
                var moveTime = 0.1f;
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.02f, 0.0f, 0.1f, moveTime),
                    new HLInstruction(0.02f, 0.0f, -0.1f, moveTime),
                    new HLInstruction(0.02f, 0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, -0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, 0.0f, 0.1f, moveTime),
                    new HLInstruction(0.02f, 0.0f, -0.1f, moveTime),
                    new HLInstruction(0.02f, 0.1f, 0f, moveTime),
                    new HLInstruction(0.02f, -0.1f, 0f, moveTime),
                    new HLInstruction(0.03f, 0.0f, 0f, moveTime),
                    new HLInstruction(0.04f, 0.0f, 0f, moveTime),
                });
            }

            if (GUILayout.Button("flex", GUILayout.Width(200)))
            {
                var moveTime = 0.3f;
                var tilt = 0.06694f;
                script.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.09f, 0.0f, 0.0f, 0.5f),
                    new HLInstruction(0.01f, 0.0f, 0.0f, 0.5f),
                    new HLInstruction(0.02f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.03f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.04f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.05f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.06f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.07f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.09f, 0f, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, -tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, -tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, tilt, 0.0f, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, -tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, tilt, 0.0f, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, -tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, -tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, tilt, 0.5f, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, -tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, -tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0f, tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.08f, 0f, tilt, moveTime, isFlexInstruction: true),
                    new HLInstruction(0.01f, 0.0f, 0.0f, 0.5f),
                    new HLInstruction(0.05f, 0.0f, 0.0f, 0.5f),
                });
            }
        }
    }
}
#endif
