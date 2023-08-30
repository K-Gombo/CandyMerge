using UnityEngine;
using System.Collections.Generic;

public class MixPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public CandyController candyController;
    public GameObject MixPanel;    // UpgradePanel
    public GiftBoxController giftBoxController; // GiftBoxController 인스턴스를 참조

    public void OnButtonClick()
    {   Debug.Log("OnButtonClick Called");
        // 모든 패널 비활성화
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    
        MixPanel.SetActive(true);
       
        // Merge 기능 비활성화
        candyController.mergeLocked = true;
        giftBoxController.TogglePassiveAutoCreate(false); // 패시브 자동생성 비활성화
        candyController.TogglePassiveAutoMerge(false); // 패시브 자동생성 활성화
        candyController.UpdateBoxTransforms(); 
        Transform mixBox = GameObject.FindGameObjectWithTag("MixBox").transform;
        candyController.MoveToMixBox(mixBox); // 캔디를 MixBox로 이동
        
       
        
        
        
    }
}