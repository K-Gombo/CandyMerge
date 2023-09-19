using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

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
        BigInteger currentGoldAmount = BigInteger.Parse(currencyManager.GetCurrencyAmount("Gold"));

        // 초기 설정된 비용 대신 'current' 비용을 참조
        luckyCreateUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentLuckyCreateUpCost.ToString());
        createSpeedUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentCreateSpeedUpCost.ToString());
        maxCandiesUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentMaxCandiesUpCost.ToString());
        candyLevelUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentCandyLevelUpCost.ToString());
        passiveAutoCreateSpeedUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentPassiveAutoCreateSpeedUpCost.ToString());
        RemoveLokcedButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentRemoveLockedCost.ToString());
        goldUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentGoldUpCost.ToString());
        luckyGoldUpButton.interactable = currentGoldAmount >= BigInteger.Parse(upgradeManager.currentLuckyGoldUpCost.ToString());

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
