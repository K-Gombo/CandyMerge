using System;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class AutoMergeBtn : MonoBehaviour
{
    public Button AMOffBtn; // Button 타입으로 변경
    public Button AMOnBtn;

    private void Start()
    {
        AMOffBtn.gameObject.SetActive(true);
        AMOnBtn.gameObject.SetActive(false);

        // 버튼에 클릭 리스너 추가
        AMOffBtn.onClick.AddListener(OnAMOffBtnClick);
        AMOnBtn.onClick.AddListener(OnAMOnBtnClick);
    }

    // ACOffBtn 클릭 시 호출될 메서드
    private void OnAMOffBtnClick()
    {
        Debug.Log("AMOffBtn 클릭됨!");
        AMOffBtn.gameObject.SetActive(false);
        AMOnBtn.gameObject.SetActive(true);
    }

    // ACOnBtn 클릭 시 호출될 메서드
    private void OnAMOnBtnClick()
    {
        
        Debug.Log("AMOnBtn 클릭됨!");
        AMOffBtn.gameObject.SetActive(true);
        AMOnBtn.gameObject.SetActive(false);
        
    }
}