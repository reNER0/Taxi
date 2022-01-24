using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private TaxiSpawner _taxiManager;

    [SerializeField] private Text _moneyText;
    [SerializeField] private Button _buyTaxiButton;


    private void OnEnable()
    {
        _moneyManager.OnMoneyValueChanged += OnMoneyValueChanged;
    }

    private void OnDisable()
    {
        _moneyManager.OnMoneyValueChanged -= OnMoneyValueChanged;
    }


    public void OnMoneyValueChanged(int money) 
    {
        _buyTaxiButton.interactable = money >= _taxiManager.TaxiCost;

        _moneyText.text = money + "$";
    }
}
