using System;
using System.Runtime.InteropServices;
using vcp = HighPrecisionStepperJuggler.OpenCVConstants.VideoCaptureProperties;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class UVCCameraPlugin : MonoBehaviour
    {
        [DllImport("UVCCameraPlugin")]
        private static extern IntPtr getCamera();

        [DllImport("UVCCameraPlugin")]
        private static extern double getCameraProperty(IntPtr camera, int propertyId);
        
        [DllImport("UVCCameraPlugin")]
        private static extern double setCameraProperty(IntPtr camera, int propertyId, double value);

        [DllImport("UVCCameraPlugin")]
        private static extern void releaseCamera(IntPtr camera);

        [DllImport("UVCCameraPlugin")]
        private static extern void getCameraTexture(IntPtr camera, IntPtr data);

        private readonly int Width = 640;
        private readonly int Height = 480;
        private readonly int Exposure = -7;
        private readonly int Gain = 15;

        private IntPtr _camera;
        private Texture2D _texture;
        private Color32[] _pixels;
        private GCHandle _pixelsHandle;
        private IntPtr _pixelsPtr;

        [SerializeField] private Renderer _renderer = null;

        void Start()
        {
            _camera = getCamera();

            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, Width);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, Height);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_EXPOSURE, Exposure);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_GAIN, Gain);
            
            _texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
            _pixels = _texture.GetPixels32();
            _pixelsHandle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
            _pixelsPtr = _pixelsHandle.AddrOfPinnedObject();
            _renderer.material.SetTexture("_UnlitColorMap", _texture);
        }

        public double GetCameraProperty(vcp property)
        {
            return getCameraProperty(_camera, (int)property);
        }

        public void SetCameraProperty(vcp property, double value)
        {
            setCameraProperty(_camera, (int) property, value);
        }

        void Update()
        {
            getCameraTexture(_camera, _pixelsPtr);
            _texture.SetPixels32(_pixels);
            _texture.Apply();
        }

        void OnApplicationQuit()
        {
            _pixelsHandle.Free();
            releaseCamera(_camera);
        }
    }
}
