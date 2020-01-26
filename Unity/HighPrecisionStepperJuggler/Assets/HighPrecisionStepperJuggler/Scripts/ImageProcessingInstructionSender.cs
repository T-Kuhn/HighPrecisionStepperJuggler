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
            var xDistance = FOVCalculations.XPixelPositionToXDistance(ballData.PositionX, distance);
            Debug.Log(xDistance);
            
            if (distance < 180f && _ballBouncing)
            {
                var moveTime = 0.15f;
                _machineController.SendInstructions(new List<HLInstruction>()
                {
                    new HLInstruction(0.04f, 0f, 0f, moveTime),
                    new HLInstruction(0.01f, 0f, 0f, moveTime),
                });
            }

        }
    }
}
