
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
        if (_controller.Taxi.Client)
        {
            _controller.SetBehaviour(_controller.GetBehaviour<TaxiBehaviourClientPickUp>());
        }
    }

}
