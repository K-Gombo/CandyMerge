using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    public CurrencyManager currencyManager;
    public Text goldText;
    public Text diaText;

    private void Awake()
    {
        currencyManager.OnCurrencyChanged += UpdateCurrencyUI;
    }

    private void OnDestroy()
    {
        currencyManager.OnCurrencyChanged -= UpdateCurrencyUI;
    }

    private void UpdateCurrencyUI(string currencyName, string amount)
    {
        if (currencyName == "Gold")
        {
            goldText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(amount);
        }

        if (currencyName == "Dia")
        {
            diaText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(amount);
        }
        
        
        // 다른 재화들에 대한 처리도 여기서 추가할 수 있습니다.
    }
}