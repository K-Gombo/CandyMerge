using System;
using UnityEngine;
using UnityEngine.UI;

public class AutoCreateBtn : MonoBehaviour
{
    public Button ACOffBtn;
    public Button ACOnBtn;
    public GiftBoxController giftBoxController; // GiftBoxController 참조
    public CandyController candycontroller; // candyController 참조
    private void Start()
    {
        ACOffBtn.gameObject.SetActive(true); // ACOffBtn을 활성화
        ACOnBtn.gameObject.SetActive(false); // ACOnBtn을 비활성화

        ACOffBtn.onClick.AddListener(OnACOffBtnClick);
        ACOnBtn.onClick.AddListener(OnACOnBtnClick);
    }

    private void OnACOffBtnClick()
    {
       
        ACOffBtn.gameObject.SetActive(false);
        ACOnBtn.gameObject.SetActive(true);
        giftBoxController.ToggleFastAutoCreate(true); // 빠른 자동 생성 활성화
        candycontroller.ToggleFastAutoMerge(true); // 빠른 자동 머지 활성화
        
    }

    public void OnACOnBtnClick() {
       
        ACOffBtn.gameObject.SetActive(true);
        ACOnBtn.gameObject.SetActive(false);
        giftBoxController.ToggleFastAutoCreate(false); // 빠른 자동 생성 비활성화
        candycontroller.ToggleFastAutoMerge(false); // 빠른 자동 머지 비활성화
    }
}