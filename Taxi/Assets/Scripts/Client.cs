using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour, IClickable
{
    public Point _startPoint;
    public Point _destinationPoint;

    private ClientManager _clientManager;

    private LineRenderer _renderer;

    private void Start()
    {
        _clientManager = GetComponentInParent<ClientManager>();

        _renderer = GetComponent<LineRenderer>();
        _renderer.enabled = false;
    }

    public void OnOverlayEnter()
    {
        _renderer.SetPositions(new Vector3[] { transform.position, _destinationPoint._position });
        _renderer.enabled = true;
    }

    public void OnOverlayExit()
    {
        _renderer.enabled = false;
    }

    public void OnClick()
    {
        _clientManager.SelectClient(this);
    }
}
