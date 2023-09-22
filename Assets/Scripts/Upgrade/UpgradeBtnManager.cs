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
    public Button offLineRewardBonusUpButton;

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
        offLineRewardBonusUpButton.interactable = true;

        // 버튼에 대한 리스너 추가
        luckyCreateUpButton.onClick.AddListener(OnLuckyCreateUpButtonClick);
        createSpeedUpButton.onClick.AddListener(OnCreateSpeedUpButtonClick);
        maxCandiesUpButton.onClick.AddListener(OnMaxCandiesUpButtonClick);
        candyLevelUpButton.onClick.AddListener(OnCandyLevelUpButtonClick);
        passiveAutoCreateSpeedUpButton.onClick.AddListener(OnPassiveAutoCreateSpeedUpButtonClick);
        RemoveLokcedButton.onClick.AddListener(OnRemoveLockedButtonClick);
        goldUpButton.onClick.AddListener(OnGoldUpButtonClick);
        luckyGoldUpButton.onClick.AddListener(OnLuckyGoldUpButtonClick);
        offLineRewardBonusUpButton.onClick.AddListener(OnOffLineRewardBonusUpButtonClick);
        
    }


    private void Update()
{
    BigInteger currentGoldAmount = BigInteger.Parse(currencyManager.GetCurrencyAmount("Gold"));

    // Lucky Create Up
    luckyCreateUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentLuckyCreateUpCost.ToString()) || upgradeManager.luckyCreateLevel >= upgradeManager.maxLuckyCreateUpgradeLevel);

    // Create Speed Up
    createSpeedUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentCreateSpeedUpCost.ToString()) || upgradeManager.currentCreateSpeedUpCost == 30001);

    // Max Candies Up
    maxCandiesUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentMaxCandiesUpCost.ToString()) || upgradeManager.maxCandiesLevel >= upgradeManager.maxCandiesUpgradeLevel);

    // Candy Level Up
    candyLevelUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentCandyLevelUpCost.ToString()) || upgradeManager.candyLevel >= upgradeManager.maxCandyLevelUpgradeLevel);

    // Passive Auto Create Speed Up
    passiveAutoCreateSpeedUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentPassiveAutoCreateSpeedUpCost.ToString()) || upgradeManager.passiveAutoCreateSpeedLevel >= upgradeManager.maxPassiveAutoCreateSpeedUpgradeLevel);

    // Remove Locked
    RemoveLokcedButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentRemoveLockedCost.ToString()) || upgradeManager.removeLockedLevel >= upgradeManager.maxRemoveLockedUpgradeLevel);

    // Gold Up
    goldUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentGoldUpCost.ToString()) || upgradeManager.goldUpLevel >= upgradeManager.maxGoldUpUpgradeLevel);

    // Lucky Gold Up
    luckyGoldUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentLuckyGoldUpCost.ToString()) || upgradeManager.luckyGoldLevel >= upgradeManager.maxLuckyGoldUpgradeLevel);

    // OffLine Reward Bonus Up
    offLineRewardBonusUpButton.interactable = !(currentGoldAmount < BigInteger.Parse(upgradeManager.currentOffLineRewardBonusCost.ToString()) || upgradeManager.offLineRewardBonusLevel >= upgradeManager.maxOffLineRewardBonusLevel);
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
    
    public void OnOffLineRewardBonusUpButtonClick() 
    {
        upgradeManager.OffLineRewardBonusUp();
         DataController.instance.Upgrade_OffLineRewardBonusUpLevel_Save(); // 해뒀습니다잇!
        upgradeUI.UpdateUI();
    }
}
