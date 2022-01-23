using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourClientPickUp : ITaxiBehaviour
{
    private TaxiBehaviourController _controller;

    public TaxiBehaviourClientPickUp(TaxiBehaviourController controller)
    {
        _controller = controller;
    }
    void ITaxiBehaviour.Enter()
    {
        _controller.taxi.SetDestinationPoint(_controller.taxi._client._startPoint);
    }

    void ITaxiBehaviour.Exit()
    {
        _controller.taxi.InitLastPoints();
    }

    void ITaxiBehaviour.Update()
    {
        _controller.taxi.Tank.BurnFuel();
        _controller.taxi.ShowWay();
        _controller.taxi._client.ShowWay();

        if (_controller.taxi.ReachedDestinationPoint()) 
        {
            _controller.SetBehaviour(_controller.GetBehaviour<TaxiBehaviourClientDelivery>());
        }
    }
}
