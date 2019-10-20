using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class CameraValidation : MonoBehaviour
    {
        private WebCamTexture _webCamTexture;
        
        [SerializeField]
        private Renderer _planeRenderer;

        void Start()
        {
            _webCamTexture = new WebCamTexture ();
            _webCamTexture.requestedWidth = 256;
            _webCamTexture.requestedHeight = 256;
            _webCamTexture.requestedFPS = 10000;
            
            _planeRenderer.material.SetTexture("_UnlitColorMap", _webCamTexture);
            _webCamTexture.Play ();
        }
    }
}
