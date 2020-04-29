using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [SerializeField] private Font _font;
    [SerializeField] private GradientDescentView _gradientDescentViewX;
    [SerializeField] private GradientDescentView _gradientDescentViewY;
    [SerializeField] private GradientDescentView _gradientDescentViewZ;

    private bool _showDebugGUI = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _showDebugGUI = !_showDebugGUI;
        }
    }

    private void OnGUI()
    {
        if (!_showDebugGUI) { return; }
        
        var labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.font = _font;
        labelStyle.alignment = TextAnchor.UpperCenter;
        labelStyle.fontSize = 28;
        labelStyle.normal.textColor = Color.white;

        var buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.font = _font;
        buttonStyle.fontSize = 28;
        buttonStyle.hover.textColor = Color.white;
        buttonStyle.normal.textColor = Color.white;

        GUI.BeginGroup(new Rect(1280, 500, 500, 100));
        GUI.Box(new Rect(0, 0, 500, 100), "");
        GUI.Label(new Rect(65, 5, 100, 40), "XY", labelStyle);
        // xy axis data x100
        if (GUI.Button(new Rect(10, 50, 100, 40), "×10", buttonStyle))
        {
            _gradientDescentViewX.TimeScaler = 10f;
            _gradientDescentViewY.TimeScaler = 10f;
        }

        // xy axis data x10
        if (GUI.Button(new Rect(120, 50, 100, 40), "×100", buttonStyle))
        {
            _gradientDescentViewX.TimeScaler = 100f;
            _gradientDescentViewY.TimeScaler = 100f;
        }

        GUI.Label(new Rect(335, 5, 100, 40), "Z", labelStyle);
        // height data x100
        if (GUI.Button(new Rect(280, 50, 100, 40), "×10", buttonStyle))
        {
            _gradientDescentViewZ.TimeScaler = 10f;
        }

        // height data x10
        if (GUI.Button(new Rect(390, 50, 100, 40), "×100", buttonStyle))
        {
            _gradientDescentViewZ.TimeScaler = 100f;
        }

        GUI.EndGroup();
    }
}
