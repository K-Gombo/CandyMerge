using UnityEngine;
using System.Collections.Generic;

public class PlayPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public CandyController candyController; // CandyController에 대한 참조
    public GameObject GiftBox;
    public GameObject AutoCreateBtn;

    public void OnButtonClick()
    {
        // 모든 패널 비활성화
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

        // mergeLocked가 true일 경우 false로 설정
        if (candyController.mergeLocked)
        {
            candyController.mergeLocked = false;
        }
    }
}