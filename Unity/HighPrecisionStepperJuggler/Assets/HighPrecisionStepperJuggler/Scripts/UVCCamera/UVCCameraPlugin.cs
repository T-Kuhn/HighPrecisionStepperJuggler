using System;
using System.Linq;
using System.Runtime.InteropServices;
using vcp = HighPrecisionStepperJuggler.OpenCVConstants.VideoCaptureProperties;
using c = HighPrecisionStepperJuggler.Constants;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;

namespace HighPrecisionStepperJuggler
{
    public class UVCCameraPlugin : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private ImageProcessingCaptionView _captionView;
        
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
            bool executeMedianBlur,
            int imgMode,
            double dp,
            double minDist,
            double param1,
            double param2,
            int minRadius,
            int maxRadius);

        [DllImport("UVCCameraPlugin")]
        private static extern double getCircleCenter_x();

        [DllImport("UVCCameraPlugin")]
        private static extern double getCircleCenter_y();

        [DllImport("UVCCameraPlugin")]
        private static extern double getCircleRadius();

        private IntPtr _camera;
        private Texture2D _texture;
        private Color32[] _pixels;
        private GCHandle _pixelsHandle;
        private IntPtr _pixelsPtr;
        private CameraProperties _defaultCameraProperties;
        private ImageProcessing _imageProcessing = new ImageProcessing();

        [SerializeField] private Constants.ImgMode _imgMode;
        [SerializeField] private HT21Parameters _ht21Parameters;
        [SerializeField] private CameraProperties _cameraProperties;
        
        private void Awake()
        {
            _imgMode = Constants.ImgMode.Src;
            
            _defaultCameraProperties = new CameraProperties()
            {
                Width = c.CameraResolutionWidth,
                Height = c.CameraResolutionHeight,
                Exposure = -7,
                Gain = 2,
                Saturation = 55,
                Contrast = 15
            };
            
            _ht21Parameters = new HT21Parameters()
            {
                ExecuteHT21 = false,
                ExecuteMedianBlue = false,
                Dp = 1,
                MinDist = 120,
                Param1 = 60,
                Param2 = 30,
                MinRadius = 12,
                MaxRadius = 160
            };
        }

        void Start()
        {
            _camera = getCamera();

            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, _defaultCameraProperties.Width);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_FRAME_WIDTH, _defaultCameraProperties.Height);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_EXPOSURE, _defaultCameraProperties.Exposure);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_GAIN, _defaultCameraProperties.Gain);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_SATURATION, _defaultCameraProperties.Saturation);
            setCameraProperty(_camera, (int) vcp.CAP_PROP_CONTRAST, _defaultCameraProperties.Contrast);

            GetCameraProperties();

            _texture = new Texture2D((int) _defaultCameraProperties.Width, (int) _defaultCameraProperties.Height,
                TextureFormat.ARGB32, false);
            _pixels = _texture.GetPixels32();
            
            _pixelsHandle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
            _pixelsPtr = _pixelsHandle.AddrOfPinnedObject();

            foreach (var c in _volume.profile.components)
            {
                if (c is OverlayComponent oc)
                {
                    oc.overlayParameter.value = _texture;
                }
            }
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
            _cameraProperties.Width = GetCameraProperty(vcp.CAP_PROP_FRAME_WIDTH);
            _cameraProperties.Height = GetCameraProperty(vcp.CAP_PROP_FRAME_HEIGHT);
            _cameraProperties.FPS = GetCameraProperty(vcp.CAP_PROP_FPS);
            _cameraProperties.Exposure = GetCameraProperty(vcp.CAP_PROP_EXPOSURE);
            _cameraProperties.Gain = GetCameraProperty(vcp.CAP_PROP_GAIN);
            _cameraProperties.Contrast = GetCameraProperty(vcp.CAP_PROP_CONTRAST);
            _cameraProperties.ISO = GetCameraProperty(vcp.CAP_PROP_ISO_SPEED);
            _cameraProperties.Saturation = GetCameraProperty(vcp.CAP_PROP_SATURATION);
        }

        public void SetCameraProperties()
        {
            SetCameraProperty(vcp.CAP_PROP_EXPOSURE, _cameraProperties.Exposure);
            SetCameraProperty(vcp.CAP_PROP_GAIN, _cameraProperties.Gain);
            SetCameraProperty(vcp.CAP_PROP_CONTRAST, _cameraProperties.Contrast);
            SetCameraProperty(vcp.CAP_PROP_ISO_SPEED, _cameraProperties.ISO);
            SetCameraProperty(vcp.CAP_PROP_SATURATION, _cameraProperties.Saturation);
        }

        public BallRadiusAndPosition UpdateImageProcessing()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                _imgMode--;
                if ((int) _imgMode < 0)
                {
                    _imgMode = (Constants.ImgMode) Enum.GetNames(typeof(Constants.ImgMode)).Length - 1;
                }

                _captionView.SetText(Constants.Captions[(int) _imgMode]);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                _captionView.SetText(Constants.Captions[(int) _imgMode]);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                _imgMode++;
                if ((int) _imgMode >= Enum.GetNames(typeof(Constants.ImgMode)).Length)
                {
                    _imgMode = 0;
                }

                _captionView.SetText(Constants.Captions[(int) _imgMode]);
            }

            foreach (var c in _volume.profile.components)
            {
                if (c is OverlayComponent oc)
                {
                    if (_imgMode == Constants.ImgMode.Red)
                    {
                        oc.tintColor.value = Color.red;
                    }
                    else if (_imgMode == Constants.ImgMode.Green)
                    {
                        oc.tintColor.value = Color.green;
                    }
                    else if (_imgMode == Constants.ImgMode.Blue)
                    {
                        oc.tintColor.value = Color.blue;
                    }
                    else
                    {
                        oc.tintColor.value = Color.white;
                    }
                }
            }

            _ht21Parameters.ExecuteHT21 = _imgMode == Constants.ImgMode.CustomgrayWithCirclesOverlayed;

            getCameraTexture(
                _camera,
                _pixelsPtr,
                _ht21Parameters.ExecuteHT21,
                _ht21Parameters.ExecuteMedianBlue,
                (int) _imgMode == 7 ? 5 : (int) _imgMode,
                _ht21Parameters.Dp,
                _ht21Parameters.MinDist,
                _ht21Parameters.Param1,
                _ht21Parameters.Param2,
                _ht21Parameters.MinRadius,
                _ht21Parameters.MaxRadius
            );

            if ((int) _imgMode == 7)
            {
                var ballPosAndRadius = _imageProcessing.BallDataFromPixelBoarders(_pixels);

                _texture.SetPixels32(_pixels);
                _texture.Apply();

                // TODO: return both ball positions, not only the first one.
                return ballPosAndRadius.FirstOrDefault();
            }

            _texture.SetPixels32(_pixels);
            _texture.Apply();

            if (_imgMode == Constants.ImgMode.CustomgrayWithCirclesOverlayed)
            {
                return new BallRadiusAndPosition()
                {
                    Radius = (float) getCircleRadius(),
                    PositionX = -(float) getCircleCenter_x() + c.CameraResolutionWidth / 2f,
                    PositionY = -(float) getCircleCenter_y() + c.CameraResolutionHeight / 2f
                };
            }

            return new BallRadiusAndPosition()
            {
                Radius = 0.1f,
                PositionX = 0f + c.CameraResolutionWidth / 2f,
                PositionY = 0f + c.CameraResolutionHeight / 2f
            };

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
        public bool ExecuteMedianBlue;
        public double Dp;
        public double MinDist;
        public double Param1;
        public double Param2;
        public int MinRadius;
        public int MaxRadius;
    }

    public struct BallRadiusAndPosition
    {
        public float Radius;
        public float PositionX;
        public float PositionY;
    }
}
