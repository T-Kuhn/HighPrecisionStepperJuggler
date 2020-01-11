using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class PluginTestScript : MonoBehaviour 
{
    [DllImport ("UVCCameraPlugin")]
    private static extern void createCheckTexture(IntPtr data, int w, int h, int ch); 

    private Texture2D texture_;
    private Color32[] pixels_;
    private GCHandle pixels_handle_;
    private IntPtr pixels_ptr_ = IntPtr.Zero;

    [SerializeField] private Renderer _renderer = null;

    void Start() 
    {
        // テクスチャを生成
        texture_ = new Texture2D(10, 10, TextureFormat.RGB24, false);
        // テクスチャの拡大方法をニアレストネイバーに変更
        texture_.filterMode = FilterMode.Point;
        // Color32 型の配列としてテクスチャの参照をもらう
        pixels_ = texture_.GetPixels32();
        // GC されないようにする
        pixels_handle_ = GCHandle.Alloc(pixels_, GCHandleType.Pinned);
        // そのテクスチャのアドレスをもらう
        pixels_ptr_ = pixels_handle_.AddrOfPinnedObject();
        // スクリプトがアタッチされたオブジェクトのテクスチャをコレにする
        _renderer.material.SetTexture("_UnlitColorMap", texture_);
    }

    void Update() 
    {
        // ネイティブ側でテクスチャを生成
        createCheckTexture(pixels_ptr_, texture_.width, texture_.height, 4);
        // セットして反映させる
        texture_.SetPixels32(pixels_);
        texture_.Apply();
    }

    void OnApplicationQuit()
    {
        // GC 対象にする
        pixels_handle_.Free();
    }
}
