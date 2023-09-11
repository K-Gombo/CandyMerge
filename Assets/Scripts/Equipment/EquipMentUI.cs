using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipMentUI : MonoBehaviour
{
    public EquipmentStatus equipmentStatus;
    public Text equipLevelText;
    public Text rankLevelText;
    
    // UI 업데이트 메서드
    public void UpdateUI()
    {
        // 레벨과 랭크를 표시하는 텍스트 오브젝트가 있다고 가정
        equipLevelText.text = "Level: " + equipmentStatus.equipLevel;
        rankLevelText.text = "Rank: " + equipmentStatus.rankLevel;
    }
}
