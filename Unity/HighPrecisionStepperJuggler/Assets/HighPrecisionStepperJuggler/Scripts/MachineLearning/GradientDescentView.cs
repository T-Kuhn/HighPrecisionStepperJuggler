using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;

public class GradientDescentView : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private RenderTexture _renderTexture;

    void Start()
    {
        foreach (var c in _volume.profile.components)
        {
            if (c is OverlayComponent oc)
            {
                oc.secondOverlayParameter.value = _renderTexture;
            }
        }
    }
}
