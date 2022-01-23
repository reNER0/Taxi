using System.Collections.Generic;
using UnityEngine;

public class TaxiManager : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private ClientManager _clientManager;
    [SerializeField] private Taxi _taxiPrefab;
    [SerializeField] private int _taxiCost;

    private List<Taxi> _taxiCars = new List<Taxi>(0);

    public int taxiCost => _taxiCost;


    public void BuyCar() 
    {
        if (MoneyManager.Instance.CanSpendMoney(_taxiCost)) 
        {
            MoneyManager.Instance.SpendMoney(_taxiCost);

            Taxi newTaxi = Instantiate(_taxiPrefab, transform);
            newTaxi.transform.position = _map.points[Random.Range(0, _map.points.Count)].position;
            _taxiCars.Add(newTaxi);
        }
    }
}
