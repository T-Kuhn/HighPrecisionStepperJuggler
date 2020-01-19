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
        private static extern void getCameraTexture(
            IntPtr camera,
            IntPtr data,
            bool executeHT21,
            double dp,
            double minDist,
            double param1,
            double param2,
            int minRadius,
            int maxRadius);

        private IntPtr _camera;
        private Texture2D _texture;
        private Color32[] _pixels;
        private GCHandle _pixelsHandle;
        private IntPtr _pixelsPtr;

        private readonly CameraProperties _defaultCameraProperties =
            new CameraProperties()
            {
                Width = 640,
                Height = 480,
                Exposure = -7,
                Gain = 2,
                Saturation = 33
            };

        [SerializeField] private HT21Parameters _ht21Parameters =
            new HT21Parameters()
            {
                ExecuteHT21 = true,
                Dp = 1,
                MinDist = 120,
                Param1 = 100,
                Param2 = 30,
                MinRadius = 20,
                MaxRadius = 110
            };

        [SerializeField] private CameraProperties _properties;
        [SerializeField] private RenderTexture _renderTexture = null;

        void Start()
        {
            _camera = getCamera();

            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, _defaultCameraProperties.Width);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, _defaultCameraProperties.Height);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_EXPOSURE, _defaultCameraProperties.Exposure);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_GAIN, _defaultCameraProperties.Gain);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_SATURATION, _defaultCameraProperties.Saturation);

            GetCameraProperties();

            _texture = new Texture2D((int) _defaultCameraProperties.Width, (int) _defaultCameraProperties.Height,
                TextureFormat.ARGB32, false);
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
            getCameraTexture(
                _camera,
                _pixelsPtr,
                _ht21Parameters.ExecuteHT21,
                _ht21Parameters.Dp,
                _ht21Parameters.MinDist,
                _ht21Parameters.Param1,
                _ht21Parameters.Param2,
                _ht21Parameters.MinRadius,
                _ht21Parameters.MaxRadius
            );

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

    [Serializable]
    struct HT21Parameters
    {
        public bool ExecuteHT21;
        public double Dp;
        public double MinDist;
        public double Param1;
        public double Param2;
        public int MinRadius;
        public int MaxRadius;
    }
}
