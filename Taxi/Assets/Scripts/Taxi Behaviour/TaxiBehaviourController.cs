using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourController : MonoBehaviour
{

    private Dictionary<Type, ITaxiBehaviour> _behavioursMap;
    private ITaxiBehaviour _currentBehaviour;

    [HideInInspector] public Taxi taxi;

    private void InitBehaviour()
    {
        _behavioursMap = new Dictionary<Type, ITaxiBehaviour>();

        _behavioursMap[typeof(TaxiBehaviourIdle)] = new TaxiBehaviourIdle(this);
        _behavioursMap[typeof(TaxiBehaviourClientPickUp)] = new TaxiBehaviourClientPickUp(this);
        _behavioursMap[typeof(TaxiBehaviourClientDelivery)] = new TaxiBehaviourClientDelivery(this);
    }

    private void Start()
    {
        taxi = GetComponent<Taxi>();

        InitBehaviour();

        SetBehaviour(GetBehaviour<TaxiBehaviourIdle>());
    }

    private void LateUpdate()
    {
        if (_currentBehaviour != null)
        {
            _currentBehaviour.Update();
        }
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
