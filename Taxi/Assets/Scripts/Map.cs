using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EditType 
{
    ExtrudingPoints,
    MovingPoints,
    AddingEdges,
    AddingPoints,
    DeletingPoints,
    DeletingEdges
}

public class Map : MonoBehaviour
{
    [SerializeField] private PointRenderer _pointPrefab;
    [SerializeField] private EdgeRenderer _edgePrefab;

    public EditType _editType;
    public List<Point> _points = new List<Point>(0);
    public List<Edge> _edges = new List<Edge>(0);


    public void AddPoint(Vector3 pos)
    {
        Point newPoint = new Point(pos);
        _points.Add(newPoint);
    }

    public void AddEdge(Point point1, Point point2) 
    {
        foreach (Edge edge in _edges) 
        {
            if (
                (edge.Point1 == point1 && edge.Point2 == point2)
                ||
                (edge.Point1 == point2 && edge.Point2 == point1)
                )
            {
                return;
            }
        }

        _edges.Add(new Edge(point1, point2));
    }

    public void RemoveEdge(Point point1, Point point2)
    {
        foreach (Edge edge in _edges)
        {
            if (
                (edge.Point1 == point1 && edge.Point2 == point2)
                ||
                (edge.Point1 == point2 && edge.Point2 == point1)
                )
            {
                _edges.Remove(edge);
                break;
            }
        }
    }

    public void RemovePoint(Point point) 
    {
        for (int i = 0; i < _edges.Count; i++)
        {
            if (_edges[i].Point1._position== point._position || _edges[i].Point2._position == point._position)
            {
                _edges.RemoveAt(i);
                i--;
            }
        }

        _points.Remove(point);
    }

    public Point GetClosestPoint(Vector3 pos) 
    {
        Point closestPoint = _points[0];
        float minDist = Vector3.Distance(pos, closestPoint._position);
        for (int i = 1; i < _points.Count; i++) 
        {
            float dist = Vector3.Distance(pos, _points[i]._position);
            if (dist < minDist) 
            {
                minDist = dist;
                closestPoint = _points[i];
            }
        }
        return closestPoint;
    }

    private void Start()
    {
        DrawEdges();
        DrawPoints();
    }

    public void DrawEdges() 
    {
        for(int i = 0; i < _edges.Count; i++) 
        {
            EdgeRenderer newRenderer = Instantiate(_edgePrefab, transform);
            newRenderer.AssignEdge(_edges[i]);
        }
    }

    public void DrawPoints()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            PointRenderer newRenderer = Instantiate(_pointPrefab, transform);
            newRenderer.AssignPosition(_points[i]._position);
        }
    }

    public Point GetNextPoint(Vector3 start, Vector3 destination, List<Point> excludePoints)
    {
        Point startPoint = GetClosestPoint(start);
        Point destinationPoint = GetClosestPoint(destination);

        List<Edge> currentEdges = new List<Edge>(0);

        foreach (Edge edge in _edges)
        {
            if (edge.Point1._position == startPoint._position || edge.Point2._position == startPoint._position)
            {
                currentEdges.Add(edge);
            }
        }

        List<Point> nextPoints = new List<Point>(0);

        foreach (Edge edge in currentEdges)
        {
            if (edge.Point1._position == startPoint._position)
            {
                nextPoints.Add(edge.Point2);
            }
            else
            {
                nextPoints.Add(edge.Point1);
            }
        }

        float minValue = Mathf.Infinity;
        Point nextPoint = null;

        for (int i = 0; i < currentEdges.Count; i++)
        {
            float value = currentEdges[i].Lenght()* (1 + currentEdges[i].EdgeTraffic()) + Vector3.Distance(nextPoints[i]._position, destinationPoint._position);
            if (value < minValue && !IfListContainsPoint(nextPoints[i], excludePoints))
            {
                minValue = value;
                nextPoint = nextPoints[i];
            }
        }

        return nextPoint;
    }

    public bool IfListContainsPoint(Point point, List<Point> points) 
    {
        foreach (Point p in points) 
        {
            if (p._position == point._position) 
            {
                return true;
            }
        }

        return false;
    }

    /*
    public void DrawShortestPath(Point fromPoint, Point destination, Color color, out float cost)
    {
        cost = 0;

        Point lastPoint = fromPoint;
        List<Point> lastData = new List<Point>(0);
        for (int i = 0; i < 2; i++)
        {
            lastData.Add(_lastData[i]);
        }

        Debug.DrawLine(transform.position, lastPoint._position, color);

        if (lastPoint != destination)
        {
            for (int i = 0; i < 100; i++)
            {
                lastData.Add(lastPoint);
                lastData.RemoveAt(0);

                Point nextPoint = GetNextPoint(lastPoint, lastData);

                Debug.DrawLine(lastPoint._position, nextPoint._position, color);

                if (nextPoint == destination)
                {
                    break;
                }

                cost += Vector3.Distance(lastPoint._position, nextPoint._position);

                lastPoint = nextPoint;
            }
        }
    }
    */
}
