using System;
using UnityEngine;
using System.Collections.Generic;

public class EquipPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels;
    public CandyController candyController;
    public GiftBoxController giftBoxController;
    public GameObject GiftBox;
    public GameObject AutoCreateBtn;
    public MixManager mixManager;
    public GameObject EquipPanel; 
    
    
    public void OnButtonClick()
    {
        bool hasCandiesInMixBox = mixManager.CheckCandiesExistInMixBox();

        if (hasCandiesInMixBox)
        {
            candyController.MoveToRandomBox();  // 캔디가 있다면 원래 박스로 이동
        }
        else
        {
            // 캔디가 없다면 모든 패널을 비활성화
            foreach (GameObject panel in allPanels)
            {
                panel.SetActive(false);
            }
            
            if (!GiftBox.activeSelf)
            {
                GiftBox.SetActive(true);
            }

            if (!AutoCreateBtn.activeSelf)
            {
                AutoCreateBtn.SetActive(true);
            }

            if (candyController.mergeLocked)
            {
                candyController.mergeLocked = false;
            }
            
            
            giftBoxController.TogglePassiveAutoCreate(true);
            candyController.TogglePassiveAutoMerge(true);
            
        }
        EquipPanel.SetActive(true);
        
    }
}