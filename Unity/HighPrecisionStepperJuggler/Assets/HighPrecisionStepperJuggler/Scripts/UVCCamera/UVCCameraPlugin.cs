using System;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Server;
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
        private readonly int Gain = 2;
        private readonly int Saturation = 22;

        private IntPtr _camera;
        private Texture2D _texture;
        private Color32[] _pixels;
        private GCHandle _pixelsHandle;
        private IntPtr _pixelsPtr;

        [SerializeField] private RenderTexture _renderTexture = null;
        [SerializeField] private CameraProperties _properties;

        void Start()
        {
            _camera = getCamera();

            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, Width);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, Height);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_EXPOSURE, Exposure);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_GAIN, Gain);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_SATURATION, Saturation);

            _texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
            _pixels = _texture.GetPixels32();
            _pixelsHandle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
            _pixelsPtr = _pixelsHandle.AddrOfPinnedObject();
        }

        private double GetCameraProperty(vcp property)
        {
            return getCameraProperty(_camera, (int) property);
        }

        private void SetCameraProperty(vcp property, double value)
        {
            setCameraProperty(_camera, (int) property, value);
        }

        public void GetCameraProperties()
        {
            _properties.Width = GetCameraProperty(vcp.CAP_PROP_FRAME_WIDTH);
            _properties.Height = GetCameraProperty(vcp.CAP_PROP_FRAME_HEIGHT);
            _properties.FPS = GetCameraProperty(vcp.CAP_PROP_FPS);
            
            _properties.Exposure = GetCameraProperty(vcp.CAP_PROP_EXPOSURE);
            _properties.Gain = GetCameraProperty(vcp.CAP_PROP_GAIN);
            _properties.Contrast = GetCameraProperty(vcp.CAP_PROP_CONTRAST);
            _properties.ISO = GetCameraProperty(vcp.CAP_PROP_ISO_SPEED);
            _properties.Saturation = GetCameraProperty(vcp.CAP_PROP_SATURATION);
        }

        public void SetCameraProperties()
        {
            SetCameraProperty(vcp.CAP_PROP_EXPOSURE, _properties.Exposure);
            SetCameraProperty(vcp.CAP_PROP_GAIN, _properties.Gain);
            SetCameraProperty(vcp.CAP_PROP_CONTRAST, _properties.Contrast);
            SetCameraProperty(vcp.CAP_PROP_ISO_SPEED, _properties.ISO);
            SetCameraProperty(vcp.CAP_PROP_SATURATION, _properties.Saturation);
        }

        void Update()
        {
            getCameraTexture(_camera, _pixelsPtr);

            _texture.SetPixels32(_pixels);
            _texture.Apply();

            Graphics.Blit(_texture, _renderTexture);
        }

        void OnApplicationQuit()
        {
            _pixelsHandle.Free();
            releaseCamera(_camera);
        }
    }

    [Serializable]
    struct CameraProperties
    {
        public double Width;
        public double Height;
        public double FPS;
        public double Exposure;
        public double Gain;
        public double Contrast;
        public double ISO;
        public double Saturation;
    }
}
