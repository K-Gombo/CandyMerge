using System;
using System.Collections.Generic;
using UnityEngine;


public class Currency
{
    public string currencyName;
    public int amount;

    public void Add(int value)
    {
        amount += value;
    }

    public bool Subtract(int value)
    {
        if (amount - value < 0) return false;
        amount -= value;
        return true;
    }

    public Currency(string currencyName, int amount)
    {
        this.currencyName = currencyName;
        this.amount = amount;
    }
}

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] CurrencyUI currencyUI;

    public List<Currency> currencies = new List<Currency>();

    public event Action<string, int> OnCurrencyChanged;
    


    private void Start()
    {
        if (!LoadCurrencies())
        {
            currencies.Add(new Currency("Gold", 0));
        }

        if (!LoadCurrencies())
        {
            currencies.Add(new Currency("Dia",0));
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddCurrency("Gold", 10000);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            AddCurrency("Dia", 100);
        }
        
        
    }

    public void AddCurrency(string currencyName, int value)
    {
        Currency currency = currencies.Find(c => c.currencyName == currencyName);
        if (currency != null)
        {
            currency.Add(value);
            OnCurrencyChanged?.Invoke(currencyName, currency.amount); // 이벤트 발생
        }
    }

    public bool SubtractCurrency(string currencyName, int value)
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

    public int GetCurrencyAmount(string currencyName)
    {
        Currency currency = currencies.Find(c => c.currencyName == currencyName);
        return currency?.amount ?? 0;
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