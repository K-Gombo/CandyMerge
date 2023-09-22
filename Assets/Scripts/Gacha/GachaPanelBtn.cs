using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GachaPanelBtn : MonoBehaviour
{
    public List<GameObject> allPanels; // 모든 패널을 관리하는 리스트
    public CandyController candyController;
    public GameObject GachaPanel;    // UpgradePanel
    public GiftBoxController giftBoxController; // GiftBoxController 인스턴스를 참조
    public AutoCreateBtn autoCreateBtn;
    public PlayPanelBtn playPanelBtn;


    public void OnButtonClick()
    {
        giftBoxController.TogglePassiveAutoCreate(false); // 패시브 자동생성 비활성화


        SoundManager.Instance.PlaySoundEffect("ButtonLight");

        GameManager.instance.DownImage.Invoke();

        UpImage();

        // 모든 패널 비활성화
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    
        GachaPanel.SetActive(true);
       
        // Merge 기능 비활성화
        candyController.mergeLocked = true;
        candyController.UpdateBoxTransforms(); 
        autoCreateBtn.OnACOnGachaClick();
        candyController.EnableDrag(true);
        Transform mixBox = GameObject.FindGameObjectWithTag("MixBox").transform;
        candyController.MoveToMixBox(mixBox); // 캔디를 MixBox로 이동
        
        if (!playPanelBtn.TrashCan.activeSelf)
        {
            playPanelBtn.TrashCan.SetActive(false);
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
        autoCreateBtn.OnACOffGachaClick();
        GameManager.instance.DownImage.RemoveListener(DownImage);
    }
}