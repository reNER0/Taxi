using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private string _savingName = "Money";

    private int Money //Needed to implement data encryption here
    {
        get
        {
            int money = PlayerPrefs.GetInt(_savingName);

            return money;
        }
        set
        {
            PlayerPrefs.SetInt(_savingName, value);
        }
    }

    public static MoneyManager Instance;


    public bool CanSpendMoney(int money) => money <= Money;


    public void AddMoney(int money)
    {
        Money += money;

        OnMoneyValueChanged.Invoke(Money);
    }

    public void SpendMoney(int money)
    {
        Money -= money;

        OnMoneyValueChanged.Invoke(Money);
    }


    public delegate void MoneyHandler(int value);

    public event MoneyHandler OnMoneyValueChanged;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Money = 1000;

        OnMoneyValueChanged?.Invoke(Money);
    }
}
