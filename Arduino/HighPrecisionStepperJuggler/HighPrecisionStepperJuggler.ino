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
    doingControlledMovements,
    error
};

Mode currentMode = idle;

Encoder Encoder1(ROTARY_ENC_1_A, ROTARY_ENC_1_B);

SineStepper sineStepper1(STEPPER1_STEP_PIN, STEPPER1_DIR_PIN, /*id:*/ 0);
SineStepper sineStepper2(STEPPER2_STEP_PIN, STEPPER2_DIR_PIN, /*id:*/ 1);
SineStepper sineStepper3(STEPPER3_STEP_PIN, STEPPER3_DIR_PIN, /*id:*/ 2);
SineStepper sineStepper4(STEPPER4_STEP_PIN, STEPPER4_DIR_PIN, /*id:*/ 3);

SineStepperController sineStepperController(/*endlessRepeat:*/ false);
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
    case idle:
        handleModeChange(doingControlledMovements);
        break;
    case doingControlledMovements:
        sineStepperController.update();
        handleModeChange(idle);
        break;
    default:
        break;
    }
    digitalWrite(EXECUTING_ISR_CODE, LOW);
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
}

void loop()
{
    if (Serial.available() > 0)
    {
        currentMode = idle;
        // Get next command from Serial (add 1 for final 0)
        char input[INPUT_SIZE + 1];
        byte size = Serial.readBytes(input, INPUT_SIZE);
        // Add the final 0 to end the C string
        input[size] = 0;

        double instructionData[12];
        for (int i = 0; i++; i < 12)
        {
            instructionData[i] = 0.0;
        }

        char s[2] = ":";
        char *token;
        int numberOfTokens = 0;

        // Read instruction
        char *instruction = strtok(input, "&");

        // get the first token
        token = strtok(instruction, s);
        instructionData[numberOfTokens++] = atof(token);
        //Serial.println(atof(token), 5);

        // walk through other tokens
        while (token != NULL)
        {
            token = strtok(NULL, s);
            if (token != NULL)
            {
                instructionData[numberOfTokens++] = atof(token);
                //Serial.println(atof(token), 5);
            }
        }

        MoveBatch mb;
        if (instructionData[0] > 10.9 && instructionData[0] < 11.1)
        {
            mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[1] / (M_PI * 2))));
            mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[2] / (M_PI * 2))));
            mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[3] / (M_PI * 2))));
            mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[4] / (M_PI * 2))));
            mb.moveDuration = instructionData[5];
            sineStepperController.addMoveBatch(mb);
        }
        if (instructionData[6] > 21.9 && instructionData[0] < 22.1)
        {
            mb.addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[7] / (M_PI * 2))));
            mb.addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[8] / (M_PI * 2))));
            mb.addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[9] / (M_PI * 2))));
            mb.addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[10] / (M_PI * 2))));
            mb.moveDuration = instructionData[11];
            sineStepperController.addMoveBatch(mb);
        }

        currentMode = doingControlledMovements;
    }
}