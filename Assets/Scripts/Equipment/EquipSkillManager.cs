using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EquipSkill
{
    public int skillId;
    public string skillName;
    public float skillPoint;
    public string skillRank;
}

public class EquipSkillManager : MonoBehaviour
{
    public TextAsset CsvData { get; set; }
    public Dictionary<int, EquipSkill> skillMap = new Dictionary<int, EquipSkill>(); // skillId를 key로 하는 딕셔너리

    private void Awake()
    {
        CsvData = Resources.Load<TextAsset>("EquipSkillData");
        if (CsvData == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
            return;
        }
        var csvText = CsvData.text;
        var csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 1; i < csvData.Length; i++)
        {
            try
            {
                var line = csvData[i];
                var data = line.Split(',');
                EquipSkill equipSkill = new EquipSkill();
                equipSkill.skillId = int.Parse(data[0]);
                equipSkill.skillName = data[1];

                // '%' 제거 후 float로 변환
                string percentStr = data[2].Replace("%", "");
                float percentValue = float.Parse(percentStr); 
                equipSkill.skillPoint = percentValue;

                equipSkill.skillRank = data[3];
                skillMap.Add(equipSkill.skillId, equipSkill);
                
            }
            catch (Exception e)
            {
                Debug.LogError($"Error processing line {i}: {e.Message}");
            }
        }
    }
    
    
    
    
    public void EquipLuckyGoldUp(EquipmentStatus equipment)
    {
        float currentEquipLuckyGoldUp = RewardButton.instance.GetEquipLuckyGoldUp();
        float newEquipLuckyGoldUp = currentEquipLuckyGoldUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수

        // skillId가 6, 7, 8, 9, 10 중에 있는지 확인
        int[] targetSkillIds = { 6, 7, 8, 9, 10 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;

                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipLuckyGoldUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }

        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }

        RewardButton.instance.SetEquipLuckyGoldUp(newEquipLuckyGoldUp);
        Debug.Log($"추가 골드 두배 획득 확률 업!: {newEquipLuckyGoldUp}");
    }
    
    
    
    // public void EquipLuckyCandyLevelUp(EquipmentStatus equipment)
    // {
    //     float currentEquipLuckyGoldUp = RewardButton.instance.GetEquipLuckyGoldUp();
    //     float newEquipLuckyGoldUp = currentEquipLuckyGoldUp;
    //     bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
    //
    //     // skillId가 6, 7, 8, 9, 10 중에 있는지 확인
    //     int[] targetSkillIds = { 6, 7, 8, 9, 10 };
    //
    //     for (int i = 0; i < equipment.skillIds.Length; i++)
    //     {
    //         if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
    //         {
    //             // 해당 번호가 있음을 표시
    //             skillIdExists = true;
    //
    //             // 해당 skillId의 skillPoints를 불러와서 적용
    //             newEquipLuckyGoldUp += equipment.skillPoints[i];
    //             Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
    //         }
    //     }
    //
    //     if (!skillIdExists)  // 해당 번호가 없을 경우
    //     {
    //         Debug.Log("대상 skillId 없음.");
    //     }
    //
    //     CandyController.instance.SetEquipLuckyGoldUp(newEquipLuckyGoldUp);
    //     Debug.Log($"추가 골드 두배 획득 확률 업!: {newEquipLuckyGoldUp}");
    // }


    
    
}

