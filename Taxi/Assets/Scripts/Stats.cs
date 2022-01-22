using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private ClientManager _clientManager;
    [SerializeField] private float _defaultTime = 30;
    [SerializeField] private int _averageMathLenght = 10;

    public static Stats Instance;

    private List<float> _averageTimesList;
    private float _timer;


    private void Awake()
    {
        Instance = this;

        InitAverageTimesList();
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0) 
        {
            _clientManager.CreateRandomClient();
            _timer = AverageTime();
        }
    }


    private void InitAverageTimesList() 
    {
        _averageTimesList = new List<float>(0);
        while (_averageTimesList.Count < _averageMathLenght) 
        {
            _averageTimesList.Add(_defaultTime);
        }
    }


    public void SetLastTime(float time) 
    {
        _averageTimesList.RemoveAt(0);
        _averageTimesList.Add(time);
    }


    private float AverageTime() 
    {
        float averageTime = 0;

        foreach (float time in _averageTimesList) 
        {
            averageTime += time;
        }
        averageTime /= _averageMathLenght;

        return averageTime / 2;
    }
}
