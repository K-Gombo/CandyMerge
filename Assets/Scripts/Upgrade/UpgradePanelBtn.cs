using UnityEngine;
using System.Collections.Generic;

public class UpgradePanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public GameObject UpgradePanel;    // UpgradePanel
    public CandyController candyController;
    public void OnButtonClick()
    {
        // 모든 패널 비활성화
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }

        // UpgradePanel 활성화
        UpgradePanel.SetActive(true);
        candyController.EnableDrag(false);
    }
}