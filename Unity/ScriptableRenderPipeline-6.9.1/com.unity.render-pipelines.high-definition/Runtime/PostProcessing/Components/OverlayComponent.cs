using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    [Serializable, VolumeComponentMenu("Post-processing/Custom/Overlay")]
    public sealed class OverlayComponent : VolumeComponent, IPostProcessComponent
    {
        public BoolParameter isActive = new BoolParameter(false);

        public TextureParameter overlayParameter = new TextureParameter(null);

        //[Tooltip("Specifies color of the outline")]
        //public ColorParameter outlineColor = new ColorParameter(new Color(0,0,0,0), false, true, true);

        public bool IsActive()
        {
            return isActive.value && overlayParameter.value != null;
        }
    }
}
