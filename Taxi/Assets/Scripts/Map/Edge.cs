using System;
using UnityEngine;

[Serializable]
public class Edge
{
    public Point point1;
    public Point point2;


    public Edge(Point point1, Point point2)
    {
        this.point1 = point1;
        this.point2 = point2;
    }


    public float Lenght()
    {
        return Vector3.Distance(point1.position, point2.position);
    }

    public Vector3 Position()
    {
        return (point1.position + point2.position) / 2;
    }

    public float EdgeTraffic()
    {
        return Traffic.GetTrafficAtPoint(Position());
    }
}
