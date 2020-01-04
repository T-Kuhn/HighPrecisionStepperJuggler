/*
  SineStepperController library
  Author: T-Kuhn.
  Sapporo, January, 2020. Released into the public domain.
*/

#include "Constants.h"
#include "Arduino.h"
#include "SineStepper.h"
#include "SineStepperController.h"

// - - - - - - - - - - - - - - -
// - - - - CONSTRUCTOR - - - - -
// - - - - - - - - - - - - - - -
SineStepperController::SineStepperController(bool repeat)
{
    _endlessRepeat = repeat;
    _counter = 0;
    _numOfAttachedSteppers = 0;
    _currentMoveBatchIndex = 0;
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

// - - - - - - - - - - - - - - - -
// - - RESET MOVEBATCH EXECUTION -
// - - - - - - - - - - - - - - - -
void SineStepperController::resetMoveBatchExecution()
{
    _currentMoveBatchIndex = 0;
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
    if (_currentMoveBatchIndex >= MAX_NUM_OF_MOVEBATCHES)
    {
        return;
    }

    if (!_isExecutingBatch && _currentMoveBatchIndex < MAX_NUM_OF_MOVEBATCHES)
    {
        MoveBatch *mb = &moveBatches[_currentMoveBatchIndex];
        if (mb->needsExecution)
        {
            setFrequencyFrom(mb->moveDuration);
            for (uint8_t i = 0; i < _numOfAttachedSteppers; i++)
            {
                if (mb->moveCommands[i].isActive)
                {
                    _sineSteppers[i]->setGoalPos(mb->moveCommands[i].position);
                }
                else
                {
                    _sineSteppers[i]->setStepsToTakeToZero();
                }
            }
            mb->needsExecution = false;
            _isExecutingBatch = true;
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
            _currentMoveBatchIndex++;
        }
    }
}
