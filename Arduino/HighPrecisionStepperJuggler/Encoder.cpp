/*
  Encoder.cpp - A small motor encoder library for the Arduino
  prototyping platform. This library provides a way to evaluate
  the A and B signal of a DC motor encoder. Created by Tobias Kuhn, 
  Sapporo, February 29, 2016. Released into the public domain.
  */

#include "Arduino.h"
#include "Encoder.h"

// - - - - - - - - - - - - - - - - - - -
// - - - - Encoder CONSTRUCTOR - - - - -
// - - - - - - - - - - - - - - - - - - -
Encoder::Encoder(int pinSignalA, int pinSignalB)
{
    _pinSignalA = pinSignalA;
    _pinSignalB = pinSignalB;
    pinMode(_pinSignalA, INPUT);
    pinMode(_pinSignalB, INPUT);
    count = 0;
    _state = 0;
}

// - - - - - - - - - - - - - - - - - - -
// - - - - - Encoder UPDATE  - - - - - -
// - - - - - - - - - - - - - - - - - - -
void Encoder::update()
{
    int a = digitalRead(_pinSignalA);
    int b = digitalRead(_pinSignalB);

    if (_state == 0)
    {
        if (a)
        {
            _state = 1;
        }
        else if (b)
        {
            _state = 3;
        }
    }
    else if (_state == 1)
    {
        if (a)
        {
            _state = 2;
            currentRot = 0;
        }
        else
        {
            _state = 0;
        }
    }
    else if (_state == 3)
    {
        if (b)
        {
            _state = 4;
            currentRot = 1;
        }
        else
        {
            _state = 0;
        }
    }
    else if (_state == 2)
    {
        if (a)
        {
            //do nothing
        }
        else if (b)
        {
            _state = 5;
        }
        else
        {
            //go back to 0 in the case "no a and no b"
            _state = 0;
        }
    }
    else if (_state == 4)
    {
        if (b)
        {
            //do nothing
        }
        else if (a)
        {
            _state = 6;
        }
        else
        {
            //go back to 0 in the case "no a and no b"
            _state = 0;
        }
    }
    else if (_state == 5)
    {
        if (a && !b)
        {
            _state = 2;
        }
        else if (!a && !b)
        {
            count--;
            _state = 0;
        }
    }
    else if (_state == 6)
    {
        if (!a && b)
        {
            _state = 4;
        }
        else if (!a && !b)
        {
            count++;
            _state = 0;
        }
    }
}
