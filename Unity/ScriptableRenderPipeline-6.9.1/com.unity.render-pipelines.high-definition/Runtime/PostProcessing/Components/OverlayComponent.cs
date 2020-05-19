using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    [Serializable, VolumeComponentMenu("Post-processing/Custom/Overlay")]
    public sealed class OverlayComponent : VolumeComponent, IPostProcessComponent
    {
        public BoolParameter isActive = new BoolParameter(false);

        public TextureParameter overlayParameter = new TextureParameter(null);
        public TextureParameter secondOverlayParameter = new TextureParameter(null);
        public TextureParameter fullScreenOverlay = new TextureParameter(null);

        public ColorParameter tintColor = new ColorParameter(new Color(1,1,1,1), false, true, true);

        public bool IsActive()
        {
            return isActive.value && overlayParameter.value != null;
        }
    }
}
