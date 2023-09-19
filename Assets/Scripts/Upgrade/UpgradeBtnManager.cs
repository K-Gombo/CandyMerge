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
    public UpgradeUI upgradeUI;

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
        string currentGold = currencyManager.GetCurrencyAmount("Gold");

        // 초기 설정된 비용 대신 'current' 비용을 참조
        //(string.Compare(currentGold, upgradeManager.currentLuckyGoldUpCost.ToString()) >= 0);
        luckyCreateUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentLuckyCreateUpCost.ToString()) >= 0);
        createSpeedUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentCreateSpeedUpCost.ToString()) >= 0);
        maxCandiesUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentMaxCandiesUpCost.ToString()) >= 0);
        candyLevelUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentCandyLevelUpCost.ToString()) >= 0);
        passiveAutoCreateSpeedUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentPassiveAutoCreateSpeedUpCost.ToString()) >= 0);
        RemoveLokcedButton.interactable = (string.Compare(currentGold, upgradeManager.currentRemoveLockedCost.ToString()) >= 0);
        goldUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentGoldUpCost.ToString()) >= 0);
        luckyGoldUpButton.interactable = (string.Compare(currentGold, upgradeManager.currentLuckyGoldUpCost.ToString()) >= 0);
    }

    public void OnLuckyCreateUpButtonClick()
    {
        upgradeManager.LuckyCreateUp();
        DataController.instance.Upgrade_luckyCreateLevel_Save();
        upgradeUI.UpdateUI();
    }

    public void OnCreateSpeedUpButtonClick()
    {
        upgradeManager.CreateSpeedUp();
        DataController.instance.Upgrade_createSpeedLevel_Save();
        upgradeUI.UpdateUI();
    }

    public void OnMaxCandiesUpButtonClick()
    {
        upgradeManager.MaxCandiesUp();
        DataController.instance.Upgrade_maxCandiesLevel_Save();
        upgradeUI.UpdateUI();
    }

    public void OnCandyLevelUpButtonClick()
    {
        upgradeManager.CandyLevelUp();
        DataController.instance.Upgrade_candyLevel_Save();
        upgradeUI.UpdateUI();
    }
    

    public void OnPassiveAutoCreateSpeedUpButtonClick()
    {
        upgradeManager.PassiveAutoCreateSpeedUp();
        DataController.instance.Upgrade_passiveAutoCreateSpeedLevel_Save();
        upgradeUI.UpdateUI();
    }

    public void OnRemoveLockedButtonClick()
    {
        upgradeManager.RemoveLocked();
        DataController.instance.Upgrade_removeLockedLevel_Save();
        upgradeUI.UpdateUI();
    }

    public void OnGoldUpButtonClick()
    {
        upgradeManager.GoldUp();
        DataController.instance.Upgrade_goldUpLevel_Save();
        upgradeUI.UpdateUI();
    }

    public void OnLuckyGoldUpButtonClick()
    {
        upgradeManager.LuckyGoldUp();
        DataController.instance.Upgrade_luckyGoldLevel_Save();
        upgradeUI.UpdateUI();
    }
}
