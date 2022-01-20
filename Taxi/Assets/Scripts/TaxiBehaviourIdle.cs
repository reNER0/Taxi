using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourIdle : ITaxiBehaviour
{
    private Taxi _taxi;

    public TaxiBehaviourIdle(Taxi taxi) 
    {
        _taxi = taxi;
    }
    void ITaxiBehaviour.Enter()
    {
        
    }

    void ITaxiBehaviour.Exit()
    {

    }

    void ITaxiBehaviour.Update()
    {
        if (_taxi._client)
        {
            _taxi.SetBehaviour(_taxi.GetBehaviour<TaxiBehaviourClientPickUp>());
        }
    }
}
