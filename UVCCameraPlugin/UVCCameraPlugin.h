#pragma once
extern "C" {
    __declspec(dllexport) void* getCamera();
    __declspec(dllexport) double getCameraProperty(void* camera, int propertyID);
    __declspec(dllexport) double setCameraProperty(void* camera, int propertyID, double value);
    __declspec(dllexport) void releaseCamera(void* camera);
    __declspec(dllexport) void getCameraTexture(
        void* camera,
        unsigned char* data,
        bool executeHT21,
        double dp,
        double minDist,
        double param1,
        double param2,
        int minRadius,
        int maxRadius
    );
}