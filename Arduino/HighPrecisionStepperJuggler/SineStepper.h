/*
  SineStepper library
  Author: T-Kuhn.
  Sapporo, January, 2020. Released into the public domain.
  */

#ifndef SineStepper_h
#define SineStepper_h
#include "Constants.h"
#include "Arduino.h"

class SineStepper
{
public:
  SineStepper(uint8_t pinStep, uint8_t pinDir, uint8_t stepperID);
  void update(float cosine);
  void setGoalPos(int32_t goalPos);
  void setStepsToTakeToZero();
  int32_t currentPos;
  int8_t id;

private:
  uint8_t pulseFromAmplitude(float stepsToTake, float cosine);
  int32_t _goalPosition;
  int32_t _currentStepsToTake;
  uint8_t _pinStep;
  uint8_t _pinDir;
  uint8_t _lastPulse;
  bool _isMovingCW;
};

#endif
