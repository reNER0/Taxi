using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private EditType _editType;

    public EditType editType => _editType;

    [HideInInspector] public List<Point> points = new List<Point>(0);
    [HideInInspector] public List<Edge> edges = new List<Edge>(0);


    public void AddPoint(Vector3 pos)
    {
        Point newPoint = new Point(pos);
        points.Add(newPoint);
    }

    public void AddEdge(Point point1, Point point2) 
    {
        foreach (Edge edge in edges) 
        {
            if (
                (edge.point1 == point1 && edge.point2 == point2)
                ||
                (edge.point1 == point2 && edge.point2 == point1)
                )
            {
                return;
            }
        }

        edges.Add(new Edge(point1, point2));
    }

    public void RemovePoint(Point point) 
    {
        for (int i = 0; i < edges.Count; i++)
        {
            if (new Vector3(Mathf.Round(edges[i].point1.position.x*10)/10, Mathf.Round(edges[i].point1.position.y * 10) / 10, Mathf.Round(edges[i].point1.position.z * 10) / 10) 
                == new Vector3(Mathf.Round(point.position.x * 10) / 10, Mathf.Round(point.position.y * 10) / 10, Mathf.Round(point.position.z * 10) / 10)
                || new Vector3(Mathf.Round(edges[i].point2.position.x * 10) / 10, Mathf.Round(edges[i].point2.position.y * 10) / 100, Mathf.Round(edges[i].point2.position.z * 10) / 10)
                == new Vector3(Mathf.Round(point.position.x * 10) / 10, Mathf.Round(point.position.y * 10) / 10, Mathf.Round(point.position.z * 10) / 10))
            {
                edges.RemoveAt(i);
                i--;
            }
        }

        points.Remove(point);
    }

    public void RemoveEdge(Point point1, Point point2)
    {
        foreach (Edge edge in edges)
        {
            if (
                (edge.point1 == point1 && edge.point2 == point2)
                ||
                (edge.point1 == point2 && edge.point2 == point1)
                )
            {
                edges.Remove(edge);
                break;
            }
        }
    }

    public void DrawEdges() 
    {
        for(int i = 0; i < edges.Count; i++) 
        {
            EdgeRenderer newRenderer = Instantiate(_edgePrefab, transform);
            newRenderer.AssignEdge(edges[i]);
        }
    }

    public void DrawPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            PointRenderer newRenderer = Instantiate(_pointPrefab, transform);
            newRenderer.AssignPosition(points[i].position);
        }
    }


    private void Start()
    {
        DrawEdges();
        DrawPoints();
    }


    public Point GetClosestPoint(Vector3 pos) 
    {
        Point closestPoint = points[0];

        float minDist = Vector3.Distance(pos, closestPoint.position);

        for (int i = 1; i < points.Count; i++) 
        {
            float dist = Vector3.Distance(pos, points[i].position);
            if (dist < minDist) 
            {
                minDist = dist;
                closestPoint = points[i];
            }
        }

        return closestPoint;
    }

    public Point GetNextPoint(Vector3 start, Vector3 destination, List<Point> excludePoints, out bool crossExcluded)
    {
        Point startPoint = GetClosestPoint(start);

        Point destinationPoint = GetClosestPoint(destination);

        List<Edge> currentEdges = new List<Edge>(0);

        foreach (Edge edge in edges)
        {
            if (edge.point1.position == startPoint.position || edge.point2.position == startPoint.position)
            {
                currentEdges.Add(edge);
            }
        }

        List<Point> nextPoints = new List<Point>(0);

        foreach (Edge edge in currentEdges)
        {
            if (edge.point1.position == startPoint.position)
            {
                nextPoints.Add(edge.point2);
            }
            else
            {
                nextPoints.Add(edge.point1);
            }
        }


        float minValue = Mathf.Infinity;
        Point nextPoint = null;

        for (int i = 0; i < currentEdges.Count; i++)
        {
            float value = currentEdges[i].Lenght()* (1 + currentEdges[i].EdgeTraffic()/2) + Vector3.Distance(nextPoints[i].position, destinationPoint.position);
            if (value < minValue && !CheckIfListContainsPoint(nextPoints[i], excludePoints))
            {
                minValue = value;
                nextPoint = nextPoints[i];
            }
        }


        crossExcluded = false;

        if (nextPoint == null)   //if nowhere to run returning starting point and warn about crossing
        {
            crossExcluded = true;
            return startPoint;
        }


        return nextPoint;
    }
    
    public Vector3[] GetShortestPath(Vector3 fromPosition, Vector3 fromPoint, Vector3 destination, List<Point> excludePoints)
    {
        Point lastPoint = GetClosestPoint(fromPoint);

        List<Point> excludeP = new List<Point>();

        for (int i = 0; i < excludePoints.Count; i++) 
        {
            excludeP.Add(excludePoints[i]);
        }

        List<Vector3> path = new List<Vector3>();

        path.Add(fromPosition);
        path.Add(fromPoint);

        if (lastPoint.position != destination)
        {
            for (int i = 0; i < 50; i++)
            {
                excludeP.Add(lastPoint);
                
                excludeP.RemoveAt(0);

                bool updateData;
                Point nextPoint = GetNextPoint(lastPoint.position, destination, excludeP, out updateData);

                if (updateData) 
                {
                    excludeP = new List<Point>();

                    for (int j = 0; j < excludePoints.Count; j++)
                    {
                        excludeP.Add(lastPoint);
                    }
                }

                path.Add(nextPoint.position);

                if (nextPoint.position == destination)
                {
                    break;
                }

                lastPoint = nextPoint;
            }

        }

        return path.ToArray();
    }

    public bool CheckIfListContainsPoint(Point point, List<Point> points) 
    {
        foreach (Point p in points) 
        {
            if (p.position == point.position) 
            {
                return true;
            }
        }

        return false;
    }

}
