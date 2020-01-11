#pragma once
extern "C" {
    __declspec(dllexport) void* getCamera();
    __declspec(dllexport) void releaseCamera(void* camera);
    __declspec(dllexport) void getCameraTexture(void* camera, unsigned char* data, int width, int height);
}