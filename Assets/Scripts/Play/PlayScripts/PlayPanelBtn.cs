using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels;
    public CandyController candyController;
    public GiftBoxController giftBoxController;
    public GameObject GiftBox;
    public GameObject AutoCreateBtn;
    public GachaManager GachaManager;
    public GameObject TrashCan;
    public GachaManager gachaManager;
    private void Start()
    {
        GameManager.instance.DownImage.AddListener(DownImage);
    }

    public void OnButtonClick()
    {   
        
        if (gachaManager.isAnimationInProgress) 
        {
            return;
        }
        bool hasCandiesInMixBox = GachaManager.CheckCandiesExistInMixBox();

        SoundManager.Instance.PlaySoundEffect("ButtonLight");

        GameManager.instance.DownImage.Invoke();

        UpImage();

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

            if (!TrashCan.activeSelf)
            {
                TrashCan.SetActive(true);
            }

            giftBoxController.TogglePassiveAutoCreate(true);
            candyController.EnableDrag(true);
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

    void DownImage()
    {
        GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 40);
        Color color = GetComponent<Image>().color;
        color.a = 0.5f;
        GetComponent<Image>().color = color;
        transform.GetChild(0).gameObject.SetActive(false);
        GameManager.instance.DownImage.RemoveListener(DownImage);
    }
}