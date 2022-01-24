using System.Collections.Generic;
using UnityEngine;

public enum EditTypes
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
    [SerializeField] private EditTypes _editType;

    public List<Point> Points = new List<Point>(0);
    public List<Edge> Edges = new List<Edge>(0);

    public EditTypes EditType => _editType;
    public static Map Instance;


    private void Start()
    {
        Instance = this;

        DrawEdges();
        DrawPoints();
    }


    public Point GetClosestPoint(Vector3 pos)
    {
        Point closestPoint = Points[0];

        float minDist = Vector3.Distance(pos, closestPoint.Position);

        for (int i = 1; i < Points.Count; i++)
        {
            float dist = Vector3.Distance(pos, Points[i].Position);
            if (dist < minDist)
            {
                minDist = dist;
                closestPoint = Points[i];
            }
        }

        return closestPoint;
    }

    public Point GetNextPoint(Vector3 start, Vector3 destination, List<Point> excludePoints, out bool crossExcluded)
    {
        Point startPoint = GetClosestPoint(start);

        Point destinationPoint = GetClosestPoint(destination);

        List<Edge> currentEdges = new List<Edge>(0);

        foreach (Edge edge in Edges)
        {
            if (edge.Point1.Position == startPoint.Position || edge.Point2.Position == startPoint.Position)
            {
                currentEdges.Add(edge);
            }
        }

        List<Point> nextPoints = new List<Point>(0);

        foreach (Edge edge in currentEdges)
        {
            if (edge.Point1.Position == startPoint.Position)
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
            float value = currentEdges[i].Lenght() * (1 + currentEdges[i].EdgeTraffic() / 2) + Vector3.Distance(nextPoints[i].Position, destinationPoint.Position);
            if (value < minValue && !CheckIfListContainsPoints(nextPoints[i], excludePoints))
            {
                minValue = value;
                nextPoint = nextPoints[i];
            }
        }


        crossExcluded = false;

        //if nowhere to run returning starting point and warn about crossing
        if (nextPoint == null)
        {
            crossExcluded = true;
            return startPoint;
        }


        return nextPoint;
    }

    public Vector3[] GetShortestPath(Vector3 fromPosition, Vector3 fromPoint, Vector3 destination, List<Point> excludePoints)
    {
        Point lastPoint = GetClosestPoint(fromPoint);

        List<Point> excludeP = new List<Point>(excludePoints);

        List<Vector3> path = new List<Vector3>();

        path.Add(fromPosition);
        path.Add(fromPoint);

        if (lastPoint.Position != destination)
        {
            for (int i = 0; i < 50; i++)
            {
                excludeP.Add(lastPoint);

                excludeP.RemoveAt(0);

                bool updateData;
                Point nextPoint = GetNextPoint(lastPoint.Position, destination, excludeP, out updateData);

                if (updateData)
                {
                    excludeP = new List<Point>();

                    for (int j = 0; j < excludePoints.Count; j++)
                    {
                        excludeP.Add(lastPoint);
                    }
                }

                path.Add(nextPoint.Position);

                if (nextPoint.Position == destination)
                {
                    break;
                }

                lastPoint = nextPoint;
            }

        }

        return path.ToArray();
    }

    public bool CheckIfListContainsPoints(Point point, List<Point> points)
    {
        foreach (Point p in points)
        {
            if (p.Position == point.Position)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckIfEdgeContainsPoint(Edge edge, Point point)
    {
        bool pointEqualEdgesPoint1 = edge.Point1.Position == point.Position;
        bool pointEqualEdgesPoint2 = edge.Point2.Position == point.Position;

        return (pointEqualEdgesPoint1 || pointEqualEdgesPoint2);
    }

    public bool CheckIfEdgeContainsPoints(Edge edge, Point point1, Point point2)
    {
        bool edgeContainsPoint1 = CheckIfEdgeContainsPoint(edge, point1);
        bool edgeContainsPoint2 = CheckIfEdgeContainsPoint(edge, point2);

        return (edgeContainsPoint1 && edgeContainsPoint2);
    }


    public void AddPoint(Vector3 pos)
    {
        Point newPoint = new Point(pos);
        Points.Add(newPoint);
    }

    public void AddEdge(Point point1, Point point2)
    {
        foreach (Edge edge in Edges)
        {
            if (CheckIfEdgeContainsPoints(edge, point1, point2))
            {
                return;
            }
        }

        Edges.Add(new Edge(point1, point2));
    }

    public void RemovePoint(Point point)
    {
        for (int i = 0; i < Edges.Count; i++)
        {
            if (CheckIfEdgeContainsPoint(Edges[i], point))
            {
                Edges.RemoveAt(i);
                i--;
            }
        }

        Points.Remove(point);
    }

    public void RemoveEdge(Point point1, Point point2)
    {
        foreach (Edge edge in Edges)
        {
            if (CheckIfEdgeContainsPoints(edge, point1, point2))
            {
                Edges.Remove(edge);
                break;
            }
        }
    }

    public void DrawEdges()
    {
        for (int i = 0; i < Edges.Count; i++)
        {
            EdgeRenderer newRenderer = Instantiate(_edgePrefab, transform);
            newRenderer.AssignEdge(Edges[i]);
        }
    }

    public void DrawPoints()
    {
        for (int i = 0; i < Points.Count; i++)
        {
            PointRenderer newRenderer = Instantiate(_pointPrefab, transform);
            newRenderer.AssignPosition(Points[i].Position);
        }
    }

}
