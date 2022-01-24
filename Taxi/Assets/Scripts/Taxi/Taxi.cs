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
    [SerializeField] private TaxiFuelTank _tank = new TaxiFuelTank();

    private Point _currentPoint;
    private Point _destinationPoint;
    private List<Point> _lastPoints = new List<Point>(0);
    private float _lastTime;
    private Client _client;
    private PathRenderer _pathRenderer;

    public float clientReward => _clientReward;
    public Point currentPoint => _currentPoint;
    public Point destinationPoint => _destinationPoint;
    public List<Point> excludePoints => _lastPoints;
    public Client Client => _client;
    public TaxiFuelTank Tank => _tank;
    public PathRenderer PathRenderer => _pathRenderer;


    private void Start()
    {
        _pathRenderer = GetComponent<PathRenderer>();

        _currentPoint = Map.Instance.GetClosestPoint(transform.position);

        _destinationPoint = _currentPoint;

        InitLastPoints();

        _lastTime = Time.time;
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
                _currentPoint = Map.Instance.GetNextPoint(_currentPoint.Position, _destinationPoint.Position, _lastPoints, out crossedDrivenRoad);
                if (crossedDrivenRoad)
                    InitLastPoints();
            }
            else 
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_currentPoint.Position - transform.position), _rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Position, _speed * Time.deltaTime * (1 - Traffic.GetTrafficAtPoint(transform.position)));
            }
        }
    }


    public bool ReachedNextPoint() => transform.position == _currentPoint.Position;

    public bool ReachedDestinationPoint() => transform.position == _destinationPoint.Position;


    public void InitLastPoints() 
    {
        _lastPoints = new List<Point>();
        while (_lastPoints.Count < _drivenRoadMemory)
        {
            _lastPoints.Add(_currentPoint);
        }
    }

    public void SetDestinationPoint(Point point) 
    {
        _destinationPoint = Map.Instance.GetClosestPoint(point.Position);
    }

    public void PickupClient(Client client) 
    {
        _client = client;
    }


    public void EndClientDelivery() 
    {
        _client.Destroy();

        float time = Time.time - _lastTime;

        DeliveryTimeStats.Instance.SetLastTime(time);
    }

    public void OnOverlayEnter() { }

    public void OnOverlayExit() { }

    public void OnOverlayStay() { }

    public void OnClick()
    {
        SelectionController.Instance.SelectTaxi(this);
    }

}
