using System;
using UnityEngine;
using UnityEngine.UI;

public class CandyStatus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int level; // 캔디의 레벨을 저장하는 변수
    public Text levelText; // 레벨을 표시할 텍스트 컴포넌트
    public GameObject levelTextObject; // 레벨 텍스트 오브젝트
    public static int baseLevel = 1; // deafault 레벨 (스킬 업그레이드 시 증가)
    public int maxBaseLevel = 58;
    public string boxName;
    public int maxCandyLevel = 60;
    
    

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

    public int GetBaseLevel()
    {
        return baseLevel;
    }
    
    public int GetMaxCandyLevel()
    {
        return maxCandyLevel;
    }
    
    public void SetBaseLevel(int newBaseLevel)
    {
        if (newBaseLevel <= maxBaseLevel)
        {
            baseLevel = newBaseLevel;
        }
        else
        {
            baseLevel = maxBaseLevel;
        }
        Debug.Log("CheckAndReturnImpossibleQuests is called");
        QuestManager.instance.CheckAndReturnImpossibleQuests();  // baseLevel이 변경될 때마다 호출
    }
    
    public void SaveCandy()
    {
        Debug.Log($"boxName : " + boxName + "\tlevel : " + level);
        ES3.Save(boxName, level);
    }
    
    public void NullSaveCandy()
    {
        Debug.Log($"boxName : " + boxName + "\tlevel : " + -1);
        ES3.Save(boxName, -1);
    }

}