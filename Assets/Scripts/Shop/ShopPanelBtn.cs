using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopPanelBtn : MonoBehaviour
{
    public GameObject ShopPanel;    // UpgradePanel
    public CandyController candyController;
    private void Start()
    {
        GameManager.instance.DownImage.AddListener(DownImage);
    }



    public void OnButtonClick()
    {
        SoundManager.Instance.PlaySoundEffect("ButtonLight");

        GameManager.instance.DownImage.Invoke();

        UpImage();
        ShopPanel.SetActive(true);
        candyController.EnableDrag(false);
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