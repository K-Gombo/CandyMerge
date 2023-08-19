using System;
using System.Collections.Generic;
using UnityEngine;
using ES3Internal;

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
    
    [SerializeField] CandyManager candyManager; // CandyManager 참조

    // 각 레벨별로 요구되는 캔디 개수와 회수된 캔디 개수를 저장하는 딕셔너리
    private Dictionary<int, int> requiredCandyCounts = new Dictionary<int, int> { { 1, 6 }, { 3, 6 } };
    private Dictionary<int, int> collectedCandyCounts = new Dictionary<int, int> { { 1, 0 }, { 3, 0 } };

    private List<GameObject> boxes = new List<GameObject>(); // "Box" 태그를 가진 게임 오브젝트의 참조를 저장하는 리스트


    private void Start()
    {
        if (!LoadCurrencies())
        {
            currencies.Add(new Currency("Gold", 0));
        }
        
        // "Box" 태그를 가진 게임 오브젝트의 참조를 미리 저장
        boxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddCurrency("Gold", 100);
        }
        
        // H 키를 누르면 1레벨과 3레벨의 캔디를 회수
        if (Input.GetKeyDown(KeyCode.H))
        {
            CollectCandy(1); // 1레벨 캔디를 회수합니다.
            CollectCandy(3); // 3레벨 캔디를 회수합니다.

            // 두 레벨의 캔디 모두 요구 개수만큼 가져갔으면 돈을 줍니다.
            if (collectedCandyCounts[1] >= requiredCandyCounts[1] && collectedCandyCounts[3] >= requiredCandyCounts[3])
            {
                AddCurrency("Gold", 100); // 예를 들어, 100 골드를 줍니다.
                collectedCandyCounts[1] = 0; // 회수된 캔디 개수를 초기화합니다.
                collectedCandyCounts[3] = 0; // 회수된 캔디 개수를 초기화합니다.

                // 퀘스트 완료 메시지를 디버그 로그로 출력합니다.
                Debug.Log("퀘스트 완료! 100 골드 지급!");
            }
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
    
    private void CollectCandy(int level)
    {
        // 이미 요구 개수만큼 캔디를 가져갔으면 더 이상 가져가지 않습니다.
        if (collectedCandyCounts[level] >= requiredCandyCounts[level]) return;

        foreach (GameObject box in boxes)
        {
            foreach (Transform child in box.transform)
            {
                CandyStatus status = child.GetComponent<CandyStatus>();
                if (status != null && status.level == level && child.gameObject.activeInHierarchy)
                {
                    candyManager.ReturnToPool(child.gameObject);
                    collectedCandyCounts[level]++;

                    // 캔디를 가져갈 때마다 상태를 디버그 로그로 출력합니다.
                    Debug.Log($"{level}레벨 캔디 {collectedCandyCounts[level]}/{requiredCandyCounts[level]}");

                    return; // 한 개만 찾아 리턴 풀로 돌려보내므로 메서드를 종료합니다.
                }
            }
        }
    }
}