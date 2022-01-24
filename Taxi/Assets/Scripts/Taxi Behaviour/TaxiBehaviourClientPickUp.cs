
public class TaxiBehaviourClientPickUp : ITaxiBehaviour
{
    private TaxiBehaviourController _controller;


    public TaxiBehaviourClientPickUp(TaxiBehaviourController controller)
    {
        _controller = controller;
    }


    void ITaxiBehaviour.Enter()
    {
        _controller.Taxi.SetDestinationPoint(_controller.Taxi.Client.StartPoint);
    }

    void ITaxiBehaviour.Exit()
    {
        _controller.Taxi.InitLastPoints();
    }

    void ITaxiBehaviour.Update()
    {
        _controller.Taxi.Tank.BurnFuel();

        _controller.Taxi.PathRenderer.DrawPath(_controller.Taxi.transform.position, _controller.Taxi.currentPoint, _controller.Taxi.destinationPoint, _controller.Taxi.excludePoints);

        _controller.Taxi.Client.PathRenderer.DrawPath(_controller.Taxi.Client.StartPoint, _controller.Taxi.Client.DestinationPoint);

        if (_controller.Taxi.ReachedDestinationPoint()) 
        {
            _controller.SetBehaviour(_controller.GetBehaviour<TaxiBehaviourClientDelivery>());
        }
    }

}
