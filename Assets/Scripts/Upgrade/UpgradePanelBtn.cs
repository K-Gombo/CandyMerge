using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradePanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public GameObject UpgradePanel;    // UpgradePanel
    public CandyController candyController;
    public GachaManager gachaManager;
    


    public void OnButtonClick()
    {   
        if (gachaManager.isAnimationInProgress) 
        {
            return;
        }
        
        bool hasCandiesInMixBox = gachaManager.CheckCandiesExistInMixBox();

        SoundManager.Instance.PlaySoundEffect("ButtonLight");

        GameManager.instance.DownImage.Invoke();

        UpImage();

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

    void UpImage()
    {
        GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 40);
        Color color = GetComponent<Image>().color;
        color.a = 1;
        GetComponent<Image>().color = color;
        transform.GetChild(0).gameObject.SetActive(true);
        GameManager.instance.DownImage.AddListener(DownImage);
    }

    public void DownImage()
    {
        GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 40);
        Color color = GetComponent<Image>().color;
        color.a = 0.5f;
        GetComponent<Image>().color = color;
        transform.GetChild(0).gameObject.SetActive(false);
        GameManager.instance.DownImage.RemoveListener(DownImage);
    }
}