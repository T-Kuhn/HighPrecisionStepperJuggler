using System.Collections.Generic;
using UnityEngine;
using c = HighPrecisionStepperJuggler.Constants;

namespace HighPrecisionStepperJuggler
{
    public static class BallControlStrategyFactory
    {
        public static IBallControlStrategy GetBouncing()
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                var moveTime = 0.1f;
                machineController.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                    new HLInstruction(0.02f, 0f, 0f, moveTime),
                    new HLInstruction(0.05f, 0f, 0f, moveTime),
                });
                return true;
            }, 1);
        }

        public static IBallControlStrategy ContinuousBouncing(int duration)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < 150f)
                {
                    // distance away from plate:
                    var p_x = -ballData.CurrentPositionVector.x * c.k_p;
                    var p_y = ballData.CurrentPositionVector.y * c.k_p;

                    // mean velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
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

                    return true;
                }

                return false;
            }, duration);
        }

        public static IBallControlStrategy Continuous2StepBouncingWithAnalyticalController(int duration)
        {
            var currentPositionIsUp = false;
            
            // return value is in radians
            float AngleBetween(Vector2 vector1, Vector2 vector2)
            {
                var sin = vector1.x * vector2.y - vector2.x * vector1.y;  
                var cos = vector1.x * vector2.x + vector1.y * vector2.y;

                return Mathf.Atan2(sin, cos);
            }

            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (!ballData.BallIsMovingUp && ballData.CurrentPositionVector.z < 150f && !currentPositionIsUp)
                {
                    // if we're in here, the ball is coming downwards
                    var velocity = ballData.CurrentVelocityVector;
                    var v_i = new Vector3(
                        velocity.x,
                        velocity.y,
                        -ballData.CalculatedOnBounceDownwardsVelocity);

                    // FIXME: we assume that the control target is in the center of the plate,
                    //        change this code in such a way that the target can be anywhere on the plate.
                    var position = ballData.CurrentPositionVector;
                    var v_o = new Vector3(
                        -position.x / ballData.AirborneTime,
                        -position.y / ballData.AirborneTime,
                        ballData.CalculatedOnBounceDownwardsVelocity);

                    var v_c = -v_i.normalized + v_o.normalized;

                    var tiltX = -AngleBetween(Vector2.up, new Vector2(v_c.x, v_c.z).normalized) * Mathf.Rad2Deg;
                    var tiltY = AngleBetween(Vector2.up, new Vector2(v_c.y, v_c.z).normalized) * Mathf.Rad2Deg;

                    var xCorrection = Mathf.Clamp(tiltX, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(tiltY, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.058f, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = true;
                    // instructionSent: true
                    return true;
                }

                if (ballData.BallIsMovingUp && currentPositionIsUp)
                {
                    // if we're in here, the ball is moving upwards
                    // so if the ball IS moving up, and the last thing we did was hitting the ball, then we should move down again
                    var velocity = ballData.CurrentVelocityVector;
                    var v_i = new Vector3(
                        velocity.x,
                        velocity.y,
                        -ballData.CalculatedOnBounceDownwardsVelocity);

                    // FIXME: we assume that the control target is in the center of the plate,
                    //        change this code in such a way that the target can be anywhere on the plate.
                    var position = ballData.PredictedPositionVectorOnHit;
                    var v_o = new Vector3(
                        -position.x / ballData.AirborneTime,
                        -position.y / ballData.AirborneTime,
                        ballData.CalculatedOnBounceDownwardsVelocity);

                    var v_c = -v_i.normalized + v_o.normalized;

                    var tiltX = -AngleBetween(Vector2.up, new Vector2(v_c.x, v_c.z).normalized) * Mathf.Rad2Deg;
                    var tiltY = AngleBetween(Vector2.up, new Vector2(v_c.y, v_c.z).normalized) * Mathf.Rad2Deg;

                    var xCorrection = Mathf.Clamp(tiltX, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(tiltY, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.05f, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = false;
                    // instructionSent: false (even though we DID send a instruction.
                    // This is because we don't want to count this instruction as a bounce instruction) 
                    return false;
                }

                // instructionSent: false
                return false;
            }, duration);
        }

        public static IBallControlStrategy Continuous2StepBouncing(int duration)
        {
            var currentPositionIsUp = false;

            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (!ballData.BallIsMovingUp && ballData.CurrentPositionVector.z < 150f && !currentPositionIsUp)
                {
                    // if we're in here, the ball is coming downwards
                    // distance away from plate:
                    var p_x = -ballData.CurrentPositionVector.x * c.twoStep_k_p;
                    var p_y = ballData.CurrentPositionVector.y * c.twoStep_k_p;

                    // velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
                    var d_x = -velocityVector.x * c.twoStep_k_d;
                    var d_y = velocityVector.y * c.twoStep_k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.058f, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = true;
                    // instructionSent: true
                    return true;
                }

                if (ballData.BallIsMovingUp && currentPositionIsUp)
                {
                    // if we're in here, the ball is moving upwards
                    // so if the ball IS moving up, and the last thing we did was hitting the ball, then we should move down again
                    var p_x = -ballData.PredictedPositionVectorOnHit.x * c.twoStep_k_p;
                    var p_y = ballData.PredictedPositionVectorOnHit.y * c.twoStep_k_p;
                    
                    // velocity of ball:
                    var velocityVector = ballData.PredictedVelocityVectorOnHit;
                    var d_x = -velocityVector.x * c.twoStep_k_d;
                    var d_y = velocityVector.y * c.twoStep_k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.05f, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = false;
                    // instructionSent: false (even though we DID send a instruction.
                    // This is because we don't want to count this instruction as a bounce instruction) 
                    return false;
                }

                // instructionSent: false
                return false;
            }, duration);
        }

        // stable bouncing with low height
        public static IBallControlStrategy Continuous2StepBouncingLow(int duration)
        {
            var currentPositionIsUp = false;
            
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (!ballData.BallIsMovingUp && ballData.CurrentPositionVector.z < 150f && !currentPositionIsUp)
                {
                    // if we're in here, the ball is coming downwards
                    // distance away from plate:
                    var p_x = -ballData.CurrentPositionVector.x * c.twoStep_k_p;
                    var p_y = ballData.CurrentPositionVector.y * c.twoStep_k_p;

                    // velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
                    var d_x = -velocityVector.x * c.twoStep_k_d;
                    var d_y = velocityVector.y * c.twoStep_k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.06f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.055f, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = true;
                    // instructionSent: true
                    return true;
                }

                if (ballData.BallIsMovingUp && currentPositionIsUp)
                {
                    // if we're in here, the ball is moving upwards
                    // so if the ball IS moving up, and the last thing we did was hitting the ball, then we should move down again
                    var p_x = -ballData.PredictedPositionVectorOnHit.x * c.twoStep_k_p;
                    var p_y = ballData.PredictedPositionVectorOnHit.y * c.twoStep_k_p;

                    // velocity of ball:
                    var velocityVector = ballData.PredictedVelocityVectorOnHit;
                    var d_x = -velocityVector.x * c.twoStep_k_d;
                    var d_y = velocityVector.y * c.twoStep_k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.06f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.05f, xCorrection, yCorrection, moveTime),
                    });

                    currentPositionIsUp = false;
                    // instructionSent: false (even though we DID send a instruction.
                    // This is because we don't want to count this instruction as a bounce instruction) 
                    return false;

                }

                // instructionSent: false
                return false;
            }, duration);
        }

        public static IBallControlStrategy BalancingAtHeight(float height, int duration)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < float.MaxValue)
                {
                    // distance away from plate:
                    var p_x = -ballData.CurrentPositionVector.x * c.k_p;
                    var p_y = ballData.CurrentPositionVector.y * c.k_p;

                    // velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
                    var d_x = -velocityVector.x * c.k_d * 0.5f;
                    var d_y = velocityVector.y * c.k_d * 0.5f;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

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

        public static IBallControlStrategy ContinuousBouncingStrong(int duration)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < 190f)
                {
                    // distance away from plate:
                    var p_x = -ballData.CurrentPositionVector.x * c.k_p;
                    var p_y = ballData.CurrentPositionVector.y * c.k_p;

                    // velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
                    var d_x = -velocityVector.x * c.k_d;
                    var d_y = velocityVector.y * c.k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle / 2f, c.MaxTiltAngle / 2f);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle / 2f, c.MaxTiltAngle / 2f);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.08f, xCorrection, yCorrection, moveTime),
                        new HLInstruction(0.05f, 0f, 0f, moveTime),
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

        public static IBallControlStrategy HighPlateBalancingAt(Vector2 position, int duration)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < float.MaxValue)
                {
                    // distance away from plate:
                    var p_x = (position.x - ballData.CurrentPositionVector.x) * c.k_p;
                    var p_y = (position.y + ballData.CurrentPositionVector.y) * c.k_p;

                    // velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
                    var d_x = -velocityVector.x * c.k_d;
                    var d_y = velocityVector.y * c.k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.08f, xCorrection, yCorrection, moveTime),
                    });

                    return true;
                }

                return false;
            }, duration);
        }

        public static IBallControlStrategy HighPlateCircleBalancing(float radius, int duration)
        {
            return new BallControlStrategy((ballData, machineController, instructionCount) =>
            {
                if (ballData.CurrentPositionVector.z < float.MaxValue)
                {
                    var position = new Vector2(
                        Mathf.Cos(Time.realtimeSinceStartup * 2f * Mathf.PI * 0.5f) * radius,
                        Mathf.Sin(Time.realtimeSinceStartup * 2f * Mathf.PI * 0.5f) * radius);
                    // distance away from plate:
                    var p_x = (position.x - ballData.CurrentPositionVector.x) * c.k_p;
                    var p_y = (position.y + ballData.CurrentPositionVector.y) * c.k_p;

                    // velocity of ball:
                    var velocityVector = ballData.CurrentVelocityVector;
                    var d_x = -velocityVector.x * c.k_d;
                    var d_y = velocityVector.y * c.k_d;

                    var xCorrection = Mathf.Clamp(p_x + d_x, c.MinTiltAngle, c.MaxTiltAngle);
                    var yCorrection = Mathf.Clamp(p_y + d_y, c.MinTiltAngle, c.MaxTiltAngle);

                    var moveTime = 0.1f;
                    machineController.SendInstructions(new List<HLInstruction>()
                    {
                        new HLInstruction(0.08f, xCorrection, yCorrection, moveTime),
                    });

                    return true;
                }

                return false;
            }, duration);
        }
    }
}
