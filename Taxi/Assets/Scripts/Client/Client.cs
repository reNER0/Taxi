using UnityEngine;

public class Client : MonoBehaviour, IClickable
{
    private PathRenderer _pathRenderer;

    public Point StartPoint;
    public Point DestinationPoint;
    public PathRenderer PathRenderer => _pathRenderer;


    private void Awake()
    {
        _pathRenderer = GetComponent<PathRenderer>();
    }


    public float GetReward()
    {
        float reward = Vector3.Distance(StartPoint.Position, DestinationPoint.Position);

        return reward;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }


    public void OnOverlayEnter()
    {
        PathRenderer.DrawPath(StartPoint, DestinationPoint);
    }

    public void OnOverlayExit()
    {
        //Checking if instance exists because of bug when OnOverlayExit called after destroying Client
        if (this)
            PathRenderer.HidePath();
    }

    public void OnOverlayStay() { }

    public void OnClick()
    {
        SelectionController.Instance.SelectClient(this);
    }
}
