
using System;
using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public sealed class ImageProcessing
    {
        public ImageProcessing()
        {
        }

        public BallRadiusAndPosition Execute(Color32[] pixels)
        {
            int numberOfWhitePixels = 0;
            var pixelWidth = c.CameraResolutionWidth;
            var accumulatedPixelX = 0;
            var accumulatedPixelY = 0;
            
            // 1. We are using the red channel to first create our black-and-white base data
            //    0: background
            //    1: foreground
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].b = 0;

                if (pixels[i].r > 70)
                {
                    accumulatedPixelX += i % pixelWidth;
                    accumulatedPixelY += i / pixelWidth;
                    numberOfWhitePixels++;
                    pixels[i].r = 1;
                    pixels[i].g = 255;
                }
                else
                {
                    pixels[i].r = 0;
                    pixels[i].g = 0;
                }
            }

            Byte newLabel = 1;

            Byte CreateNewLabel()
            {
                return newLabel++;
            }

            // 2. now we're looking at the base data and use the green channel save labels in order to store labels
            //    look at 4 pixels in neighbourhood
            //      IF all the pixels are 0 but the pixel we're looking at is 1 put a new label on the pixel
            //      IF ELSE one of the pixels already has a label, but the same label on the pixel we're looking at
            //         IF there are more than 2 labels on the 4 pixels chose the smallest one for the pixel we're
            //         looking at
            for (int height = 0; height < c.CameraResolutionHeight; height++)
            {
                for (int width = 0; width < c.CameraResolutionWidth; width++)
                {
                    var index = height * c.CameraResolutionWidth + width;
                    if (pixels[index].r == 0)
                    {
                        continue;
                    }

                    // we need to check pixels 0 ~ 3. x is the pixel we're currently looking at, i.e. pixels[i]
                    // [0] [1] [2]  
                    // [3] [x] 
                    var probeStartIndex = (height - 1) * c.CameraResolutionWidth + width -1;

                    Byte smallestLabel = 255;
                    // check pixel at [0], [1], [2]
                    for (int j = 0; j < 3; j++)
                    {
                        var probeIndex = probeStartIndex + j;
                        if (probeIndex >= 0)
                        {
                            if (pixels[probeIndex].r == 1)
                            {
                                if (pixels[probeIndex].g < smallestLabel)
                                {
                                    smallestLabel = pixels[probeIndex].g;
                                }
                            }

                        }
                    }

                    // check pixel at [3]
                    if (width > 0)
                    {
                        var probeIndex = index - 1;
                        if (pixels[probeIndex].r == 1)
                        {
                            if (pixels[probeIndex].g < smallestLabel)
                            {
                                smallestLabel = pixels[probeIndex].g;
                            }
                        }
                    }

                    if (smallestLabel == 255)
                    {
                        smallestLabel = CreateNewLabel();
                    }

                    pixels[index].g = smallestLabel;
                }
            }

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].g *= 10;
            }

            // 3. multiple labels might be on the same pixel cluster. clean up.       

            var meanPixelX = (float) accumulatedPixelX / numberOfWhitePixels;
            var meanPixelY = (float) accumulatedPixelY / numberOfWhitePixels;

            /*
            // color pixel at ball centre white
            var meanPixelIndex = (int) meanPixelY * c.CameraResolutionWidth + (int) meanPixelX;
            _pixels[meanPixelIndex].r = 1;
            _pixels[meanPixelIndex].g = 1;
            _pixels[meanPixelIndex].b = 1;
            */

            // NOTE: use number of pixels and A_c = r^2 * PI to get the radius (r) of the ball
            var pixelRadius = Mathf.Sqrt(numberOfWhitePixels / Mathf.PI);

            return new BallRadiusAndPosition()
            {
                Radius = pixelRadius,
                PositionX = -meanPixelX + c.CameraResolutionWidth / 2f,
                PositionY = -meanPixelY + c.CameraResolutionHeight / 2f
            };
        }
    }
}
