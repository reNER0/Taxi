using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private Client _clientPrefab;
    [SerializeField] private Stats _stats;

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
            CreateRandomClient();
            _timer = _stats.AverageTime();
        }
    }


    public void CreateRandomClient() 
    {
        Client newClient = Instantiate(_clientPrefab,transform);

        List<Point> employedPoints = new List<Point>(0);

        foreach (Client client in Clients()) 
        {
            employedPoints.Add(client.startPoint);
        }

        int recursionLimit = Map.Instance.points.Count;
        
        Point randomPoint = new Point(Vector3.zero);

        for (int i = 0; i < recursionLimit; i++) 
        {
            randomPoint = Map.Instance.points[UnityEngine.Random.Range(0, recursionLimit)];

            if (!employedPoints.Contains(randomPoint)) 
            {
                employedPoints.Add(randomPoint);

                newClient.startPoint = randomPoint;

                for (int j = 0; j < recursionLimit; j++)
                {
                    randomPoint = Map.Instance.points[UnityEngine.Random.Range(0, recursionLimit)];

                    if (!employedPoints.Contains(randomPoint))
                    {
                        newClient.destinationPoint = randomPoint;

                        newClient.transform.position = newClient.startPoint.position;

                        return;
                    }
                }
            }
        }

        Destroy(newClient.gameObject);

        throw new Exception("Reached recursion limit!");
    }


    private Client[] Clients() 
    {
        Client[] clients = GetComponentsInChildren<Client>();

        return clients;
    }

}
