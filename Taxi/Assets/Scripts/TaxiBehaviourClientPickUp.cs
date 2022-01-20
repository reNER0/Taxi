using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourClientPickUp : ITaxiBehaviour
{
    private Taxi _taxi;

    public TaxiBehaviourClientPickUp(Taxi taxi)
    {
        _taxi = taxi;
    }
    void ITaxiBehaviour.Enter()
    {
        _taxi.SetDestinationPoint(_taxi._client._startPoint);
    }

    void ITaxiBehaviour.Exit()
    {

    }

    void ITaxiBehaviour.Update()
    {
        if (_taxi.ReachedDestinationPoint()) 
        {
            _taxi.SetBehaviour(_taxi.GetBehaviour<TaxiBehaviourClientDelivery>());
        }
    }
}
