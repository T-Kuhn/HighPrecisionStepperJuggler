using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class UVCCameraPlugin : MonoBehaviour 
{
    [DllImport ("UVCCameraPlugin")]
    private static extern IntPtr getCamera(); 
    [DllImport ("UVCCameraPlugin")]
    private static extern void releaseCamera(IntPtr camera); 
    [DllImport ("UVCCameraPlugin")]
    private static extern void getCameraTexture(IntPtr camera, IntPtr data, int width, int height); 

    private IntPtr camera_;
    private Texture2D texture_;
    private Color32[] pixels_;
    private GCHandle pixels_handle_;
    private IntPtr pixels_ptr_;
    
    [SerializeField] private Renderer _renderer = null;

    void Start() 
    {
        camera_ = getCamera();
        texture_ = new Texture2D(640, 480, TextureFormat.ARGB32, false);
        pixels_ = texture_.GetPixels32();
        pixels_handle_ = GCHandle.Alloc(pixels_, GCHandleType.Pinned);
        pixels_ptr_ = pixels_handle_.AddrOfPinnedObject();
        _renderer.material.SetTexture("_UnlitColorMap", texture_);
    }

    void Update() 
    {
        getCameraTexture(camera_, pixels_ptr_, texture_.width, texture_.height);
        texture_.SetPixels32(pixels_);
        texture_.Apply();
    }

    void OnApplicationQuit()
    {
        pixels_handle_.Free();
        releaseCamera(camera_);
    }
}
