using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class BallControlStrategyFactory
    {
        public static IBallControlStrategy ContinuousBouncing(int duration, ITiltController tiltController,
            float lowPos = 0.5f, float highPos = 0.6f, float moveTime = 0.1f)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < 150f)
                {
                    var tilt = tiltController.CalculateTilt(
                        ballData.CurrentPositionVector,
                        ballData.CurrentVelocityVector,
                        Vector2.zero,
                        ballData.CalculatedOnBounceDownwardsVelocity,
                        ballData.AirborneTime
                    );

                    var xCorrection = Mathf.Clamp(tilt.xTilt, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(tilt.yTilt, c.MinTiltAngle, c.MaxTiltAngle);

                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.06f, xCorrection, yCorrection, moveTime),
                        new HLInstruction(0.05f, 0f, 0f, moveTime),
                    });

                    return true;
                }

                return false;
            }, duration);
        }

        public static IBallControlStrategy ContinuousBouncingStrong(int duration, ITiltController tiltController,
            float lowPos = 0.05f, float highPos = 0.08f, float moveTime = 0.1f)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < 190f)
                {
                    var tilt = tiltController.CalculateTilt(
                        ballData.CurrentPositionVector,
                        ballData.CurrentVelocityVector,
                        Vector2.zero,
                        ballData.CalculatedOnBounceDownwardsVelocity,
                        ballData.AirborneTime
                    );

                    var xCorrection = Mathf.Clamp(tilt.xTilt, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(tilt.yTilt, c.MinTiltAngle, c.MaxTiltAngle);

                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(highPos, xCorrection, yCorrection, moveTime),
                        new HLInstruction(lowPos, 0f, 0f, moveTime),
                    });

                    return true;
                }

                return false;
            }, duration);
        }

        public static IBallControlStrategy Continuous2StepBouncing(int duration, ITiltController tiltController,
            Vector2? target = null, float lowPos = 0.05f, float highPos = 0.058f, float moveTime = 0.1f)
        {
            var currentPositionIsUp = false;

            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (!ballData.BallIsMovingUp && ballData.CurrentPositionVector.z < 140f && !currentPositionIsUp)
                {
                    // if we're in here, the ball is coming downwards

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
                        new HLInstruction(highPos, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = true;
                    // instructionSent: true
                    return true;
                }

                if (ballData.BallIsMovingUp && currentPositionIsUp)
                {
                    // if we're in here, the ball is moving upwards
                    // so if the ball IS moving up, and the last thing we did was hitting the ball, then we should move down again

                    var tilt = tiltController.CalculateTilt(
                        ballData.PredictedPositionVectorOnHit,
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
                        new HLInstruction(lowPos, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = false;
                    // instructionSent: false (even though we DID send a instruction.
                    // This is because we don't want to count this instruction as a bounce instruction) 
                    return false;
                }

                // instructionSent: false
                return false;
            }, duration, true);
        }

        // stable bouncing with low height
        public static IBallControlStrategy Continuous2StepBouncingLow(int duration, ITiltController tiltController)
        {
            return Continuous2StepBouncing(duration, tiltController, null, 0.05f, 0.055f, 0.06f);
        }

        public static IBallControlStrategy Balancing(float height, int duration, Vector2 target,
            ITiltController tiltController)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < float.MaxValue)
                {
                    var tilt = tiltController.CalculateTilt(
                        ballData.CurrentPositionVector,
                        new Vector2(ballData.CurrentVelocityVector.x, ballData.CurrentVelocityVector.y),
                        target,
                        ballData.CalculatedOnBounceDownwardsVelocity,
                        ballData.AirborneTime);

                    var xCorrection = Mathf.Clamp(tilt.xTilt, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(tilt.yTilt, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(height, xCorrection, yCorrection, moveTime),
                    });

                    return true;
                }

                return false;
            }, duration);
        }

        public static IBallControlStrategy GoTo(float height, float time = 0.5f)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                machineController.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(height, 0f, 0f, time),
                });
                return true;
            }, 1);
        }

        public static IBallControlStrategy GoToWhenBallOnPlate(float height, float time = 0.5f)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < 200f)
                {
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(height, 0f, 0f, time),
                    });
                    return true;
                }

                return false;
            }, 1);
        }
    }
}
