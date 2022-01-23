using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourIdle : ITaxiBehaviour
{
    private TaxiBehaviourController _controller;

    public TaxiBehaviourIdle(TaxiBehaviourController controller) 
    {
        _controller = controller;
    }


    void ITaxiBehaviour.Enter()
    {
        
    }

    void ITaxiBehaviour.Exit()
    {

    }

    void ITaxiBehaviour.Update()
    {
        if (_controller.taxi._client)
        {
            _controller.SetBehaviour(_controller.GetBehaviour<TaxiBehaviourClientPickUp>());
        }
    }
}
