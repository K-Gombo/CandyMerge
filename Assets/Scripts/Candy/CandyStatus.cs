using UnityEngine;
using UnityEngine.UI;

public class CandyStatus : MonoBehaviour
{
    public int level; // 캔디의 레벨을 저장하는 변수
    public Text levelText; // 레벨을 표시할 텍스트 컴포넌트
    public GameObject levelTextObject; // 레벨 텍스트 오브젝트

    private void Start()
    {
        ToggleLevelText(LevelBtn.IsLevelOn); // LevelBtn 클래스의 IsLevelOn 변수를 사용하여 레벨 텍스트 오브젝트의 활성화 상태 설정
        UpdateLevelText();
    }


    private void Update()
    {
        levelText.text = level.ToString(); 
    }
    
    public void ToggleLevelText(bool show)
    {
        levelTextObject.SetActive(show); // 레벨 텍스트 오브젝트의 활성화 상태 설정
    }

    private void UpdateLevelText()
    {
        levelText.text = level.ToString(); 
    }
}