using UnityEngine;

public class PointRenderer : MonoBehaviour
{
    [SerializeField] private Color _freeRoadColor;
    [SerializeField] private Color _fullRoadColor;

    private SpriteRenderer _renderer;


    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetTrafficInfo(Traffic.GetTrafficAtPoint(transform.position));
    }


    public void AssignPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetTrafficInfo(float traffic)
    {
        Color color = _freeRoadColor * (1 - traffic) + _fullRoadColor * traffic;

        _renderer.color = color;
    }
}
