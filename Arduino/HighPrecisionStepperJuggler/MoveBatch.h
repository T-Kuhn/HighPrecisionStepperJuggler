/*
  MoveBatch
  Author: T-Kuhn.
  Sapporo, January, 2020. Released into the public domain.
*/

#ifndef MoveBatch_h
#define MoveBatch_h
#include "Constants.h"
#include "Arduino.h"

struct MoveCommand
{
  // isActive: whether or not the corresponding stepperMotor is active during the MoveBatch this moveCommand is part of.
  bool isActive;
  int32_t position;
};

class MoveBatch
{
public:
  MoveBatch();
  void addMove(uint8_t id, int32_t pos);
  MoveCommand moveCommands[MAX_NUM_OF_STEPPERS];
  bool needsExecution;
  float moveDuration;
};

#endif
