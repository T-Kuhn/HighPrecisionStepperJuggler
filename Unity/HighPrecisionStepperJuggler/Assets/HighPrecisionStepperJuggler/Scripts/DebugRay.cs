using UnityEngine;

public class DebugRay : MonoBehaviour
{
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }
}
