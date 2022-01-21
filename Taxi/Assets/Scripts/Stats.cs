using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private ClientManager _clientManager;
    [SerializeField] private int _averageLenght = 10;
    [SerializeField] private float _defaultTime = 30;

    private List<float> _averageTimesList;
    private float _timer;

    private void Start()
    {
        InitAverageTimesList();
    }

    private void InitAverageTimesList() 
    {
        _averageTimesList = new List<float>(0);
        while (_averageTimesList.Count < _averageLenght) 
        {
            _averageTimesList.Add(_defaultTime);
        }
    }

    public void SetLastTime(float time) 
    {
        _averageTimesList.RemoveAt(0);
        _averageTimesList.Add(time);
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

    private float AverageTime() 
    {
        float averageTime = 0;

        foreach (float time in _averageTimesList) 
        {
            averageTime += time;
        }
        averageTime /= _averageLenght;

        return averageTime / 2;
    }
}
