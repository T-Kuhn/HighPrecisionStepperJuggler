using System.Collections.Generic;
using UnityEngine;

public sealed class BallData
{
    private float _lastFramesXDistance;
    private float _lastFramesYDistance;
    private Vector3 _lastFramePositionVector;

    private List<float> _xVelocities = new List<float>();
    private List<float> _yVelocities = new List<float>();

    public Vector3 CurrentPositionVector => _currentPositionVector;
    private Vector3 _currentPositionVector = Vector3.zero;

    public BallData() {}

    public void UpdateData(Vector3 positionVector, bool machineReadyForNextMove)
    {
        _currentPositionVector = positionVector;
        
        if (machineReadyForNextMove)
        {
            var v_x = (positionVector.x - _lastFramesXDistance) / Time.deltaTime;
            var v_y = (positionVector.y - _lastFramesYDistance) / Time.deltaTime;

            _xVelocities.Add(v_x);
            _yVelocities.Add(v_y);
        }
        
        _lastFramesXDistance = positionVector.x;
        _lastFramesYDistance = positionVector.y;

    }

    public Vector2 GetVelocityVector()
    {
        var accumulatedV_x = 0f;
        _xVelocities.ForEach(v => accumulatedV_x += v);
        var xVelocity = accumulatedV_x / _xVelocities.Count;
        
        var accumulatedV_y = 0f;
        _yVelocities.ForEach(v => accumulatedV_y += v);
        var yVelocity = accumulatedV_y / _yVelocities.Count;
        
        return new Vector2(xVelocity, yVelocity);
    }

    public void ResetVelocityAccumulation()
    {
        _xVelocities.Clear();
        _yVelocities.Clear();
    }

}
