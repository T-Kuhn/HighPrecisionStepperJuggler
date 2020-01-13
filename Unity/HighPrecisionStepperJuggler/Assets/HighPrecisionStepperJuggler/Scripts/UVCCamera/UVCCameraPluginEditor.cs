using UnityEditor;
using UnityEngine;
using vcp = HighPrecisionStepperJuggler.OpenCVConstants.VideoCaptureProperties;

namespace HighPrecisionStepperJuggler
{
    [CustomEditor(typeof(UVCCameraPlugin))]
    public class UVCCameraPluginEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (UVCCameraPlugin) target;
            if (GUILayout.Button("SetProperties", GUILayout.Width(200)))
            {
                script.SetCameraProperty(vcp.CAP_PROP_EXPOSURE, -7);
                script.SetCameraProperty(vcp.CAP_PROP_GAIN, 10);
            }

            if (GUILayout.Button("GetProperties", GUILayout.Width(200)))
            {
                var width = script.GetCameraProperty(vcp.CAP_PROP_FRAME_WIDTH);
                var height = script.GetCameraProperty(vcp.CAP_PROP_FRAME_HEIGHT);
                var exposure = script.GetCameraProperty(vcp.CAP_PROP_EXPOSURE);
                var gain = script.GetCameraProperty(vcp.CAP_PROP_GAIN);

                Debug.Log("width: " + width);
                Debug.Log("height: " + height);
                Debug.Log("exposure: " + exposure);
                Debug.Log("gain: " + gain);
            }
        }
    }
}
