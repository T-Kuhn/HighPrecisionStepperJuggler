/*
  HighPrecisionStepperJuggler
  Author: T-Kuhn.
  Sapporo, January, 2020. Released into the public domain.
 */

#include "Constants.h"
#include "SineStepper.h"
#include "SineStepperController.h"
#include "MoveBatch.h"

enum Mode
{
    idle,
    doingControlledMovements,
    error
};

Mode currentMode = idle;
char inputBuffer[INPUT_SIZE + 1];

SineStepper sineStepper1(STEPPER1_STEP_PIN, STEPPER1_DIR_PIN, /*id:*/ 0);
SineStepper sineStepper2(STEPPER2_STEP_PIN, STEPPER2_DIR_PIN, /*id:*/ 1);
SineStepper sineStepper3(STEPPER3_STEP_PIN, STEPPER3_DIR_PIN, /*id:*/ 2);
SineStepper sineStepper4(STEPPER4_STEP_PIN, STEPPER4_DIR_PIN, /*id:*/ 3);

SineStepperController sineStepperController(/*endlessRepeat:*/ false);
IntervalTimer myTimer;

void onTimer()
{
    digitalWrite(EXECUTING_ISR_CODE, HIGH);

    switch (currentMode)
    {
    case idle:
        break;
    case doingControlledMovements:
        sineStepperController.update();
        break;
    default:
        break;
    }
    digitalWrite(EXECUTING_ISR_CODE, LOW);
}

void setup()
{
    inputBuffer[0] = '\0';
    Serial.begin(921600);
    Serial.setTimeout(1);
    myTimer.begin(onTimer, TIMER_US);

    pinMode(EXECUTING_ISR_CODE, OUTPUT);

    sineStepperController.attach(&sineStepper1);
    sineStepperController.attach(&sineStepper2);
    sineStepperController.attach(&sineStepper3);
    sineStepperController.attach(&sineStepper4);
}

void loop()
{

    if (Serial.available() > 0)
    {
        char inputChar = Serial.read();
        static int s_len;
        if (s_len >= INPUT_SIZE)
        {
            // We have received already the maximum number of characters
            // Ignore all new input until line termination occurs
        }
        else if (inputChar != '\n' && inputChar != '\r')
        {
            inputBuffer[s_len++] = inputChar;
        }
        else
        {
            // We have received a LF or CR character
            //Serial.print("RECEIVED MSG: ");
            //Serial.println(inputBuffer);

            inputBuffer[s_len] = 0;

            currentMode = idle;
            int index = 0;
            double instructionData[MAX_NUM_OF_MOVEBATCHES * 6];
            for (int i = 0; i < MAX_NUM_OF_MOVEBATCHES * 6; i++)
            {
                instructionData[i] = 0;
            }

            // Read each command
            char *command = strtok(inputBuffer, ":");
            while (command != 0)
            {
                instructionData[index] = atof(command);

                command = strtok(0, ":");
                index++;
            }

            int numOfMoveBatches = index / 6;
            for (int i = 0; i < numOfMoveBatches; i++)
            {
                int offset = i * 6;
                MoveBatch *mb = &sineStepperController.moveBatches[i];
                if (instructionData[offset] > ((i + 1) * 11.0) - 0.1 && instructionData[offset] < ((i + 1) * 11) + 0.1)
                {
                    mb->addMove(/*id:*/ 0, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[offset + 1] / (M_PI * 2))));
                    mb->addMove(/*id:*/ 1, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[offset + 2] / (M_PI * 2))));
                    mb->addMove(/*id:*/ 2, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[offset + 3] / (M_PI * 2))));
                    mb->addMove(/*id:*/ 3, /*pos:*/ (int32_t)(PULSES_PER_REV * (instructionData[offset + 4] / (M_PI * 2))));
                    mb->moveDuration = instructionData[offset + 5];
                }
            }

            sineStepperController.resetMoveBatchExecution();
            currentMode = doingControlledMovements;

            memset(inputBuffer, 0, sizeof(inputBuffer));
            s_len = 0;
        }
    }
}