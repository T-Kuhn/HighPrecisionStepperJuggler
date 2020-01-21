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
        bool executeMedianBlur,
        int imgMode,            // 0: src, 1: red, 2: green, 3: blue, 4: normalgray, 5: customgray 
        double dp,
        double minDist,
        double param1,
        double param2,
        int minRadius,
        int maxRadius
    );
    __declspec(dllexport) double getCircleCenter_x();
    __declspec(dllexport) double getCircleCenter_y();
    __declspec(dllexport) double getCircleRadius();
}