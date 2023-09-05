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
    




}

