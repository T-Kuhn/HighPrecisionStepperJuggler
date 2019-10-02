/*
  ESP-32 StepperMotorSpeedTest
  Author: T-Kuhn.
  Sapporo, October, 2019. Released into the public domain.
 */

#include "Constants.h"
#include "SineStepper.h"
#include "SineStepperController.h"
#include "Queue.h"
#include "MoveBatch.h"
#include "Encoder.h"

enum Mode
{
    adjustingJoint1,
    adjustingJoint2,
    adjustingJoint3,
    adjustingJoint4,
    doingControlledMovements,
    error
};

Mode currentMode = adjustingJoint1;

Encoder Encoder1(ROTARY_ENC_1_A, ROTARY_ENC_1_B);

SineStepper sineStepper1(STEPPER1_STEP_PIN, STEPPER1_DIR_PIN, /*id:*/ 0);
SineStepperController sineStepperController(/*endlessRepeat:*/ true);
IntervalTimer myTimer;

int buttonCoolDownCounter = 0;

void handleModeChange(Mode newMode)
{
    if (buttonCoolDownCounter < BUTTON_COOLDOWN_CYCLES)
    {
        buttonCoolDownCounter++;
    }
    if (digitalRead(BUTTON_PIN) && buttonCoolDownCounter >= BUTTON_COOLDOWN_CYCLES)
    {
        buttonCoolDownCounter = 0;
        currentMode = newMode;
    }
}

void onTimer()
{
    digitalWrite(EXECUTING_ISR_CODE, HIGH);

    switch (currentMode)
    {
    case adjustingJoint1:
        Encoder1.update();
        digitalWrite(STEPPER1_DIR_PIN, Encoder1.currentRot);
        digitalWrite(STEPPER1_STEP_PIN, Encoder1.count % 2);

        handleModeChange(doingControlledMovements);
        break;
    case adjustingJoint2:
        Encoder1.update();
        digitalWrite(STEPPER2_DIR_PIN, Encoder1.currentRot);
        digitalWrite(STEPPER2_STEP_PIN, Encoder1.count % 2);

        //handleModeChange(adjustingJoint3);
        break;
    case adjustingJoint3:
        Encoder1.update();
        digitalWrite(STEPPER3_DIR_PIN, Encoder1.currentRot);
        digitalWrite(STEPPER3_STEP_PIN, Encoder1.count % 2);

        //handleModeChange(adjustingJoint4);
        break;
    case adjustingJoint4:
        Encoder1.update();
        digitalWrite(STEPPER4_DIR_PIN, Encoder1.currentRot);
        digitalWrite(STEPPER4_STEP_PIN, Encoder1.count % 2);

        //handleModeChange(doingControlledMovements);
        break;
    case doingControlledMovements:
        //portENTER_CRITICAL_ISR(&timerMux);
        sineStepperController.update();
        //portEXIT_CRITICAL_ISR(&timerMux);

        handleModeChange(adjustingJoint1);
        break;
    default:
        break;
    }
    digitalWrite(EXECUTING_ISR_CODE, LOW);
}

void setupMoveBatch(MoveBatch mb)
{
    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.moveDuration = MOVE_DURATION;
    sineStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.moveDuration = PAUSE_DURATION;
    sineStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.moveDuration = MOVE_DURATION;
    sineStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.moveDuration = PAUSE_DURATION;
    sineStepperController.addMoveBatch(mb);
}

void setup()
{
    Serial.begin(115200);
    myTimer.begin(onTimer, 2);

    pinMode(EXECUTING_ISR_CODE, OUTPUT);
    pinMode(BUTTON_PIN, INPUT);
    pinMode(13, OUTPUT);

    sineStepperController.attach(&sineStepper1);

    // initialize MoveBatches
    MoveBatch mb;
    mb.moveDuration = 0.15;

    //repeatabilityTest(mb);
    setupMoveBatch(mb);
}

void loop()
{
    digitalWrite(13, HIGH);
    delay(500);
    digitalWrite(13, LOW);
    delay(500);
}