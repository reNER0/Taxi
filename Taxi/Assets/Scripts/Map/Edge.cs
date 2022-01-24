using System;
using UnityEngine;

[Serializable]
public class Edge
{
    public Point Point1;
    public Point Point2;


    public Edge(Point point1, Point point2)
    {
        Point1 = point1;
        Point2 = point2;
    }


    public float Lenght()
    {
        return Vector3.Distance(Point1.Position, Point2.Position);
    }

    public Vector3 Position()
    {
        return (Point1.Position + Point2.Position) / 2;
    }

    public float EdgeTraffic()
    {
        return Traffic.GetTrafficAtPoint(Position());
    }
}
