using UnityEngine;

public class AnalyticBallControlTest : MonoBehaviour
{
    void Start()
    {
        // return value is in radians
        float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            var sin = vector1.x * vector2.y - vector2.x * vector1.y;
            var cos = vector1.x * vector2.x + vector1.y * vector2.y;

            return Mathf.Atan2(sin, cos);
        }

        var airborneTime = 1f;
        var velocity = new Vector2(0f, 1f);
        var position = new Vector2(0f, 0f);
        var v_i = new Vector3(
            velocity.x,
            velocity.y,
            -1f);

        // FIXME: we assume that the control target is in the center of the plate,
        //        change this code in such a way that the target can be anywhere on the plate.
        var v_o = new Vector3(
            -position.x / airborneTime,
            -position.y / airborneTime,
            1f);

        var v_c = -v_i.normalized + v_o.normalized;

        var tiltX = -AngleBetween(Vector2.up, new Vector2(v_c.x, v_c.z));
        var tiltY = AngleBetween(Vector2.up, new Vector2(v_c.y, v_c.z));
        
        Debug.Log("tiltX: " + tiltX);
        Debug.Log("tiltY: " + tiltY);
    }
}
