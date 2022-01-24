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
        _controller.Taxi.SetDestinationPoint(_controller.Taxi.Client.DestinationPoint);

        _controller.Taxi.Client.transform.parent = _controller.transform;
    }

    void ITaxiBehaviour.Exit()
    {
        _controller.Taxi.EndClientDelivery();
        _controller.Taxi.InitLastPoints();

        int reward = Mathf.RoundToInt(_controller.Taxi.Client.GetReward() * _controller.Taxi.clientReward);

        MoneyManager.Instance.AddMoney(reward);
    }

    void ITaxiBehaviour.Update()
    {
        _controller.Taxi.Tank.BurnFuel();

        _controller.Taxi.PathRenderer.DrawPath(_controller.Taxi.transform.position, _controller.Taxi.currentPoint, _controller.Taxi.destinationPoint, _controller.Taxi.excludePoints);

        _controller.Taxi.Client.PathRenderer.DrawPath(_controller.Taxi.Client.StartPoint, _controller.Taxi.Client.DestinationPoint);

        if (_controller.Taxi.ReachedDestinationPoint())
        {
            _controller.SetBehaviour(_controller.GetBehaviour<TaxiBehaviourIdle>());
        }
    }

}