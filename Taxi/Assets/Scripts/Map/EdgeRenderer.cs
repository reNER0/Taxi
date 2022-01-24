using UnityEngine;

public class EdgeRenderer : MonoBehaviour
{
    [SerializeField] private Color _freeRoadColor;
    [SerializeField] private Color _fullRoadColor;

    private LineRenderer _renderer;
    private Edge _edge;


    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        SetTrafficInfo(Traffic.GetTrafficAtPoint(_edge.Point1.Position), Traffic.GetTrafficAtPoint(_edge.Point2.Position));
    }


    public void AssignEdge(Edge edge)
    {
        _edge = edge;

        _renderer.SetPosition(0, _edge.Point1.Position);
        _renderer.SetPosition(1, _edge.Point2.Position);
    }

    public void SetTrafficInfo(float traffic1, float traffic2)
    {
        Color startColor = _freeRoadColor * (1 - traffic1) + _fullRoadColor * traffic1;
        Color endColor = _freeRoadColor * (1 - traffic2) + _fullRoadColor * traffic2;

        _renderer.startColor = startColor;
        _renderer.endColor = endColor;
    }

}
