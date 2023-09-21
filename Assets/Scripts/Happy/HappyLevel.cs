using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyLevel : MonoBehaviour
{   
    
    public static HappyLevel instance;
    public TextAsset CsvData { get; set; }
    
    public Dictionary<int, int> HappylevelUp = new Dictionary<int, int>();
    public Dictionary<int, int> HappylevelUpReward = new Dictionary<int, int>();

    public int CurrentLevel = 1; // 현재 레벨
    public int currentExperience = 0; // 현재 경험치
    
    public int CurrentDia = 0;

    public Text levelText; // Level Text 오브젝트
    public Text happinessText; // HappinessText 오브젝트
    public Image happinessBar; // Happiness Bar를 참조하는 Image 컴포넌트
    public Text DiaText;

    private void Awake()
    {   
        instance = this;
        
        CsvData = Resources.Load<TextAsset>("LevelData");
        var csvText = CsvData.text;
        var csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 1; i < csvData.Length - 1; i++)
        {
            var line = csvData[i];
            var data = line.Split(',');
            var level = int.Parse(data[0]);
            var requireExperience = int.Parse(data[1]);
            var levelReward = int.Parse(data[2]);
            HappylevelUp[level] = requireExperience;
            HappylevelUpReward[level] = levelReward;
        }

        // 초기 레벨과 경험치,다이타, 텍스트 설정
        UpdateLevelText();
        UpdateHappinessText();
        UpdateDiaText();
    }

    public void InitUI()
    {
        UpdateLevelText();
        UpdateHappinessText();
        UpdateDiaText();
        UpdateHappinessBar();
    }

    private void UpdateLevelText()
    {
        // Level Text 오브젝트의 텍스트를 현재 레벨로 설정
        levelText.text = CurrentLevel.ToString();
    }
    
    private void UpdateDiaText()
    {
        DiaText.text = CurrentDia.ToString();
    }

    private void UpdateHappinessText()
    {
        Debug.Log("나 불렸다.");
        // HappylevelUp 딕셔너리에서 현재 레벨에 필요한 경험치 가져오기
        int requiredExperience = HappylevelUp[CurrentLevel + 1]; // 현재 레벨의 다음 레벨 경험치

        // 퍼센테이지 계산
        float percentage = ((float)currentExperience / requiredExperience) * 100.0f;

        // 텍스트로 퍼센테이지 표시
        happinessText.text = string.Format("{0:F1}%", percentage);
    }


    // 경험치를 증가시키고 레벨업이 필요한지 확인하는 메서드
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        DataController.instance.Player_Experience_Save();
        while (currentExperience >= HappylevelUp[CurrentLevel + 1])
        {
            LevelUp();
        }
        UpdateHappinessText();
    }
    
    // 레벨업 보상 메서드
    private void GiveLevelUpReward()
    {
        int reward = HappylevelUpReward[CurrentLevel]; // 현재 레벨의 보상 가져오기
        CurrentDia += reward; // 다이아몬드에 보상 더하기
        UpdateDiaText(); // 다이아몬드 텍스트 업데이트
    }

    private void LevelUp()
    {
        CurrentLevel++;
        DataController.instance.Player_Level_Save();
        UpdateLevelText();
        GiveLevelUpReward(); // 레벨업 보상 주기
        // currentExperience 초기화
        currentExperience = 0;
        DataController.instance.Player_Experience_Save();
    }
    
    public void UpdateHappinessBar()
    {
        // HappylevelUp 딕셔너리에서 현재 레벨의 다음 레벨 경험치 가져오기
        int requiredExperience = HappylevelUp[CurrentLevel + 1];

        // currentExperience와 requiredExperience의 비율을 계산
        float fillAmount = (float)currentExperience / requiredExperience;

        // 비율을 Happiness Bar의 Fill Amount로 설정
        happinessBar.fillAmount = fillAmount;
    }
}
