using UnityEngine;
using DG.Tweening;

public class DebugGUI : MonoBehaviour
{
    [SerializeField] private Font _font;
    [SerializeField] private GradientDescentView _gradientDescentViewX;
    [SerializeField] private GradientDescentView _gradientDescentViewY;
    [SerializeField] private GradientDescentView _gradientDescentViewZ;

    private bool _showDebugGUI = false;

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

        GUI.BeginGroup(new Rect(1020, 600, 250, 800));
        
        GUI.Box(new Rect(0, 0, 250, 210), "");
        
        GUI.Label(new Rect(75, 5, 100, 40), "XY", labelStyle);
        if (GUI.Button(new Rect(20, 50, 100, 40), "×10", buttonStyle))
        {
            var currentTimeScaler = 100f;
            DOTween.To(() => currentTimeScaler, x =>
            {
                currentTimeScaler = x;
                _gradientDescentViewX.TimeScaler = currentTimeScaler;
                _gradientDescentViewY.TimeScaler = currentTimeScaler;
            }, 10f, 1f);
        }

        if (GUI.Button(new Rect(130, 50, 100, 40), "×100", buttonStyle))
        {
            var currentTimeScaler = 10f;
            DOTween.To(() => currentTimeScaler, x =>
            {
                currentTimeScaler = x;
                _gradientDescentViewX.TimeScaler = currentTimeScaler;
                _gradientDescentViewY.TimeScaler = currentTimeScaler;
            }, 100f, 1f);
        }

        GUI.Label(new Rect(75, 105, 100, 40), "Z", labelStyle);
        if (GUI.Button(new Rect(20, 150, 100, 40), "×10", buttonStyle))
        {
            var currentTimeScaler = 100f;
            DOTween.To(() => currentTimeScaler, x =>
            {
                currentTimeScaler = x;
                _gradientDescentViewZ.TimeScaler = currentTimeScaler;
            }, 10f, 1f);
        }

        if (GUI.Button(new Rect(130, 150, 100, 40), "×100", buttonStyle))
        {
            var currentTimeScaler = 10f;
            DOTween.To(() => currentTimeScaler, x =>
            {
                currentTimeScaler = x;
                _gradientDescentViewZ.TimeScaler = currentTimeScaler;
            }, 100f, 1f);
        }

        GUI.EndGroup();
    }
}
