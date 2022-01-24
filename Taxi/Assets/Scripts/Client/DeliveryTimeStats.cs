using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeliveryTimeStats
{
    [SerializeField] private float _defaultTime = 30;
    [SerializeField] private int _averageMathLenght = 10;

    private List<float> _averageTimesList;

    public static DeliveryTimeStats Instance;


    public void Init() 
    {
        Instance = this;

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


    public float AverageTime() 
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
