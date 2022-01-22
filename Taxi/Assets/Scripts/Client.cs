using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour, IClickable
{
    [SerializeField] private Transform _destinationMarker;

    public Point _startPoint;
    public Point _destinationPoint;

    private ClientManager _clientManager;

    private LineRenderer _renderer;

    private void Awake()
    {
        _clientManager = GetComponentInParent<ClientManager>();

        _renderer = GetComponent<LineRenderer>();

        HideWay();
    }

    public float GetReward() 
    {
        float reward = Vector3.Distance(_startPoint.position, _destinationPoint.position);

        return reward;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void ShowWay() 
    {
        UpdateWay();

        _destinationMarker.gameObject.SetActive(true);
        _renderer.enabled = true;
    }
    public void HideWay()
    {
        if (_destinationMarker)
        {
            _destinationMarker.gameObject.SetActive(false);
            _renderer.enabled = false;
        }
    }

    public void UpdateWay()
    {
        _renderer.SetPositions(new Vector3[] { transform.position, _destinationPoint.position });

        _destinationMarker.transform.position = _destinationPoint.position;
    }

    public void OnOverlayEnter()
    {
        ShowWay();
    }

    public void OnOverlayExit()
    {
        HideWay();
    }

    public void OnOverlayStay()
    {
        UpdateWay();
    }

    public void OnClick()
    {
        _clientManager.SelectClient(this);
    }
}
