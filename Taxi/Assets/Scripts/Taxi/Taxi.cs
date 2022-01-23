using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Taxi : MonoBehaviour, IClickable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _clientReward;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _drivenRoadMemory = 3;
    [SerializeField] private Client _clientPrefab;

    public float ClientReward => _clientReward;

    private Point _destination;

    private Point _currentPoint;
    private List<Point> _lastPoints = new List<Point>(0);

    private TaxiManager _taxiManager;
    private Map _map;

    private float _lastTime;

    private LineRenderer _renderer;

    public Client _client { get; private set; }

    [SerializeField] private TaxiFuelTank _tank = new TaxiFuelTank();

    public TaxiFuelTank Tank => _tank;

    public void InitLastPoints() 
    {
        _lastPoints = new List<Point>();
        while (_lastPoints.Count < _drivenRoadMemory)
        {
            _lastPoints.Add(_currentPoint);
        }
    }

    private void Start()
    {
        _taxiManager = GetComponentInParent<TaxiManager>();

        _map = FindObjectOfType<Map>();

        _currentPoint = _map.GetClosestPoint(transform.position);

        _destination = _currentPoint;

        InitLastPoints();

        _lastTime = Time.time;

        _renderer = GetComponent<LineRenderer>();
        _renderer.enabled = false;
    }

    private void Update()
    {
        if (!ReachedDestinationPoint()) 
        {
            if (ReachedNextPoint())
            {
                _lastPoints.Add(_currentPoint);
                _lastPoints.RemoveAt(0);

                bool crossedDrivenRoad;
                _currentPoint = _map.GetNextPoint(_currentPoint.position, _destination.position, _lastPoints, out crossedDrivenRoad);
                if (crossedDrivenRoad)
                    InitLastPoints();
            }
            else 
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_currentPoint.position - transform.position), _rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, _speed * Time.deltaTime * (1 - Traffic.GetTrafficAtPoint(transform.position)));
            }
        }

    }

    public void ShowWay() 
    {
        Vector3[] positions = _map.GetShortestPath(transform.position, _currentPoint.position, _destination.position, _lastPoints);
        _renderer.positionCount = positions.Length;
        _renderer.SetPositions(positions);

        _renderer.enabled = true;
    }

    public void HideWay() 
    {
        _renderer.enabled = false;
    }

    public bool ReachedNextPoint() => transform.position == _currentPoint.position;

    public bool ReachedDestinationPoint() => transform.position == _destination.position;

    public void SetDestinationPoint(Point point) 
    {
        _destination = _map.GetClosestPoint(point.position);
    }


    public void EndClientDelivery() 
    {
        _client.Destroy();

        float time = Time.time - _lastTime;

        Stats.Instance.SetLastTime(time);
    }

    void IClickable.OnOverlayEnter() { }

    void IClickable.OnOverlayExit() { }

    void IClickable.OnOverlayStay() { }

    public void OnClick()
    {
        _client = _taxiManager.PickUpClient();
    }
}
