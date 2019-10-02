/*
  Encoder.cpp - A small motor encoder library for the Arduino
  prototyping platform. This library provides a way to evaluate
  the A and B signal of a DC motor encoder. Created by Tobias Kuhn, 
  Sapporo, February 29, 2016. Released into the public domain.
  */

#ifndef Encoder_h
#define Encoder_h
#include "Arduino.h"

// - - - - - - - - - - - - - - - - - - -
// - - - - - Encoder CLASS - - - - - - -
// - - - - - - - - - - - - - - - - - - -
class Encoder
{
  public:
    Encoder(int pinSignalA, int pinSignalB);
    void update();
    int count;
    int currentRot;
    // currentRot:
    // 0 -> rotating CCW
    // 1 -> rotating CW
  private:
    int _pinSignalA;
    int _pinSignalB;
    int _state;
    // _state:
    // 0 -> waiting for first rising edge
    // 1 -> Signal A first edge, first detection
    // 2 -> Signal A second detection -> edge locked in
    // 3 -> Signal B first edge, first detection
    // 4 -> Signal B second detection -> edge locked in
    // 5 -> Other Signal detected. waiting for both falling edges then return to 0
};

#endif
