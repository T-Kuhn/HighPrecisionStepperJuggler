/*
  SineStepper library
  Author: T-Kuhn.
  Sapporo, October, 2018. Released into the public domain.
  */

#ifndef MoveBatch_h
#define MoveBatch_h
#include "Constants.h"
#include "Arduino.h"

struct MoveCommand
{
  bool isActive;
  int32_t position;
};

class MoveBatch
{
public:
  MoveBatch();
  void addMove(uint8_t id, int32_t pos);
  MoveCommand batch[MAX_NUM_OF_STEPPERS];
  bool isActive;
  float moveDuration;
};

#endif
