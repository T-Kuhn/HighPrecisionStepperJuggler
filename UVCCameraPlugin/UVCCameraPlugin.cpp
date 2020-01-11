#include "UVCCameraPlugin.h"

void createCheckTexture(unsigned char* arr, int w, int h, int ch)
{
    int n = 0;
    for (int i = 0; i < w; ++i) {
        for (int j = 0; j < h; ++j) {
            for (int k = 0; k < ch; ++k) {
                arr[n++] = ((i + j) % 2 == 0) ? 255 : 0;
            }
        }
    }
}