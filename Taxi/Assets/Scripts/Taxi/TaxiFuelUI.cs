using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class TaxiFuelUI : MonoBehaviour, IClickable
{
    [Range(0,1)]
    [SerializeField] private float _minFillAmount = 0;
    [Range(0, 1)]
    [SerializeField] private float _maxFillAmount = 1;
    [SerializeField] private Image _fillImage;

    private Taxi _taxi;
    

    private void Start()
    {
        _taxi = GetComponentInParent<Taxi>();

        HideFuel();
    }


    private float FillAmount() 
    {
        float delta = _maxFillAmount - _minFillAmount;
        float fillAmount = _minFillAmount + delta * _taxi.Tank.FuelPercent();
        return fillAmount;
    }


    private void ShowFuel() 
    {
        _fillImage.enabled = true;
    }

    private void UpdateFuel()
    {
        _fillImage.fillAmount = FillAmount();
    }

    private void HideFuel() 
    {
        _fillImage.enabled = false;
    }


    public void OnClick() { }

    public void OnOverlayEnter() 
    {
        UpdateFuel();

        ShowFuel();
    }

    public void OnOverlayExit()
    {
        HideFuel();
    }

    public void OnOverlayStay()
    {
        UpdateFuel();
    }

}
