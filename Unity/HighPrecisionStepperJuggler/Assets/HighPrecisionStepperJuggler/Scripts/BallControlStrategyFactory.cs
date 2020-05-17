using System;
using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class BallControlStrategyFactory
    {
        public static IBallControlStrategy Bouncing(int duration, ITiltController tiltController,
            float lowPos = 0.5f, float highPos = 0.6f, float moveTime = 0.1f, Action action = null)
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
            }, duration, onStrategyExecutionStart: action);
        }

        public static IBallControlStrategy BouncingStrong(int duration, ITiltController tiltController,
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

        public static IBallControlStrategy StepBouncing_Up(ITiltController tiltController,
            Vector2? target = null, float highPos = 0.058f, float moveTime = 0.1f,
            Action action = null)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (!ballData.BallIsMovingUp && ballData.CurrentPositionVector.z < 140f )
                {
                    // if we're in here, the ball is coming downwards
                    BallControlling.MoveToHeightWithXYTiltCorrection(machineController, tiltController,
                        ballData, highPos, moveTime, target);

                    return true;
                }
                return false;
            }, 1, true, onStrategyExecutionStart: action);
        }

        public static IBallControlStrategy StepBouncing_Down(ITiltController tiltController,
            Vector2? target = null, float lowPos = 0.05f, float moveTime = 0.1f,
            Action action = null)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.BallIsMovingUp)
                {
                    // if we're in here, the ball is moving upwards
                    // so if the ball IS moving up, and the last thing we did was hitting the ball, then we should move down again
                    BallControlling.MoveToHeightWithXYTiltCorrection(machineController, tiltController,
                        ballData, lowPos, moveTime, target);

                    return true;
                }
                return false;
            }, 1, true, onStrategyExecutionStart: action);
        }

        // NOTE: Duration needs to be a multiple of 2 since both the upwards motion and the downwards motion
        //       count as 1 instructionSent
        public static IBallControlStrategy TwoStepBouncing(int duration, ITiltController tiltController,
            Vector2? target = null, float lowPos = 0.05f, float highPos = 0.058f, float moveTime = 0.1f,
            Action action = null)
        {
            var currentPositionIsUp = false;

            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (!ballData.BallIsMovingUp && ballData.CurrentPositionVector.z < 140f && !currentPositionIsUp)
                {
                    // if we're in here, the ball is coming downwards

                    BallControlling.MoveToHeightWithAlternatingXYTiltCorrection(machineController, tiltController,
                        ballData, highPos, moveTime, instructionCount, target);

                    currentPositionIsUp = true;
                    return true;
                }

                if (ballData.BallIsMovingUp && currentPositionIsUp)
                {
                    // if we're in here, the ball is moving upwards
                    // so if the ball IS moving up, and the last thing we did was hitting the ball, then we should move down again

                    BallControlling.MoveToHeightWithAlternatingXYTiltCorrection(machineController, tiltController,
                        ballData, lowPos, moveTime, instructionCount, target);

                    currentPositionIsUp = false;
                    return true;
                }

                // instructionSent: false
                return false;
            }, duration, true, onStrategyExecutionStart: action);
        }



        // stable bouncing with shallow plate (not much up down movement)
        public static IBallControlStrategy TwoStepBouncingWithShallowMovement(int duration, ITiltController tiltController,
            Vector2? target)
        {
            return TwoStepBouncing(duration, tiltController, target, 0.05f, 0.054f, 0.06f);
        }
        
        // TODO: fix this ControlStrategy. Bouncing isn't stable.
        public static IBallControlStrategy TwoStepBouncingLow(int duration,
            ITiltController tiltController, Vector2? target)
        {
            return TwoStepBouncing(duration, tiltController, target, 0.05f, 0.054f, 0.07f);
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
