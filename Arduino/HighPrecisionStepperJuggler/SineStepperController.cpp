/*
  SineStepperController library
  Author: T-Kuhn.
  Sapporo, October, 2018. Released into the public domain.
  */

#include "Constants.h"
#include "Arduino.h"
#include "SineStepper.h"
#include "SineStepperController.h"
#include "Queue.h"

// - - - - - - - - - - - - - - -
// - - - - CONSTRUCTOR - - - - -
// - - - - - - - - - - - - - - -
SineStepperController::SineStepperController(bool repeat)
{
    _endlessRepeat = repeat;
    _counter = 0;
    _numOfAttachedSteppers = 0;
    _isExecutingBatch = false;

    for (uint8_t i = 0; i < MAX_NUM_OF_STEPPERS; i++)
    {
        _sineSteppers[i] = {0};
    }
}

// - - - - - - - - - - - - - - -
// - - - - - ATTACH  - - - - - -
// - - - - - - - - - - - - - - -
void SineStepperController::attach(SineStepper *sStepper)
{
    if (sStepper->id < MAX_NUM_OF_STEPPERS)
    {
        _sineSteppers[sStepper->id] = sStepper;
        _numOfAttachedSteppers++;
    }
}

// - - - - - - - - - - - - - - -
// - - -  ADD MOVE BATCH - - - -
// - - - - - - - - - - - - - - -
void SineStepperController::addMoveBatch(MoveBatch mb)
{
    _batchQueue.push(mb);
    _batchQueue.setResetPoint();
}

// - - - - - - - - - - - - - - -
// - - -  POP MOVE BATCH - - - -
// - - - - - - - - - - - - - - -
// NOTE: for debugging purposes.
MoveBatch SineStepperController::popMoveBatch()
{
    return _batchQueue.pop();
}

// - - - - - - - - - - - - - - -
// - - -  PEEK MOVE BATCH  - - -
// - - - - - - - - - - - - - - -
// NOTE: for debugging purposes.
MoveBatch SineStepperController::peekMoveBatch()
{
    return _batchQueue.peek();
}

// - - - - - - - - - - - - - - -
// - - -  SET FREQ FROM  - - - -
// - - - - - - - - - - - - - - -
void SineStepperController::setFrequencyFrom(float moveDuration)
{
    _frequency = FREQUENCY_MULTIPLIER * M_PI / moveDuration;
}

// - - - - - - - - - - - - - - -
// - - - - - UPDATE  - - - - - -
// - - - - - - - - - - - - - - -
void SineStepperController::update()
{
    if (!_isExecutingBatch)
    {
        // LOAD NEW BATCH
        if (_batchQueue.peek().isActive)
        {
            MoveBatch mb = _batchQueue.pop();
            setFrequencyFrom(mb.moveDuration);
            for (uint8_t i = 0; i < _numOfAttachedSteppers; i++)
            {
                if (mb.batch[i].isActive)
                {
                    _sineSteppers[i]->setGoalPos(mb.batch[i].position);
                }
                else
                {
                    _sineSteppers[i]->setStepsToTakeToZero();
                }
            }
            _isExecutingBatch = true;
        }
        else if (_endlessRepeat)
        {
            _batchQueue.reset();
        }
        else
        {
            _batchQueue.clear();
        }
    }
    else
    {
        // GENERATE PULSES
        _counter++;
        // Theta goes from 0 ~ M_PI
        float theta = _counter * _frequency;
        // cosine takes values form 0 ~ 2.
        // starting at 2 and ending at 0.

        float cosine = (cosf(theta) + 1.0);

        for (uint8_t i = 0; i < MAX_NUM_OF_STEPPERS; i++)
        {
            if (_sineSteppers[i] != 0)
            {
                _sineSteppers[i]->update(cosine);
            }
        }

        if (theta > M_PI)
        {
            _isExecutingBatch = false;
            _counter = 0;
        }
    }
}
