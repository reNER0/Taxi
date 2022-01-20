using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiManager : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private Stats _stats;
    [SerializeField] private ClientManager _clientManager;
    [SerializeField] private Taxi _taxiPrefab;

    private List<Taxi> _taxiCars = new List<Taxi>(0);

    public void BuyCar() 
    {
        Taxi newTaxi = Instantiate(_taxiPrefab, transform);
        newTaxi.transform.position = _map._points[Random.Range(0, _map._points.Count)]._position;
        _taxiCars.Add(newTaxi);
    }

    private void Update()
    {
        if (_clientManager.LookingForTaxi()) 
        {

        }
    }

    public Client PickUpClient() 
    {
        return _clientManager.GetCurrentClient();
    }

    public void SetLastTime(float time) 
    {
        _stats.SetLastTime(time);
    }
}
