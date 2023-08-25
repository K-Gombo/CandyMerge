using UnityEngine;
using UnityEngine.UI;

public class CandyStatus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int level; // 캔디의 레벨을 저장하는 변수
    public Text levelText; // 레벨을 표시할 텍스트 컴포넌트
    public GameObject levelTextObject; // 레벨 텍스트 오브젝트
    public static int baseLevel = 1; // deafault 레벨 (스킬 업그레이드 시 증가)
    public string boxName;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (level <= baseLevel)
            level = baseLevel; // 기본 레벨로 설정
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

    public void UpdateLevelText()
    {
        levelText.text = level.ToString();
    }

    // 스킬 업그레이드 메서드
    public static void UpgradeLevel()
    {
        baseLevel++; // 기본 레벨 증가
    }
}