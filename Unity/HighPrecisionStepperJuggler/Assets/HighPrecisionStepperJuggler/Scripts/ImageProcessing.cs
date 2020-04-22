using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public sealed class ImageProcessing
    {
        private List<Vector2Int> _positiveProbePoints;
        private List<Vector2Int> _boarderPixelPositions;
        private List<(Vector2Int center, float radius)> _detectedObjects;

        private Vector2Int[] _cyclePattern1 = new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
        };

        private Vector2Int[] _cyclePattern2 = new Vector2Int[]
        {
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
        };

        private Vector2Int[] _cyclePattern3 = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
        };

        private Vector2Int[] _cyclePattern4 = new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
        };

        private Vector2Int[] _cyclePattern5 = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
        };

        private Vector2Int[] _cyclePattern6 = new Vector2Int[]
        {
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
        };

        private Vector2Int[] _cyclePattern7 = new Vector2Int[]
        {
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
        };

        private Vector2Int[] _cyclePattern8 = new Vector2Int[]
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
        };

        public ImageProcessing()
        {
            _positiveProbePoints = new List<Vector2Int>(200);
            _boarderPixelPositions = new List<Vector2Int>(20000);
            _detectedObjects = new List<(Vector2Int center, float radius)>();
        }

        public List<BallRadiusAndPosition> BallDataFromPixelBoarders(Color32[] pixels)
        {
            _positiveProbePoints.Clear();

            // we are trying to find the boarders of all r == 255 pixel clusters.
            for (int height = 0; height < c.CameraResolutionHeight; height += c.PixelSpacing)
            {
                for (int width = 0; width < c.CameraResolutionWidth; width += c.PixelSpacing)
                {
                    var index = height * c.CameraResolutionWidth + width;

                    if (pixels[index].b > c.Threshold)
                    {
                        pixels[index].g = 255;
                        // we found a ball-pixel
                        _positiveProbePoints.Add(new Vector2Int(width, height));
                    }
                    else
                    {
                        pixels[index].g = 0;
                    }
                }
            }

            _detectedObjects.Clear();
            foreach (var probe in _positiveProbePoints)
            {
                bool isInsideDetectedObject = false;
                foreach (var detectedObject in _detectedObjects)
                {
                    var distance = Vector2Int.Distance(detectedObject.center, probe);

                    if (distance < detectedObject.radius * 1.5f)
                    {
                        isInsideDetectedObject = true;
                    }
                }

                if (isInsideDetectedObject)
                {
                    continue;
                }

                // find boarder pixel
                var offset = -1;
                while (pixels.AtPosition(probe + Vector2Int.right * offset).b > c.Threshold)
                {
                    offset--;
                }

                offset++;

                var currentPixel = probe + Vector2Int.right * offset;
                var startPixelPosition = currentPixel;
                var lastProbePositionRelativeToCurrentPixel = Vector2Int.left;
                pixels.AtPosition(currentPixel).r = 255;

                _boarderPixelPositions.Clear();

                while (true)
                {
                    bool foundPixel = false;
                    var cyclePattern = RotationPatternFromLastProbe(lastProbePositionRelativeToCurrentPixel);

                    for (int i = 0; i < 8; i++)
                    {
                        if (pixels.AtPosition(currentPixel + cyclePattern[i]).b > c.Threshold)
                        {
                            // we found a new foreground pixel!
                            lastProbePositionRelativeToCurrentPixel = cyclePattern[i - 1] - cyclePattern[i];

                            currentPixel = currentPixel + cyclePattern[i];
                            pixels.AtPosition(currentPixel).r = 255;
                            _boarderPixelPositions.Add(currentPixel);

                            foundPixel = true;
                            break;
                        }
                    }

                    if (!foundPixel)
                    {
                        // it's an island pixel
                        break;
                    }

                    if (currentPixel == startPixelPosition)
                    {
                        // the boarder is complete


                        var dataPoints = new List<(Vector2Int center, float radius)>()
                        {
                            CalculateCenterAndRadius(0,
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.6),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.12),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.18),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.24),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.30),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.36),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.42),
                                _boarderPixelPositions),
                            CalculateCenterAndRadius((int) (_boarderPixelPositions.Count * 0.48),
                                _boarderPixelPositions),
                        };

                        var biggestSix = dataPoints
                            .OrderByDescending(dataPoint => dataPoint.radius)
                            .Take(6)
                            .ToList();

                        Vector2Int accumulatedCenter = new Vector2Int(0, 0);
                        float accumulatedRadius = 0f;
                        foreach (var dataPoint in biggestSix)
                        {
                            accumulatedCenter += dataPoint.center;
                            accumulatedRadius += dataPoint.radius;
                        }

                        _detectedObjects.Add(
                            (new Vector2Int(
                                    Mathf.RoundToInt(accumulatedCenter.x / (float) biggestSix.Count),
                                    Mathf.RoundToInt(accumulatedCenter.y / (float) biggestSix.Count)),
                                accumulatedRadius / biggestSix.Count));

                        break;
                    }
                }
            }

            var sortedData = _detectedObjects
                .OrderByDescending(data => data.radius)
                .Take(2)
                .ToList();

            foreach (var data in sortedData)
            {
                for (int i = 0; i < (int) data.radius; i++)
                {
                    pixels.AtPosition(data.center + Vector2Int.right * i).r = 255;
                }
            }

            return sortedData.Select(data => new BallRadiusAndPosition()
                {
                    Radius = data.radius,
                    PositionX = -data.center.x + c.CameraResolutionWidth / 2f,
                    PositionY = -data.center.y + c.CameraResolutionHeight / 2f
                })
                .ToList();
        }

        private (Vector2Int center, float radius) CalculateCenterAndRadius(int atIndex, List<Vector2Int> boarderPixels)
        {
            var position = boarderPixels[atIndex];
            Vector2Int maxDistBoarderPixel = new Vector2Int(0, 0);
            float maxDistance = 0f;
            foreach (var boarderPixel in boarderPixels)
            {
                var distance = Vector2Int.Distance(position, boarderPixel);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxDistBoarderPixel = boarderPixel;
                }

            }

            var center = new Vector2Int(
                (maxDistBoarderPixel.x + position.x) / 2,
                (maxDistBoarderPixel.y + position.y) / 2);
            var radius = maxDistance / 2;
            
            return (center, radius);
        }

        private Vector2Int[] RotationPatternFromLastProbe(Vector2Int lastProbe)
        {
            if (lastProbe.x == 1)
            {
                // pattern 6: (1, 1)
                if (lastProbe.y == 1)
                    return _cyclePattern4;

                // pattern 4: (1, -1)
                if (lastProbe.y == -1)
                    return _cyclePattern6;

                // pattern 5: (1, 0)
                return _cyclePattern5;
            }

            if (lastProbe.x == -1)
            {
                // pattern 8: (-1, 1)
                if (lastProbe.y == 1)
                    return _cyclePattern2;

                // pattern 2: (-1, -1)
                if (lastProbe.y == -1)
                    return _cyclePattern8;

                // pattern 1: (-1, 0)
                return _cyclePattern1;
            }

            // pattern 7: (0, 1)
            if (lastProbe.y == 1)
                return _cyclePattern3;

            // pattern 3: (0, -1)
                return _cyclePattern7;
        }


        public BallRadiusAndPosition BallDataFromArea(Color32[] pixels)
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
                    pixels[i].r = 255;
                    pixels[i].g = 100;
                    pixels[i].b = 0;
                }
                else
                {
                    pixels[i].r = 0;
                    pixels[i].g = 0;
                    pixels[i].b = 0;
                }
            }

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
