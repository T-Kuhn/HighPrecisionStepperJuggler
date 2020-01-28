using System.Collections.Generic;
using UnityEngine;

namespace HighPrecisionStepperJuggler
{
    public class ImageProcessingInstructionSender : MonoBehaviour
    {
        [SerializeField] private UVCCameraPlugin _cameraPlugin;
        [SerializeField] private MachineController _machineController;

        [SerializeField] private bool _ballBouncing;
        
        private void Update()
        {
            var ballData = _cameraPlugin.UpdateImageProcessing();
            var distance = FOVCalculations.RadiusToDistance(ballData.Radius);
            var xDistance = FOVCalculations.PixelPositionToDistanceFromCenter(ballData.PositionX, distance);
            var yDistance = FOVCalculations.PixelPositionToDistanceFromCenter(ballData.PositionY, distance);
            
            // PD controller START
            // X tilt: -0.05 is tilting to left 
            // X tilt:  0.05 is tilting to right 
            // Y tilt: -0.05 is tilting to top
            // Y tilt:  0.05 is tilting to bottom 

            var xCorrection = -xDistance * 0.0025f;
            Mathf.Clamp(xCorrection, Constants.MinTiltAngle, Constants.MaxTiltAngle);
            
            var yCorrection = yDistance * 0.0025f;
            Mathf.Clamp(yCorrection, Constants.MinTiltAngle, Constants.MaxTiltAngle);
            
            // PD controller END

            
            if (distance < 180f && _ballBouncing)
            {
                var moveTime = 0.15f;
                _machineController.SendInstructions(new List<HLInstruction>()
                {
                    // TODO: Add tilt correction to first and second HLInstruction
                    new HLInstruction(0.02f, xCorrection, yCorrection, moveTime / 2f),
                    //new HLInstruction(0.05f, 0f, 0f, moveTime),
                    //new HLInstruction(0.02f, 0f, 0f, moveTime),
                });
            }
        }
    }
}
