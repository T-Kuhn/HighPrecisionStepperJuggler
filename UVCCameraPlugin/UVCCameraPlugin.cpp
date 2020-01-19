
#include "UVCCameraPlugin.h"
#include <opencv2/opencv.hpp>
#include <cstdio>
#include "opencv2/imgcodecs.hpp"
#include "opencv2/highgui.hpp"
#include "opencv2/imgproc.hpp"

using namespace cv;
using namespace std;

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

void getCameraTexture(
    void* camera,
    unsigned char* data,
    bool executeHT21,
    bool executeMedianBlur,
    double dp,
    double minDist,
    double param1,
    double param2,
    int minRadius,
    int maxRadius)
{
    auto cap = static_cast<cv::VideoCapture*>(camera);

    cv::Mat img;
    *cap >> img;
    Mat src = img;

    if (executeHT21)
    {
        Mat gray;
        cvtColor(src, gray, COLOR_BGR2GRAY);

        if (executeMedianBlur)
        {
            medianBlur(gray, gray, 5);
        }

        vector<Vec3f> circles;
        HoughCircles(
            gray,             // inputArray
            circles,          // outputArray
            HOUGH_GRADIENT,   // method
            dp,               // dp
            minDist,          // minDist
            param1,           // param1
            param2,           // param2
            minRadius,        // minRadius
            maxRadius         // maxRadius
        );

        for (size_t i = 0; i < circles.size(); i++)
        {
            Vec3i c = circles[i];
            Point center = Point(c[0], c[1]);
            // circle center
            circle(src, center, 1, Scalar(0, 100, 100), 3, LINE_AA);
            // circle outline
            int radius = c[2];
            circle(src, center, radius, Scalar(255, 0, 255), 3, LINE_AA);
        }
    }

    // RGB --> ARGB •ÏŠ·
    cv::Mat argb_img;
    cv::cvtColor(src, argb_img, 2);
    std::vector<cv::Mat> bgra;
    cv::split(argb_img, bgra);
    std::swap(bgra[0], bgra[3]);
    std::swap(bgra[1], bgra[2]);
    std::memcpy(data, argb_img.data, argb_img.total() * argb_img.elemSize());
}

