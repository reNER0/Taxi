using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    public static float GetTrafficAtPoint(Vector3 point) 
    {
        float offset = Time.time / 5;
        return Mathf.PerlinNoise(point.x + offset,point.z + offset);
    }
}
