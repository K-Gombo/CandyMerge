using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{   
    public CurrencyManager currencyManager;
    
    public UpgradeManager upgradeManager; // UpgradeManager 참조
    public Text luckyCreateUpCostText; // UI 텍스트 참조
    public Text createSpeedUpCostText;
    public Text removeLockedCostText;
    public Text maxCandiesUpCostText;
    public Text candyLevelUpCostText;
    public Text passiveAutoMergeSpeedUpCostText;
    public Text passiveAutoCreateSpeedUpCostText;
    public Text goldUpCostText;
    public Text luckyGoldUpCostText;
    
    
    public Text luckyCreateLevelText;
    public Text createSpeedLevelText;
    public Text removeLockedLevelText;
    public Text maxCandiesLevelText;
    public Text candyLevelUpText;
    public Text PassiveAutoMergeSpeedUpLevelText;
    public Text PassiveAutoCreateSpeedUpLevelText;
    public Text goldUpLevelText;
    public Text luckyGoldUpLevelText;
    
    
    
    // Start is called before the first frame update
    void Start()
    {  
       UpdateUI();
        
    }
   

    // Update is called once per frame
    void Update()
    {
       UpdateUI();
    }

    void UpdateUI()
    {
        // 각 업그레이드의 현재 비용 표시
        luckyCreateUpCostText.text =BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentLuckyCreateUpCost.ToString());
        createSpeedUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentCreateSpeedUpCost.ToString());
        removeLockedCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentRemoveLockedCost.ToString());
        maxCandiesUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentMaxCandiesUpCost.ToString());
        candyLevelUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentCandyLevelUpCost.ToString());
        passiveAutoMergeSpeedUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentPassiveAutoMergeSpeedUpCost.ToString());
        passiveAutoCreateSpeedUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentPassiveAutoCreateSpeedUpCost.ToString());
        goldUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentGoldUpCost.ToString());
        luckyGoldUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentLuckyGoldUpCost.ToString());
        
        // 각 업그레이드 현재 레벨 표시
        luckyCreateLevelText.text = "Lv." + upgradeManager.luckyCreateLevel;
        createSpeedLevelText.text =  "Lv." + upgradeManager.createSpeedLevel;
        removeLockedLevelText.text = "Lv." + upgradeManager.removeLockedLevel;
        maxCandiesLevelText.text =  "Lv." + upgradeManager.maxCandiesLevel;
        candyLevelUpText.text = "Lv." + upgradeManager.candyLevel;
        PassiveAutoMergeSpeedUpLevelText.text =  "Lv." + upgradeManager.passiveAutoMergeSpeedLevel;
        PassiveAutoCreateSpeedUpLevelText.text = "Lv." + upgradeManager.passiveAutoCreateSpeedLevel;
        goldUpLevelText.text = "Lv." + upgradeManager.goldUpLevel;
        luckyGoldUpLevelText.text = "Lv." + upgradeManager.ludkyGoldLevel;

    }
    
    
}