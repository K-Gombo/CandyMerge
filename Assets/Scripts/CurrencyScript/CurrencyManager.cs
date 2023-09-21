using System;
using System.Collections.Generic;
using UnityEngine;
//using System.Numerics;
using Keiwando.BigInteger;

public class Currency
{
    public string currencyName;
    public string amount;

    public void Add(BigInteger value)
    {
        BigInteger currentAmount = new BigInteger(amount);
        currentAmount += value;
        amount = currentAmount.ToString();
    }

    public bool Subtract(BigInteger value)
    {
        BigInteger currentAmount = new BigInteger(amount);
        if (currentAmount - value < 0) return false;
        currentAmount -= value;
        amount = currentAmount.ToString();
        return true;
    }


    public Currency(string currencyName, string initialAmount)
    {
        this.currencyName = currencyName;
        this.amount = initialAmount;
    }
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [SerializeField] CurrencyUI currencyUI;

    public List<Currency> currencies = new List<Currency>();

    public event Action<string, string> OnCurrencyChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!LoadCurrencies())
        {
            currencies.Add(new Currency("Gold", "0"));
        }

        if (!LoadCurrencies())
        {
            currencies.Add(new Currency("Dia", "0"));
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("uhguygg");
            RewardMovingManager.instance.RequestMovingCurrency(5, CurrencyType.Gold, "1000");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            RewardMovingManager.instance.RequestMovingCurrency(5, CurrencyType.Dia, "100");
        }
    }

    public void AddCurrency(string currencyName, BigInteger value)
    {
        Currency currency = currencies.Find(c => c.currencyName == currencyName);
        if (currency != null)
        {
            currency.Add(value);
            OnCurrencyChanged?.Invoke(currencyName, currency.amount); // 이벤트 발생
            SaveCurrencies();
        }
    }

    public bool SubtractCurrency(string currencyName, BigInteger value)
    {
        Currency currency = currencies.Find(c => c.currencyName == currencyName);
        if (currency != null)
        {
            bool result = currency.Subtract(value);
            if (result)
            {
                OnCurrencyChanged?.Invoke(currencyName, currency.amount); // 이벤트 발생
            }
            return result;
        }
        return false;
    }

    public string GetCurrencyAmount(string currencyName)
    {
        Currency currency = currencies.Find(c => c.currencyName == currencyName);
        return currency?.amount ?? "0";
    }

    public void SaveCurrencies()
    {
        ES3.Save<List<Currency>>("currencies", currencies);
    }

    public bool LoadCurrencies()
    {
        if (ES3.KeyExists("currencies"))
        {
            currencies = ES3.Load<List<Currency>>("currencies");
            foreach (Currency currency in currencies)
            {
                OnCurrencyChanged?.Invoke(currency.currencyName, currency.amount); // 로딩 후 이벤트 발생
            }
        }
        else return false;
        return true;
    }

}
