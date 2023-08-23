using System;
using System.Collections.Generic;
using UnityEngine;

public class HappyLevel : MonoBehaviour
{
    public TextAsset CsvData { get; set; }
    public Dictionary<int, int> HappylevelUp = new Dictionary<int, int>();
    public Dictionary<int, int> HappylevelUpReward = new Dictionary<int, int>();

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentExperience { get; private set; } = 0;

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
    }

    public void AddExperience(int experience)
    {
        CurrentExperience += experience;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (CurrentExperience >= HappylevelUp[CurrentLevel])
        {
            // 레벨업
            CurrentLevel++;
            CurrentExperience -= HappylevelUp[CurrentLevel - 1];
            
            // 레벨업 보상 지급
            int reward = HappylevelUpReward[CurrentLevel - 1];
            // reward를 플레이어에게 지급하는 로직 구현

            // 다음 레벨의 필요한 경험치로 올림
            CheckLevelUp();
        }
    }
}