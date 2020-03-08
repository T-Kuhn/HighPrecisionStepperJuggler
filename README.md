# HighPrecisionStepperJuggler
a.k.a Octo-Bouncer. Highly precise stepper motor driving with a Teensy 4.0 and custom pulse generating algorithm and PC based image processing with the goal of getting a machine to juggle a ping pong ball.

Here's a blog post with more infos about this project:
https://electrondust.com/2020/03/01/the-octo-bouncer/

https://youtu.be/lYyAMDYzJQM

The Arduino folder contains the code for the Teensy 4.0. The Teensy 4.0's job is simple:

- Listen for movement comments on the serial bus
- Generate pulses for the stepper motors

The Unity folder contains the code for the Unity Application. This Application is responsible for:

- Setting up the camera (120 FPS 640×480 data stream, gain, exposure, contrast, ISO, saturation) via OpenCV
- Execute OpenCVs Hough Transform circle recognition algorithm
- Get 3D position of ping pong ball using the results of above-mentioned circle recognition algorithm
- Calculate ball velocity
- Use ball position and velocity in PID control code to calculate correction-tilt of plate
- Execute Inverse Kinematic code to figure out how much each motor needs to rotate in order to get the plate to a certain height with a     specific tilt.
- Send result of IK calculation to the microcontroller via serial interface
- Render machine position and movements
- Also render image processing data

The UVCCameraPlugin folder contains the C++ code for the camera plugin. This is a Unity plugin. All the OpenCV code is being executed in here.
