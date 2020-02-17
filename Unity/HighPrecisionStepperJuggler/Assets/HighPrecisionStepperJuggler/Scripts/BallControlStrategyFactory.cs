using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class BallControlStrategyFactory
    {
        public static IBallControlStrategy CreateGetBouncingBCS()
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                var moveTime = 0.1f;
                machineController.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.01f, 0f, 0f, 0.5f),
                    new HLInstruction(0.05f, 0f, 0f, 0.5f),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                });
                ballData.ResetVelocityAccumulation();
                return true;
            }, 1);
        }

        public static IBallControlStrategy CreateContinuousBouncingBCS(int duration)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < 150f)
                {
                    // distance away from plate:
                    var p_x = -ballData.CurrentPositionVector.x * c.k_p;
                    var p_y = ballData.CurrentPositionVector.y * c.k_p;

                    // mean velocity of ball:
                    var velocityVector = ballData.GetVelocityVector();
                    var d_x = -velocityVector.x * c.k_d;
                    var d_y = velocityVector.y * c.k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.06f, xCorrection, yCorrection, moveTime),
                        new HLInstruction(0.05f, 0f, 0f, moveTime),
                    });

                    ballData.ResetVelocityAccumulation();
                    return true;
                }

                return false;
            }, duration);
        }
    }
}
