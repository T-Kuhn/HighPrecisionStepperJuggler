/*
  MoveBatch
  Author: T-Kuhn.
  Sapporo, January, 2020. Released into the public domain.
*/

#include "Constants.h"
#include "Arduino.h"
#include "MoveBatch.h"

// - - - - - - - - - - - - - - - - - - - - -
// - - - - SineStepper CONSTRUCTOR - - - - -
// - - - - - - - - - - - - - - - - - - - - -
MoveBatch::MoveBatch()
{
    needsExecution = false;
    moveDuration = 1.0;

    for (uint8_t i = 0; i < MAX_NUM_OF_STEPPERS; i++)
    {
        moveCommands[i] = {false, 0};
    }
}

void MoveBatch::addMove(uint8_t id, int32_t pos)
{
    if (id < MAX_NUM_OF_STEPPERS)
    {
        needsExecution = true;
        moveCommands[id] = {true, pos};
    }
}
