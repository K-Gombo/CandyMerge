using System;
using UnityEngine;
using UnityEngine.UI;

public class AutoCreateBtn : MonoBehaviour
{
    public Button ACOffBtn;
    public Button ACOnBtn;
    public GiftBoxController giftBoxController; // GiftBoxController 참조

    private void Start()
    {
        ACOffBtn.gameObject.SetActive(true); // ACOffBtn을 활성화
        ACOnBtn.gameObject.SetActive(false); // ACOnBtn을 비활성화

        ACOffBtn.onClick.AddListener(OnACOffBtnClick);
        ACOnBtn.onClick.AddListener(OnACOnBtnClick);
    }

    private void OnACOffBtnClick()
    {
        Debug.Log("ACOffBtn 클릭됨!");
        ACOffBtn.gameObject.SetActive(false);
        ACOnBtn.gameObject.SetActive(true);
        giftBoxController.ToggleFastAutoCreate(true); // 빠른 자동 생성 활성화
    }

    private void OnACOnBtnClick()
    {
        Debug.Log("ACOnBtn 클릭됨!");
        ACOffBtn.gameObject.SetActive(true);
        ACOnBtn.gameObject.SetActive(false);
        giftBoxController.ToggleFastAutoCreate(false); // 빠른 자동 생성 비활성화
    }
}