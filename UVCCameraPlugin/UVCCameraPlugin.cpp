#include "UVCCameraPlugin.h"
#include <opencv2/opencv.hpp>
#include <cstdio>

void* getCamera()
{
    auto cap = new cv::VideoCapture(0);
    //cap->set(cv::CAP_PROP_FRAME_WIDTH, 640);
    //cap->set(cv::CAP_PROP_FRAME_HEIGHT, 480); 
    //cap->set(cv::CAP_PROP_EXPOSURE, -7);
    //cap->set(cv::CAP_PROP_GAIN, 4);

    return static_cast<void*>(cap);
}

double getCameraProperty(void* camera, int propertyID) 
{
    auto cap = static_cast<cv::VideoCapture*>(camera);
    return cap->get(propertyID);
}

double setCameraProperty(void* camera, int propertyID, double value) 
{
    auto cap = static_cast<cv::VideoCapture*>(camera);
    return cap->set(propertyID, value);
}

void releaseCamera(void* camera)
{
    auto cap = static_cast<cv::VideoCapture*>(camera);
    delete cap;
}

void getCameraTexture(void* camera, unsigned char* data)
{
    auto cap = static_cast<cv::VideoCapture*>(camera);

    cv::Mat img;
    *cap >> img;

    // RGB --> ARGB •ÏŠ·
    cv::Mat argb_img;
    cv::cvtColor(img, argb_img, 2);
    std::vector<cv::Mat> bgra;
    cv::split(argb_img, bgra);
    std::swap(bgra[0], bgra[3]);
    std::swap(bgra[1], bgra[2]);
    std::memcpy(data, argb_img.data, argb_img.total() * argb_img.elemSize());
}

