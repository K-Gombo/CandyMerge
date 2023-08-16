using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    public CurrencyManager currencyManager;
    public Text goldText;

    private void Start()
    {
        currencyManager.OnCurrencyChanged += UpdateCurrencyUI;
    }

    private void OnDestroy()
    {
        currencyManager.OnCurrencyChanged -= UpdateCurrencyUI;
    }

    private void UpdateCurrencyUI(string currencyName, int amount)
    {
        if (currencyName == "Gold")
        {
            goldText.text = amount.ToString();
        }
        // 다른 재화들에 대한 처리도 여기서 추가할 수 있습니다.
    }
}