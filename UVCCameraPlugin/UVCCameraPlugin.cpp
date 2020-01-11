#include "UVCCameraPlugin.h"
#include <opencv2/opencv.hpp>
#include <cstdio>

void* getCamera()
{
    //cv::namedWindow("hoge", cv::WindowFlags::WINDOW_AUTOSIZE | cv::WindowFlags::WINDOW_FREERATIO);

    auto cap = new cv::VideoCapture(0);
    cap->set(cv::CAP_PROP_FRAME_WIDTH, 640);
    cap->set(cv::CAP_PROP_FRAME_HEIGHT, 480); 

    cap->set(cv::CAP_PROP_EXPOSURE, -7);
    cap->set(cv::CAP_PROP_GAIN, 4);
    //cap->get(cv::CAP_PROP_EXPOSURE);

    return static_cast<void*>(cap);
}

void releaseCamera(void* camera)
{
    //cv::destroyWindow("hoge");
    auto vc = static_cast<cv::VideoCapture*>(camera);
    delete vc;
}

void getCameraTexture(void* camera, unsigned char* data, int width, int height)
{
    auto vc = static_cast<cv::VideoCapture*>(camera);

    // カメラ画の取得
    cv::Mat img;
    *vc >> img;

    // リサイズ
    cv::Mat resized_img(height, width, img.type());
    cv::resize(img, resized_img, resized_img.size(), cv::INTER_CUBIC);

    //cv::imshow("hoge", resized_img);

    // RGB --> ARGB 変換
    cv::Mat argb_img;
    cv::cvtColor(resized_img, argb_img, 2);
    std::vector<cv::Mat> bgra;
    cv::split(argb_img, bgra);
    std::swap(bgra[0], bgra[3]);
    std::swap(bgra[1], bgra[2]);
    std::memcpy(data, argb_img.data, argb_img.total() * argb_img.elemSize());
}

