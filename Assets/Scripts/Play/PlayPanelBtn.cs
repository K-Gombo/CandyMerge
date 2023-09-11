using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels;
    public CandyController candyController;
    public GiftBoxController giftBoxController;
    public GameObject GiftBox;
    public GameObject AutoCreateBtn;
    public GachaManager GachaManager;

    public void OnButtonClick()
    {
        bool hasCandiesInMixBox = GachaManager.CheckCandiesExistInMixBox();

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
            
            // 이하의 로직은 패널이 비활성화된 후에 실행됩니다.
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
            candyController.EnableDrag(true);
        }
    }
}