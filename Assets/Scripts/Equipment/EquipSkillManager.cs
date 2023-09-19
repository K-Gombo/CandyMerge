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
    
    
    
    public void EquipLuckyCandyLevelUp(EquipmentStatus equipment)
    {
        float currentLuckyCandyLevelUp = CandyController.instance.GetEquipLuckyCandyLevelUp();
        float newEquipLuckyCandyLevelUp = currentLuckyCandyLevelUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
        
        int[] targetSkillIds = { 16, 17, 18, 19, 20 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;
    
                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipLuckyCandyLevelUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }
    
        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }
    
        CandyController.instance.SetEquipLuckyCandyLevelUp(newEquipLuckyCandyLevelUp);
        Debug.Log($"캔디 레벨 2단계 상승 확률 업!: {newEquipLuckyCandyLevelUp}");
    }
    
    public void EquipLuckyExperienceUp(EquipmentStatus equipment)
    {
        float currentExperienceUp = QuestManager.instance.GetEquipLuckyExperienceUp();
        float newEquipLuckyExperienceUp = currentExperienceUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
    
       
        int[] targetSkillIds = { 11, 12, 13, 14, 15 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;
    
                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipLuckyExperienceUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }
    
        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }
    
        QuestManager.instance.SetEquipLuckyExperienceUp(newEquipLuckyExperienceUp);
        Debug.Log($"퀘스트 완료 경험치 2배 확률 업!: {newEquipLuckyExperienceUp}");
    }
    
    public void EquipQuestDiaUp(EquipmentStatus equipment)
    {
        float currentQuestDiaUp = RewardButton.instance.GetEquipQuestDiaUp();
        float newEquipQuestDiaUp = currentQuestDiaUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
        
        int[] targetSkillIds = { 25, 26, 27 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;
    
                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipQuestDiaUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }
    
        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }
    
        RewardButton.instance.SetEquipQuestDiaUp(newEquipQuestDiaUp);
        Debug.Log($"다이아 퀘스트 보상 증가!: {newEquipQuestDiaUp}");
    }

    
    public void EquipLuckyDiaQuestUp(EquipmentStatus equipment)
    {   Debug.Log("EquipEquipLuckyDiaQuestUp 메서드 시작");
        Debug.Log($"메서드 시작 전 다이아 퀘스트 생성 확률: {Quest.instance.GetEquipLuckyDiaQuestUp()}");
        float currentLuckyDiaQuestUp = Quest.instance.GetEquipLuckyDiaQuestUp();
        float newEquipLuckyDiaQuestUp = currentLuckyDiaQuestUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
        
        int[] targetSkillIds = { 21, 22, 23, 24 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;
    
                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipLuckyDiaQuestUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }
    
        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }
    
        Quest.instance.SetEquipLuckyDiaQuestUp(newEquipLuckyDiaQuestUp);
        Debug.Log($"다이아 퀘스트 생성 확률 업!: {newEquipLuckyDiaQuestUp}");
        
        
    }

    public void EquipLuckyCreatKeyUp(EquipmentStatus equipment)
    {
        float currentLuckyCreatKeyUp = GiftBoxController.instance.GetEquipLuckyCreatKeyUp();
        float newEquipLuckyCreatKeyUp = currentLuckyCreatKeyUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
    
       
        int[] targetSkillIds = { 28, 29, 30, 31 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;
    
                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipLuckyCreatKeyUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }
    
        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }
    
        GiftBoxController.instance.SetEquipLuckyCreatKeyUp(newEquipLuckyCreatKeyUp);
        Debug.Log($"장비 열쇠 획득 확률 업!: {newEquipLuckyCreatKeyUp}");
    }
        
    public void EquipKeyDoubleUp(EquipmentStatus equipment)
    {
        float currentEquipKeyDoubleUp = GiftBoxController.instance.GetEquipKeyDoubleUp();
        float newEquipKeyDoubleUp = currentEquipKeyDoubleUp;
        bool skillIdExists = false;  // 해당 번호가 있는지 확인하는 변수
    
       
        int[] targetSkillIds = { 32, 33, 34, 35 };
    
        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i]))
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;
    
                // 해당 skillId의 skillPoints를 불러와서 적용
                newEquipKeyDoubleUp += equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }
    
        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }
    
        GiftBoxController.instance.SetEquipKeyDoubleUp(newEquipKeyDoubleUp);
        Debug.Log($"장비 열쇠 2개 획득 확률 업!: {newEquipKeyDoubleUp}");
    }
}

