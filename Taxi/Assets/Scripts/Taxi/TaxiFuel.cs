using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TaxiFuelTank
{
    [SerializeField] private float _fuelBurningSpeed = 1;
    [SerializeField] private int _fuelCapacity = 10;

    private float _fuelQuantity;


    public void BurnFuel()
    {
        _fuelQuantity -= Time.deltaTime * _fuelBurningSpeed;
        if (_fuelQuantity <= 0)
        {
            if (MoneyManager.Instance.CanSpendMoney(_fuelCapacity))
            {
                MoneyManager.Instance.SpendMoney(_fuelCapacity);
                _fuelQuantity = _fuelCapacity;
            }
        }
    }

    public float FuelPercent()
    {
        return _fuelQuantity / _fuelCapacity;
    }
}
