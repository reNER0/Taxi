using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private Client _clientPrefab;
    [SerializeField] private DeliveryTimeStats _stats;

    private float _timer;


    private void Awake()
    {
        _stats.Init();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            SpawnRandomClient();
            _timer = _stats.AverageTime();
        }
    }


    public void SpawnRandomClient() 
    {
        List<Point> availablePoints = new List<Point>(Map.Instance.Points);

        foreach (Client client in Clients()) 
        {
            availablePoints.Remove(client.StartPoint);
        }

        if (availablePoints.Count == 0) 
        {
            throw new Exception("Nowhere to spawn client!");
        }

        Client newClient = Instantiate(_clientPrefab, transform);

        newClient.StartPoint = GetRandomPoint(availablePoints.ToArray());
        availablePoints.Remove(newClient.StartPoint);
        newClient.DestinationPoint = GetRandomPoint(availablePoints.ToArray());

        newClient.transform.position = newClient.StartPoint.Position;
    }


    private Client[] Clients() 
    {
        Client[] clients = GetComponentsInChildren<Client>();

        return clients;
    }

    private Point GetRandomPoint(Point[] points)
    {
        Point randomPoint = points[UnityEngine.Random.Range(0, points.Length)];

        return randomPoint;
    }

}
