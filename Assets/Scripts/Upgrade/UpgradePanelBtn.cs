using UnityEngine;
using System.Collections.Generic;

public class UpgradePanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public GameObject UpgradePanel;    // UpgradePanel
    public CandyController candyController;
    public GachaManager GachaManager;
    
    
    public void OnButtonClick()
    {
        bool hasCandiesInMixBox = GachaManager.CheckCandiesExistInMixBox();

        if (hasCandiesInMixBox)
        {
            candyController.MoveToRandomBox(); // 캔디가 있다면 원래 박스로 이동
        }
        else
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
}