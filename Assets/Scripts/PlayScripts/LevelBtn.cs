using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public Button LevelOffBtn;
    public Button LevelOnBtn;

    private void Start()
    {
        LevelOffBtn.gameObject.SetActive(true);
        LevelOnBtn.gameObject.SetActive(false);

        LevelOffBtn.onClick.AddListener(OnLevelOffBtnClick);
        LevelOnBtn.onClick.AddListener(OnLevelOnBtnClick);
    }

    private void OnLevelOffBtnClick()
    {
        Debug.Log("LevelOffBtn 클릭됨!");
        LevelOffBtn.gameObject.SetActive(false);
        LevelOnBtn.gameObject.SetActive(true);
        ToggleLevelText(true);
    }

    private void OnLevelOnBtnClick()
    {
        Debug.Log("LevelOnBtn 클릭됨!");
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