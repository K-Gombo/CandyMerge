using UnityEngine;
using System.Collections.Generic;

public class GachaPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public CandyController candyController;
    public GameObject MixPanel;    // UpgradePanel
    public GiftBoxController giftBoxController; // GiftBoxController 인스턴스를 참조
    public AutoCreateBtn autoCreateBtn;

    public void OnButtonClick()
    {   
        
        giftBoxController.TogglePassiveAutoCreate(false); // 패시브 자동생성 비활성화
        candyController.TogglePassiveAutoMerge(false); // 패시브 자동머지 비활성화
        // 모든 패널 비활성화
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    
        MixPanel.SetActive(true);
       
        // Merge 기능 비활성화
        candyController.mergeLocked = true;
        candyController.UpdateBoxTransforms(); 
        autoCreateBtn.OnACOnBtnClick();
        Transform mixBox = GameObject.FindGameObjectWithTag("MixBox").transform;
        candyController.MoveToMixBox(mixBox); // 캔디를 MixBox로 이동
        
        
        
    }
}