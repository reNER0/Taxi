using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Taxi : MonoBehaviour, IClickable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _drivenRoadMemory = 3;
    [SerializeField] private Client _clientPrefab;

    private Point _destination;

    private Point _currentPoint;
    private List<Point> _lastPoints = new List<Point>(0);

    private TaxiManager _taxiManager;
    private Map _map;

    private float _lastTime;

    private LineRenderer _renderer;

    public Client _client { get; private set; }

    private Dictionary<Type, ITaxiBehaviour> _behavioursMap;
    private ITaxiBehaviour _currentBehaviour;



    private void InitBehaviour() 
    {
        _behavioursMap = new Dictionary<Type, ITaxiBehaviour>();

        _behavioursMap[typeof(TaxiBehaviourIdle)] = new TaxiBehaviourIdle(this);
        _behavioursMap[typeof(TaxiBehaviourClientPickUp)] = new TaxiBehaviourClientPickUp(this);
        _behavioursMap[typeof(TaxiBehaviourClientDelivery)] = new TaxiBehaviourClientDelivery(this);
    }

    private void Start()
    {
        _taxiManager = GetComponentInParent<TaxiManager>();

        _map = FindObjectOfType<Map>();

        _currentPoint = _map.GetClosestPoint(transform.position);

        _destination = _currentPoint;

        _lastPoints = new List<Point>();
        while (_lastPoints.Count < _drivenRoadMemory) 
        {
            _lastPoints.Add(_currentPoint);
        }

        InitBehaviour();

        SetBehaviour(GetBehaviour<TaxiBehaviourIdle>());

        _lastTime = Time.time;

        _renderer = GetComponent<LineRenderer>();
        _renderer.enabled = false;
    }

    private void Update()
    {
        if (_currentBehaviour != null) 
        {
            _currentBehaviour.Update();
        }

        if (!ReachedDestinationPoint()) 
        {
            if (ReachedNextPoint())
            {
                _lastPoints.Add(_currentPoint);
                _lastPoints.RemoveAt(0);

                _currentPoint = _map.GetNextPoint(_currentPoint._position,_destination._position,_lastPoints);
            }
            else 
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_currentPoint._position - transform.position), _rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, _currentPoint._position, _speed * Time.deltaTime * (1 - Traffic.GetTrafficAtPoint(transform.position)));
            }
        }
    }

    public void ShowWay() 
    {
        Vector3[] positions = _map.GetShortestPath(transform.position, _currentPoint._position, _destination._position, _lastPoints);
        _renderer.positionCount = positions.Length;
        _renderer.SetPositions(positions);

        _renderer.enabled = true;
    }

    public void HideWay() 
    {
        _renderer.enabled = false;
    }

    public bool ReachedNextPoint() => transform.position == _currentPoint._position;

    public bool ReachedDestinationPoint() => transform.position == _destination._position;

    public void SetDestinationPoint(Point point) 
    {
        _destination = _map.GetClosestPoint(point._position);
    }

    public void EndClientDelivery() 
    {
        Destroy(_client.gameObject);

        float time = Time.time - _lastTime;

        _taxiManager.SetLastTime(time);
    }

    void IClickable.OnOverlayEnter()
    {

    }

    void IClickable.OnOverlayExit()
    {

    }

    void IClickable.OnClick()
    {
        _client = _taxiManager.PickUpClient();
    }

    public void SetBehaviour(ITaxiBehaviour behaviour) 
    {
        if (_currentBehaviour != null) 
        {
            _currentBehaviour.Exit();
        }

        _currentBehaviour = behaviour;
        _currentBehaviour.Enter();
    }

    public ITaxiBehaviour GetBehaviour<T>() where T : ITaxiBehaviour 
    {
        var type = typeof(T);
        return _behavioursMap[type];
    }
}
