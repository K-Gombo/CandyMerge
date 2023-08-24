using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public Button LevelOffBtn;
    public Button LevelOnBtn;
    public static bool IsLevelOn = false; // 현재 레벨 표시 상태를 저장하는 정적 변수

    private void Start()
    {
        LevelOffBtn.gameObject.SetActive(!IsLevelOn);
        LevelOnBtn.gameObject.SetActive(IsLevelOn);

        LevelOffBtn.onClick.AddListener(OnLevelOffBtnClick);
        LevelOnBtn.onClick.AddListener(OnLevelOnBtnClick);
    }

    private void OnLevelOffBtnClick()
    {
        IsLevelOn = true;
        LevelOffBtn.gameObject.SetActive(false);
        LevelOnBtn.gameObject.SetActive(true);
        ToggleLevelText(true);
    }

    private void OnLevelOnBtnClick()
    {
        IsLevelOn = false;
        LevelOffBtn.gameObject.SetActive(true);
        LevelOnBtn.gameObject.SetActive(false);
        ToggleLevelText(false);
    }

    private void ToggleLevelText(bool show)
    {
        CandyStatus[] candies = FindObjectsOfType<CandyStatus>();
        foreach (CandyStatus candy in candies)
        {
            candy.ToggleLevelText(show);
        }
    }
}