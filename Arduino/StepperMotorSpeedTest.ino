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

int buttonCoolDownCounter = 0;
hw_timer_t *timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
volatile SemaphoreHandle_t timerSemaphore;
uint32_t cp0_regs[18];

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

void IRAM_ATTR onTimer()
{
    digitalWrite(EXECUTING_ISR_CODE, HIGH);
    // get FPU state
    uint32_t cp_state = xthal_get_cpenable();

    if (cp_state)
    {
        // Save FPU registers
        xthal_save_cp0(cp0_regs);
    }
    else
    {
        // enable FPU
        xthal_set_cpenable(1);
    }

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
    xSemaphoreGiveFromISR(timerSemaphore, NULL);
    digitalWrite(EXECUTING_ISR_CODE, LOW);

    if (cp_state)
    {
        // Restore FPU registers
        xthal_restore_cp0(cp0_regs);
    }
    else
    {
        // turn it back off
        xthal_set_cpenable(0);
    }
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

    pinMode(EXECUTING_ISR_CODE, OUTPUT);
    pinMode(BUTTON_PIN, INPUT);

    timerSemaphore = xSemaphoreCreateBinary();
    sineStepperController.attach(&sineStepper1);

    // initialize MoveBatches
    MoveBatch mb;
    mb.moveDuration = 0.15;

    //repeatabilityTest(mb);
    setupMoveBatch(mb);

    // Set 80 divider for prescaler
    timer = timerBegin(0, 80, true);
    timerAttachInterrupt(timer, &onTimer, true);
    // onTimer gets called every 50uS.
    timerAlarmWrite(timer, TIMER_US, true);

    timerAlarmEnable(timer);
}

void loop()
{
    if (xSemaphoreTake(timerSemaphore, 0) == pdTRUE)
    {
        int32_t pos = 0;

        //portENTER_CRITICAL(&timerMux);
        pos = sineStepper1.currentPos;
        //portEXIT_CRITICAL(&timerMux);

        delay(10);
    }
}