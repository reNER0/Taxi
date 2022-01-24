using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField] private Transform _destinationMarker;

    private LineRenderer _renderer;
    

    private void Start()
    {
        _renderer = GetComponent<LineRenderer>();

        HidePath();
    }


    public void DrawPath(Point nearestPoint, Point destinationPoint)
    {
        Vector3[] positions = new Vector3[] { nearestPoint.Position, destinationPoint.Position };
        _renderer.positionCount = positions.Length;
        _renderer.SetPositions(positions);

        SetDestinationMarker(destinationPoint.Position);

        ShowPath();
    }

    public void DrawPath(Point nearestPoint, Point destinationPoint,List<Point> excludePoints)
    {
        Vector3[] positions = Map.Instance.GetShortestPath(nearestPoint.Position, nearestPoint.Position, destinationPoint.Position, excludePoints);
        _renderer.positionCount = positions.Length;
        _renderer.SetPositions(positions);

        SetDestinationMarker(destinationPoint.Position);

        ShowPath();
    }

    public void DrawPath(Vector3 position, Point nearestPoint, Point destinationPoint, List<Point> excludePoints)
    {
        Vector3[] positions = Map.Instance.GetShortestPath(position, nearestPoint.Position, destinationPoint.Position, excludePoints);
        _renderer.positionCount = positions.Length;
        _renderer.SetPositions(positions);

        SetDestinationMarker(destinationPoint.Position);

        ShowPath();
    }

    public void HidePath()
    {
        if (_destinationMarker)
            _destinationMarker.gameObject.SetActive(false);

        _renderer.enabled = false;
    }


    private void SetDestinationMarker(Vector3 pos) 
    {
        if (_destinationMarker)
            _destinationMarker.position = pos;
    }

    private void ShowPath()
    {
        if (_destinationMarker)
            _destinationMarker.gameObject.SetActive(true);

        _renderer.enabled = true;
    }

}
