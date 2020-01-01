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
    idle,
    moveUpToZeroPosition,
    doingControlledMovements,
    stop,
    returnToMechanicalZero,
    error
};

Mode currentMode = idle;

Encoder Encoder1(ROTARY_ENC_1_A, ROTARY_ENC_1_B);

SineStepper sineStepper1(STEPPER1_STEP_PIN, STEPPER1_DIR_PIN, /*id:*/ 0);
SineStepper sineStepper2(STEPPER2_STEP_PIN, STEPPER2_DIR_PIN, /*id:*/ 1);
SineStepper sineStepper3(STEPPER3_STEP_PIN, STEPPER3_DIR_PIN, /*id:*/ 2);
SineStepper sineStepper4(STEPPER4_STEP_PIN, STEPPER4_DIR_PIN, /*id:*/ 3);

SineStepper startupSineStepper1(STEPPER1_STEP_PIN, STEPPER1_DIR_PIN, /*id:*/ 0);
SineStepper startupSineStepper2(STEPPER2_STEP_PIN, STEPPER2_DIR_PIN, /*id:*/ 1);
SineStepper startupSineStepper3(STEPPER3_STEP_PIN, STEPPER3_DIR_PIN, /*id:*/ 2);
SineStepper startupSineStepper4(STEPPER4_STEP_PIN, STEPPER4_DIR_PIN, /*id:*/ 3);

SineStepperController startUpStepperController(/*endlessRepeat:*/ false);
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

        if (newMode == returnToMechanicalZero)
        {
            setupMoveToMechanicalZeroMoveBatch();
        }
        if (newMode == idle)
        {
            setupStartUpMoveBatch();
        }
    }
}

void onTimer()
{
    digitalWrite(EXECUTING_ISR_CODE, HIGH);

    switch (currentMode)
    {
    case idle:
        handleModeChange(moveUpToZeroPosition);
        break;
    case moveUpToZeroPosition:
        //portENTER_CRITICAL_ISR(&timerMux);
        startUpStepperController.update();
        //portEXIT_CRITICAL_ISR(&timerMux);

        handleModeChange(doingControlledMovements);
        break;
    case doingControlledMovements:
        sineStepperController.update();

        handleModeChange(stop);
        break;
    case stop:
        handleModeChange(returnToMechanicalZero);
        break;

    case returnToMechanicalZero:
        startUpStepperController.update();
        handleModeChange(idle);
        break;

    default:
        break;
    }
    digitalWrite(EXECUTING_ISR_CODE, LOW);
}

void setupMoveBatch(MoveBatch mb)
{
    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.moveDuration = MOVE_DURATION;
    sineStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 1.0));
    mb.moveDuration = PAUSE_DURATION;
    sineStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.moveDuration = MOVE_DURATION;
    sineStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_TO_MOVE * 0));
    mb.moveDuration = PAUSE_DURATION;
    sineStepperController.addMoveBatch(mb);
}

void setupStartUpMoveBatch()
{
    MoveBatch mb;

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.moveDuration = STARTUP_MOVE_DURATION;
    startUpStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(STARTUP_PULSES_TO_MOVE));
    mb.moveDuration = STARTUP_MOVE_DURATION;
    startUpStepperController.addMoveBatch(mb);
}

void setupMoveToMechanicalZeroMoveBatch()
{
    MoveBatch mb;

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)0);
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)0);
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)0);
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)0);
    mb.moveDuration = STARTUP_MOVE_DURATION;
    startUpStepperController.addMoveBatch(mb);

    mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)0);
    mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)0);
    mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)0);
    mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)0);
    mb.moveDuration = STARTUP_MOVE_DURATION;
    startUpStepperController.addMoveBatch(mb);
}

void setup()
{
    Serial.begin(115200);
    Serial.setTimeout(20);
    myTimer.begin(onTimer, TIMER_US);

    pinMode(EXECUTING_ISR_CODE, OUTPUT);
    pinMode(BUTTON_PIN, INPUT);

    sineStepperController.attach(&sineStepper1);
    sineStepperController.attach(&sineStepper2);
    sineStepperController.attach(&sineStepper3);
    sineStepperController.attach(&sineStepper4);

    startUpStepperController.attach(&startupSineStepper1);
    startUpStepperController.attach(&startupSineStepper2);
    startUpStepperController.attach(&startupSineStepper3);
    startUpStepperController.attach(&startupSineStepper4);

    // initialize MoveBatches
    setupStartUpMoveBatch();

    MoveBatch mb;
    setupMoveBatch(mb);
}

void loop()
{
    if (Serial.available() > 0)
    {
        // Get next command from Serial (add 1 for final 0)
        char input[INPUT_SIZE + 1];
        byte size = Serial.readBytes(input, INPUT_SIZE);
        // Add the final 0 to end the C string
        input[size] = 0;

        // Read command pair
        char *command = strtok(input, "&");
        // Split the command in two values
        char *separator = strchr(command, ':');
        if (separator != 0)
        {
            // Actually split the string in 2: replace ':' with 0
            *separator = 0;
            int ver = atoi(command);
            ++separator;
            int hor = atoi(separator);

            float horizontal = (float)hor;
            float vertical = (float)ver;

            // DEBUG
            Serial.print("v: ");
            Serial.println(ver);
            Serial.print("h: ");
            Serial.println(hor);
            // DEBUG
        }
    }
}