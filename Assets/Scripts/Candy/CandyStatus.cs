using UnityEngine;
using UnityEngine.UI;

public class CandyStatus : MonoBehaviour
{
    public int level; // 캔디의 레벨을 저장하는 변수
    public Text levelText; // 레벨을 표시할 텍스트 컴포넌트
    public GameObject levelTextObject; // 레벨 텍스트 오브젝트

    private void Start()
    {
        ToggleLevelText(false); // 레벨 텍스트 오브젝트를 비활성화
        UpdateLevelText();
    }

    private void Update()
    {
        levelText.text = level.ToString(); 
    }

    private void OnDestroy()
    {
        CandyManager.instance.CandyDestroyed();
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