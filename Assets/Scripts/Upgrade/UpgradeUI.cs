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
    public Text passiveAutoCreateSpeedUpCostText;
    public Text goldUpCostText;
    public Text luckyGoldUpCostText;
    public Text offLineRewardBonusUpCostText;
    
    
    public Text luckyCreateLevelText;
    public Text createSpeedLevelText;
    public Text removeLockedLevelText;
    public Text maxCandiesLevelText;
    public Text candyLevelUpText;
    public Text PassiveAutoCreateSpeedUpLevelText;
    public Text goldUpLevelText;
    public Text luckyGoldUpLevelText;
    public Text offLineRewardBonusLevelText;
    
    

    public void UpdateUI()
    {
        // 각 업그레이드의 현재 비용 표시
        if (upgradeManager.luckyCreateLevel == upgradeManager.maxLuckyCreateUpgradeLevel)
        {
            luckyCreateUpCostText.text = "Max";
        }
        else
        {
            luckyCreateUpCostText.text =BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentLuckyCreateUpCost.ToString());
        }
        // 생성 속도 업그레이드의 현재 비용 표시
        if (upgradeManager.createSpeedLevel == upgradeManager.maxCreateSpeedUpgradeLevel)
        {
            createSpeedUpCostText.text = "Max";
        }
        else
        {
            createSpeedUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentCreateSpeedUpCost.ToString());
        }
        // Remove Locked
        if (upgradeManager.removeLockedLevel == upgradeManager.maxRemoveLockedUpgradeLevel)
        {
            removeLockedCostText.text = "Max";
        }
        else
        {
            removeLockedCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentRemoveLockedCost.ToString());
        }

// Max Candies
        if (upgradeManager.maxCandiesLevel == upgradeManager.maxCandiesUpgradeLevel)
        {
            maxCandiesUpCostText.text = "Max";
        }
        else
        {
            maxCandiesUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentMaxCandiesUpCost.ToString());
        }

// Candy Level
        if (upgradeManager.candyLevel == upgradeManager.maxCandyLevelUpgradeLevel)
        {
            candyLevelUpCostText.text = "Max";
        }
        else
        {
            candyLevelUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentCandyLevelUpCost.ToString());
        }

// Passive Auto Create Speed
        if (upgradeManager.passiveAutoCreateSpeedLevel == upgradeManager.maxPassiveAutoCreateSpeedUpgradeLevel)
        {
            passiveAutoCreateSpeedUpCostText.text = "Max";
        }
        else
        {
            passiveAutoCreateSpeedUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentPassiveAutoCreateSpeedUpCost.ToString());
        }

// Gold Up
        if (upgradeManager.goldUpLevel == upgradeManager.maxGoldUpUpgradeLevel)
        {
            goldUpCostText.text = "Max";
        }
        else
        {
            goldUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentGoldUpCost.ToString());
        }

// Lucky Gold
        if (upgradeManager.luckyGoldLevel == upgradeManager.maxLuckyGoldUpgradeLevel)
        {
            luckyGoldUpCostText.text = "Max";
        }
        else
        {
            luckyGoldUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentLuckyGoldUpCost.ToString());
        }
        
       
        
        // offLineRewardBonus
        if (upgradeManager.offLineRewardBonusLevel == upgradeManager.maxOffLineRewardBonusLevel)
        {
            offLineRewardBonusUpCostText.text = "Max";
            offLineRewardBonusLevelText.text = "Max";
        }
        else
        {
            offLineRewardBonusUpCostText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(upgradeManager.currentOffLineRewardBonusCost.ToString());
            offLineRewardBonusLevelText.text = "Lv." + upgradeManager.offLineRewardBonusLevel;
        }
        
        
       
        // 각 업그레이드 현재 레벨 표시

// Lucky Create
        if (upgradeManager.luckyCreateLevel == upgradeManager.maxLuckyCreateUpgradeLevel)
        {
            luckyCreateLevelText.text = "Max";
        }
        else
        {
            luckyCreateLevelText.text = "Lv." + upgradeManager.luckyCreateLevel;
        }

// Create Speed
        if (upgradeManager.createSpeedLevel == upgradeManager.maxCreateSpeedUpgradeLevel)
        {
            createSpeedLevelText.text = "Max";
        }
        else
        {
            createSpeedLevelText.text = "Lv." + upgradeManager.createSpeedLevel;
        }

// Remove Locked
        if (upgradeManager.removeLockedLevel == upgradeManager.maxRemoveLockedUpgradeLevel)
        {
            removeLockedLevelText.text = "Max";
        }
        else
        {
            removeLockedLevelText.text = "Lv." + upgradeManager.removeLockedLevel;
        }

// Max Candies
        if (upgradeManager.maxCandiesLevel == upgradeManager.maxCandiesUpgradeLevel)
        {
            maxCandiesLevelText.text = "Max";
        }
        else
        {
            maxCandiesLevelText.text = "Lv." + upgradeManager.maxCandiesLevel;
        }

// Candy Level
        if (upgradeManager.candyLevel == upgradeManager.maxCandyLevelUpgradeLevel)
        {
            candyLevelUpText.text = "Max";
        }
        else
        {
            candyLevelUpText.text = "Lv." + upgradeManager.candyLevel;
        }

// Passive Auto Create Speed
        if (upgradeManager.passiveAutoCreateSpeedLevel == upgradeManager.maxPassiveAutoCreateSpeedUpgradeLevel)
        {
            PassiveAutoCreateSpeedUpLevelText.text = "Max";
        }
        else
        {
            PassiveAutoCreateSpeedUpLevelText.text = "Lv." + upgradeManager.passiveAutoCreateSpeedLevel;
        }

// Gold Up
        if (upgradeManager.goldUpLevel == upgradeManager.maxGoldUpUpgradeLevel)
        {
            goldUpLevelText.text = "Max";
        }
        else
        {
            goldUpLevelText.text = "Lv." + upgradeManager.goldUpLevel;
        }

// Lucky Gold
        if (upgradeManager.luckyGoldLevel == upgradeManager.maxLuckyGoldUpgradeLevel)
        {
            luckyGoldUpLevelText.text = "Max";
        }
        else
        {
            luckyGoldUpLevelText.text = "Lv." + upgradeManager.luckyGoldLevel;
        }


    }
    
}