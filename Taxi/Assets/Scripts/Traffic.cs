using UnityEngine;

public class Traffic
{
    public static float GetTrafficAtPoint(Vector3 point) 
    {
        float _trafficMoveSpeed = 0.1f;

        float offset = Time.time * _trafficMoveSpeed;

        return Mathf.PerlinNoise(point.x + offset,point.z + offset);
    }
}
