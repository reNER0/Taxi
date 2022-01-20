using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourClientDelivery : ITaxiBehaviour
{
    private Taxi _taxi;

    public TaxiBehaviourClientDelivery(Taxi taxi)
    {
        _taxi = taxi;
    }
    void ITaxiBehaviour.Enter()
    {
        _taxi.SetDestinationPoint(_taxi._client._destinationPoint);

        _taxi._client.transform.parent = _taxi.transform;
    }

    void ITaxiBehaviour.Exit()
    {
        _taxi.EndClientDelivery();
    }

    void ITaxiBehaviour.Update()
    {
        _taxi._client.OnOverlayEnter();

        if (_taxi.ReachedDestinationPoint())
        {
            _taxi.SetBehaviour(_taxi.GetBehaviour<TaxiBehaviourIdle>());
        }
    }
}