using System;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class LevelBtn : MonoBehaviour
{
    public Button LevelOffBtn; // Button 타입으로 변경
    public Button LevelOnBtn;

    private void Start()
    {
        LevelOffBtn.gameObject.SetActive(true);
        LevelOnBtn.gameObject.SetActive(false);

        // 버튼에 클릭 리스너 추가
        LevelOffBtn.onClick.AddListener(OnAMOffBtnClick);
        LevelOnBtn.onClick.AddListener(OnAMOnBtnClick);
    }

    // ACOffBtn 클릭 시 호출될 메서드
    private void OnAMOffBtnClick()
    {
        Debug.Log("LevelOffBtn 클릭됨!");
        LevelOffBtn.gameObject.SetActive(false);
        LevelOnBtn.gameObject.SetActive(true);
    }

    // ACOnBtn 클릭 시 호출될 메서드
    private void OnAMOnBtnClick()
    {
        
        Debug.Log("LevelOnBtn 클릭됨!");
        LevelOffBtn.gameObject.SetActive(true);
        LevelOnBtn.gameObject.SetActive(false);
        
    }
}