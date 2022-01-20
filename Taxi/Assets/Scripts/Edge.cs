using System;
using System.Collections;
using System.Collections.Generic;
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
        return Vector3.Distance(Point1._position, Point2._position);
    }

    public float EdgeTraffic()
    {
        return Traffic.GetTrafficAtPoint((Point1._position + Point2._position) / 2);
    }
}
