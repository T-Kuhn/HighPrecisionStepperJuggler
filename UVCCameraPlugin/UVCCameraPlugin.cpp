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
    int imgMode,            // 0: src, 1: red, 2: green, 3: blue, 4: normalgray, 5: customgray 
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

    Mat bgr[3];
    split(src, bgr);
    Mat r = Mat(src.rows, src.cols, CV_8U, bgr[2].data);
    Mat g = Mat(src.rows, src.cols, CV_8U, bgr[1].data);
    Mat b = Mat(src.rows, src.cols, CV_8U, bgr[0].data);

    Mat gray;

    if (imgMode == 4)
    {
        cv::cvtColor(src, gray, COLOR_BGR2GRAY);
    }
    else
    {
        gray = r - b;
    }

    if (executeHT21)
    {
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

        if (imgMode == 0)
        {
            for (size_t i = 0; i < circles.size(); i++)
            {
                Vec3i c = circles[i];
                Point center = Point(c[0], c[1]);
                int radius = c[2];

                circle(src, center, 1, Scalar(0, 100, 100), 3, LINE_AA);
                circle(src, center, radius, Scalar(255, 0, 255), 3, LINE_AA);
            }
        }
    }

    // 0: src, 1: red, 2: green, 3: blue, 4: normalgray, 5: customgray
    cv::Mat rgba;
    if (imgMode == 0)
    {
        cv::cvtColor(src, rgba, cv::COLOR_BGR2RGBA);
    }
    else if (imgMode == 1)
    {
        cv::cvtColor(r, rgba, cv::COLOR_GRAY2RGBA);
    }
    else if (imgMode == 2)
    {
        cv::cvtColor(g, rgba, cv::COLOR_GRAY2RGBA);
    }
    else if (imgMode == 3)
    {
        cv::cvtColor(b, rgba, cv::COLOR_GRAY2RGBA);
    }
    else if (imgMode == 4 || imgMode == 5)
    {
        cv::cvtColor(gray, rgba, cv::COLOR_GRAY2RGBA);
    }
    std::memcpy(data, rgba.data, rgba.total() * rgba.elemSize());
}

