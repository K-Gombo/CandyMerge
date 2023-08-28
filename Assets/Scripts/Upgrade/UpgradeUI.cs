using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager; // UpgradeManager 참조
    public Text luckyCreateUpCostText; // UI 텍스트 참조
    public Text createSpeedUpCostText;
    public Text removeLockedCostText;
    public Text maxCandiesUpCostText;
    public Text candyLevelUpCostText;
    public Text passiveAutoMergeSpeedUpCostText;
    public Text passiveAutoCreateSpeedUpCostText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI(); // 초기 UI 설정
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI(); // 매 프레임마다 UI 업데이트
    }

    void UpdateUI()
    {
        // 각 업그레이드의 현재 비용을 텍스트로 표시
        luckyCreateUpCostText.text = upgradeManager.currentLuckyCreateUpCost.ToString();
        createSpeedUpCostText.text = upgradeManager.currentCreateSpeedUpCost.ToString();
        removeLockedCostText.text = upgradeManager.currentRemoveLockedCost.ToString();
        maxCandiesUpCostText.text = upgradeManager.currentMaxCandiesUpCost.ToString();
        candyLevelUpCostText.text = upgradeManager.currentCandyLevelUpCost.ToString();
        passiveAutoMergeSpeedUpCostText.text = upgradeManager.currentPassiveAutoMergeSpeedUpCost.ToString();
        passiveAutoCreateSpeedUpCostText.text = upgradeManager.currentPassiveAutoCreateSpeedUpCost.ToString();
    }
}