using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour, IClickable
{
    private PathRenderer _pathRenderer;

    public Point startPoint;
    public Point destinationPoint;
    public PathRenderer PathRenderer => _pathRenderer;


    private void Awake()
    {
        _pathRenderer = GetComponent<PathRenderer>();
    }


    public float GetReward() 
    {
        float reward = Vector3.Distance(startPoint.position, destinationPoint.position);

        return reward;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }


    public void OnOverlayEnter()
    {
        PathRenderer.DrawPath(startPoint, destinationPoint);
    }

    public void OnOverlayExit()
    {
        if(this)  //Checking if instance exists because of bug when OnOverlayExit called after destroying Client
            PathRenderer.HidePath();
    }

    public void OnOverlayStay() { }

    public void OnClick()
    {
        SelectionController.Instance.SelectClient(this);
    }
}
