
using UnityEngine;

public interface ITiltController
{
    (float xTilt, float yTilt) CalculateTilt(
        Vector2 position,
        Vector2 velocity,
        Vector2 targetPosition,
        float calculatedOnBounceDownwardsVelocity,
        float airborneTime);
}
