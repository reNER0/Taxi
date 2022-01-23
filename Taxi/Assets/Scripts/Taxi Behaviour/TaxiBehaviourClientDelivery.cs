using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiBehaviourClientDelivery : ITaxiBehaviour
{
    private TaxiBehaviourController _controller;

    public TaxiBehaviourClientDelivery(TaxiBehaviourController controller)
    {
        _controller = controller;
    }
    void ITaxiBehaviour.Enter()
    {
        _controller.taxi.SetDestinationPoint(_controller.taxi._client._destinationPoint);

        _controller.taxi._client.transform.parent = _controller.transform;
    }

    void ITaxiBehaviour.Exit()
    {
        _controller.taxi.EndClientDelivery();
        _controller.taxi.InitLastPoints();

        int reward = Mathf.RoundToInt(_controller.taxi._client.GetReward() * _controller.taxi.ClientReward);

        MoneyManager.Instance.AddMoney(reward);
    }

    void ITaxiBehaviour.Update()
    {
        _controller.taxi.Tank.BurnFuel();
        _controller.taxi.ShowWay();
        _controller.taxi._client.ShowWay();

        if (_controller.taxi.ReachedDestinationPoint())
        {
            _controller.SetBehaviour(_controller.GetBehaviour<TaxiBehaviourIdle>());
        }
    }
}