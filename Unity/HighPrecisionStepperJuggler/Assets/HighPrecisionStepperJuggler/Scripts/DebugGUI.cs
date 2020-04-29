using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [SerializeField] private Font _font;
    private GUIStyle _style;
    
    void OnGUI()
    {
        _style = new GUIStyle(GUI.skin.button);
        _style.fontSize = 28;
        _style.hover.textColor = Color.white;
        _style.normal.textColor = Color.white;
        _style.font = _font;
        
        if (GUI.Button(new Rect(10, 70, 100, 40), "Click", _style))
        Debug.Log("Clicked the button");
    } 
}
