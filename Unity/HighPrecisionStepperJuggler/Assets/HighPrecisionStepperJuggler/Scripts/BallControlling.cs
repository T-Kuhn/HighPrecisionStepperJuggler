
using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class BallControlling
    {
        public static void MoveToHeightWithXYTiltCorrection(MachineController machineController,
            ITiltController tiltController, BallData ballData, float height, float moveTime,
            Vector2? target)
        {
            var tilt = tiltController.CalculateTilt(
                new Vector2(ballData.CurrentPositionVector.x, ballData.CurrentPositionVector.y),
                new Vector2(ballData.CurrentVelocityVector.x, ballData.CurrentVelocityVector.y),
                target ?? Vector2.zero,
                ballData.CalculatedOnBounceDownwardsVelocity,
                ballData.AirborneTime);

            var xCorrection = Mathf.Clamp(tilt.xTilt, c.MinTiltAngle, c.MaxTiltAngle);
            var yCorrection = Mathf.Clamp(tilt.yTilt, c.MinTiltAngle, c.MaxTiltAngle);

            machineController.SendInstructions(new List<HLInstruction>()
            {
                new HLInstruction(height, xCorrection, yCorrection, moveTime),
            });
        }

        public static void MoveToHeightWithAlternatingXYTiltCorrection(MachineController machineController,
            ITiltController tiltController, BallData ballData, float height, float moveTime,
            int instructionCount, Vector2? target)
        {
            var tilt = tiltController.CalculateTilt(
                new Vector2(ballData.CurrentPositionVector.x, ballData.CurrentPositionVector.y),
                new Vector2(ballData.CurrentVelocityVector.x, ballData.CurrentVelocityVector.y),
                target ?? Vector2.zero,
                ballData.CalculatedOnBounceDownwardsVelocity,
                ballData.AirborneTime);

            var xCorrection = instructionCount % 2 == 1
                ? Mathf.Clamp(tilt.xTilt, c.MinTiltAngle, c.MaxTiltAngle)
                : 0f;
            var yCorrection = instructionCount % 2 == 0
                ? Mathf.Clamp(tilt.yTilt, c.MinTiltAngle, c.MaxTiltAngle)
                : 0f;

            machineController.SendInstructions(new List<HLInstruction>()
            {
                new HLInstruction(height, xCorrection, yCorrection, moveTime),
            });
        }
    }
}
