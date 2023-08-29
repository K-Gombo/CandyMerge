using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{
    public Button luckyCreateUpButton;
    public Button createSpeedUpButton;
    public Button maxCandiesUpButton;
    public Button candyLevelUpButton;
    public Button passiveAutoMergeSpeedUpButton;
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
        passiveAutoMergeSpeedUpButton.interactable = true;
        passiveAutoCreateSpeedUpButton.interactable = true;
        RemoveLokcedButton.interactable = true;
        goldUpButton.interactable = true;
        luckyGoldUpButton.interactable = true;
    }

    private void Update()
    {
        // 골드가 부족하면 버튼 비활성화
        int currentGold = currencyManager.GetCurrencyAmount("Gold");
        luckyCreateUpButton.interactable = currentGold >= upgradeManager.LuckyCreateUpCost;
        createSpeedUpButton.interactable = currentGold >= upgradeManager.CreateSpeedUpCost;
        maxCandiesUpButton.interactable = currentGold >= upgradeManager.MaxCandiesUpCost;
        candyLevelUpButton.interactable = currentGold >= upgradeManager.CandyLevelUpCost;
        passiveAutoMergeSpeedUpButton.interactable = currentGold >= upgradeManager.PassiveAutoMergeSpeedUpCost;
        passiveAutoCreateSpeedUpButton.interactable = currentGold >= upgradeManager.PassiveAutoCreateSpeedUpCost;
        RemoveLokcedButton.interactable = currentGold >= upgradeManager.RemoveLockedCost;
        goldUpButton.interactable = currentGold >= upgradeManager.GoldUpCost;
        luckyGoldUpButton.interactable = currentGold >= upgradeManager.LuckyGoldUpCost;
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

    public void OnPassiveAutoMergeSpeedUpButtonClick()
    {
        upgradeManager.PassiveAutoMergeSpeedUp();
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
