using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private TaxiManager _taxiManager;
    [SerializeField] private Client _clientPrefab;

    public List<Client> _clients;

    private Client _clientSelection;

    public void CreateRandomClient() 
    {
        Client newClient = Instantiate(_clientPrefab,transform);

        List<Point> employedPoints = new List<Point>(0);

        foreach (Client client in _clients) 
        {
            employedPoints.Add(client._startPoint);
        }

        int recursionLimit = _map._points.Count;
        
        Point randomPoint = new Point(Vector3.zero);

        for (int i = 0; i < recursionLimit; i++) 
        {
            randomPoint = _map._points[UnityEngine.Random.Range(0, recursionLimit)];

            if (!employedPoints.Contains(randomPoint)) 
            {
                employedPoints.Add(randomPoint);

                newClient._startPoint = randomPoint;

                for (int j = 0; j < recursionLimit; j++)
                {
                    randomPoint = _map._points[UnityEngine.Random.Range(0, recursionLimit)];

                    if (!employedPoints.Contains(randomPoint))
                    {
                        newClient._destinationPoint = randomPoint;

                        newClient.transform.position = newClient._startPoint._position;

                        _clients.Add(newClient);

                        return;
                    }
                }
            }
        }

        Destroy(newClient.gameObject);

        throw new Exception("Reached recursion limit!");
    }

    public void SelectClient(Client client) 
    {
        _clientSelection = client;
    }

    public bool LookingForTaxi() => _clientSelection;

    public Client GetCurrentClient() 
    {
        Client client = _clientSelection;
        _clientSelection = null;
        return client;
    }

    private void Update()
    {
        
    }
}
