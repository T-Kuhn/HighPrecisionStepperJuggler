/*
  SineStepper library
  Author: T-Kuhn.
  Sapporo, January, 2020. Released into the public domain.
  */

#include "Arduino.h"
#include "SineStepper.h"
#include "Constants.h"

// - - - - - - - - - - - - - - -
// - - - - CONSTRUCTOR - - - - -
// - - - - - - - - - - - - - - -
SineStepper::SineStepper(uint8_t pinStep, uint8_t pinDir, uint8_t stepperID)
{
    currentPos = 0;
    id = stepperID;

    _pinStep = pinStep;
    _pinDir = pinDir;
    _isMovingCW = true;
    _goalPosition = 0;
    _currentStepsToTake = 0;
    _lastPulse = 0;

    pinMode(_pinStep, OUTPUT);
    pinMode(_pinDir, OUTPUT);
}

// - - - - - - - - - - - - - - -
// - - - - - UPDATE  - - - - - -
// - - - - - - - - - - - - - - -
void SineStepper::update(float cosine)
{
    uint8_t pulse = pulseFromAmplitude(_currentStepsToTake, cosine);
    digitalWrite(_pinStep, pulse);
    _lastPulse = pulse;
}

// - - - - - - - - - - - - - - -
// - - - - SET GOAL POS  - - - -
// - - - - - - - - - - - - - - -
void SineStepper::setGoalPos(int32_t goalPos)
{
    _goalPosition = goalPos;
    _currentStepsToTake = goalPos - currentPos;

    if (_currentStepsToTake > 0)
    {
        digitalWrite(_pinDir, LOW);
        _isMovingCW = true;
    }
    else
    {
        digitalWrite(_pinDir, HIGH);
        _isMovingCW = false;
    }
}

// - - - - - - - - - - - - - - -
// - SET STEPS TO TAKE TO ZERO -
// - - - - - - - - - - - - - - -
void SineStepper::setStepsToTakeToZero()
{
    _currentStepsToTake = 0;
}

// - - - - - - - - - - - - - - -
// - -  PULSE FROM AMPLITUDE - -
// - - - - - - - - - - - - - - -
uint8_t SineStepper::pulseFromAmplitude(float stepsToTake, float cosine)
{
    uint32_t doubledStepCount = (uint32_t)(round(cosine * stepsToTake));
    uint8_t stepLevel = doubledStepCount % 2;

    if (stepLevel > _lastPulse)
    {
        currentPos += _isMovingCW ? 1 : -1;
    }

    return stepLevel;
}
