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
        SetTrafficInfo(Traffic.GetTrafficAtPoint(_edge.point1.position), Traffic.GetTrafficAtPoint(_edge.point2.position));
    }


    public void AssignEdge(Edge edge)
    {
        _edge = edge;

        _renderer.SetPosition(0, _edge.point1.position);
        _renderer.SetPosition(1, _edge.point2.position);
    }

    public void SetTrafficInfo(float traffic1, float traffic2)
    {
        Color startColor = _freeRoadColor * (1 - traffic1) + _fullRoadColor * traffic1;
        Color endColor = _freeRoadColor * (1 - traffic2) + _fullRoadColor * traffic2;

        _renderer.startColor = startColor;
        _renderer.endColor = endColor;
    }

}
