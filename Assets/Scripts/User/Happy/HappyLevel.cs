using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyLevel : MonoBehaviour
{
    public TextAsset CsvData { get; set; }

    public Dictionary<int, int> HappylevelUp = new Dictionary<int, int>();
    public Dictionary<int, int> HappylevelUpReward = new Dictionary<int, int>();

    public int CurrentLevel = 1; // 현재 레벨
    public int currentExperience = 0; // 현재 경험치

    public Text levelText; // Level Text 오브젝트
    public Text happinessText; // HappinessText 오브젝트

    private void Awake()
    {
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

        // 초기 레벨과 경험치 텍스트 설정
        UpdateLevelText();
        UpdateHappinessText();
    }

    private void UpdateLevelText()
    {
        // Level Text 오브젝트의 텍스트를 현재 레벨로 설정
        levelText.text = CurrentLevel.ToString();
    }

    private void UpdateHappinessText()
    {
        // HappylevelUp 딕셔너리에서 현재 레벨에 필요한 경험치 가져오기
        int requiredExperience = HappylevelUp[CurrentLevel + 1]; // 현재 레벨의 다음 레벨 경험치
        happinessText.text = currentExperience + "/" + requiredExperience;
    }

    // 경험치를 증가시키고 레벨업이 필요한지 확인하는 메서드
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        while (currentExperience >= HappylevelUp[CurrentLevel + 1])
        {
            LevelUp();
        }
        UpdateHappinessText();
    }

    private void LevelUp()
    {
        CurrentLevel++;
        UpdateLevelText();
        // currentExperience 초기화
        currentExperience = 0;
    }
}
