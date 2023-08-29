using UnityEngine;
using System.Collections.Generic;

public class OvenPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public GameObject OvenPanel;    // UpgradePanel

    public void OnButtonClick()
    {
        // 모든 패널 비활성화
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }

    
        OvenPanel.SetActive(true);
    }
}