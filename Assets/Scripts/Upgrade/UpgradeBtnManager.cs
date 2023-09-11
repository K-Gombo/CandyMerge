using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{
    public Button luckyCreateUpButton;
    public Button createSpeedUpButton;
    public Button maxCandiesUpButton;
    public Button candyLevelUpButton;
    public Button passiveAutoCreateSpeedUpButton;
    public Button RemoveLokcedButton;
    public Button goldUpButton;
    public Button luckyGoldUpButton;

    public UpgradeManager upgradeManager;
    public CurrencyManager currencyManager;  // CurrencyManager 참조 추가

    private void Start()
    {
        // 초기에 모든 버튼을 활성화
        luckyCreateUpButton.interactable = true;
        createSpeedUpButton.interactable = true;
        maxCandiesUpButton.interactable = true;
        candyLevelUpButton.interactable = true;
        passiveAutoCreateSpeedUpButton.interactable = true;
        RemoveLokcedButton.interactable = true;
        goldUpButton.interactable = true;
        luckyGoldUpButton.interactable = true;
    }

    private void Update()
    {
        // 골드가 부족하면 버튼 비활성화
        int currentGold = currencyManager.GetCurrencyAmount("Gold");
    
        // 초기 설정된 비용 대신 'current' 비용을 참조
        luckyCreateUpButton.interactable = currentGold >= upgradeManager.currentLuckyCreateUpCost;
        createSpeedUpButton.interactable = currentGold >= upgradeManager.currentCreateSpeedUpCost;
        maxCandiesUpButton.interactable = currentGold >= upgradeManager.currentMaxCandiesUpCost;
        candyLevelUpButton.interactable = currentGold >= upgradeManager.currentCandyLevelUpCost;
        passiveAutoCreateSpeedUpButton.interactable = currentGold >= upgradeManager.currentPassiveAutoCreateSpeedUpCost;
        RemoveLokcedButton.interactable = currentGold >= upgradeManager.currentRemoveLockedCost;
        goldUpButton.interactable = currentGold >= upgradeManager.currentGoldUpCost;
        luckyGoldUpButton.interactable = currentGold >= upgradeManager.currentLuckyGoldUpCost;
    }

    public void OnLuckyCreateUpButtonClick()
    {
        upgradeManager.LuckyCreateUp();
    }

    public void OnCreateSpeedUpButtonClick()
    {
        upgradeManager.CreateSpeedUp();
    }

    public void OnMaxCandiesUpButtonClick()
    {
        upgradeManager.MaxCandiesUp();
    }

    public void OnCandyLevelUpButtonClick()
    {
        upgradeManager.CandyLevelUp();
    }
    

    public void OnPassiveAutoCreateSpeedUpButtonClick()
    {
        upgradeManager.PassiveAutoCreateSpeedUp();
    }

    public void OnRemoveLockedButtonClick()
    {
        upgradeManager.RemoveLocked();
    }

    public void OnGoldUpButtonClick()
    {
        upgradeManager.GoldUp();
    }

    public void OnLuckyGoldUpButtonClick()
    {
        upgradeManager.LuckyGoldUp();
    }
}
