using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 사용하기 위해 추가

public class EquipmentStatus : MonoBehaviour
{
    public EquipmentManager equipmentManager;
    public int equipId;
    public EquipmentManager.SlotType slotType;
    public string equipName;
    public int[] skillIds = new int[4];
    public EquipmentManager.Rank[] skillRanks = new EquipmentManager.Rank[4];
    public EquipmentManager.Rank equipRank;
    public Image imageComponent;
    public string[] skillNames = new string[4];
    public float[] skillPoints = new float[4];
    public Image backgroundImageComponent;
    public Image slotImageComponent;
    public int equipLevel;
    public int rankLevel;
    
    

    void Awake()
    {
        EquipArrangeManager equipArrangeManager = FindObjectOfType<EquipArrangeManager>();
        if (equipArrangeManager != null)
        {
            equipArrangeManager.AddEquipment(this);
        }
    }
    
    public void UpdateRankBasedOnLevel()
    {   
        
        // C의 2레벨은 C1로
        if (equipRank == EquipmentManager.Rank.C && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.C1;
            equipLevel = 1;
        }
        // C1의 1레벨은 B로
        else if (equipRank == EquipmentManager.Rank.C1 && equipLevel == 1)
        {
            equipRank = EquipmentManager.Rank.B;
            equipLevel = 1;
        }
        // B의 2레벨은 B1로
        else if (equipRank == EquipmentManager.Rank.B && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.B1;
            equipLevel = 1;
        }
        // B1의 1레벨은 A로
        else if (equipRank == EquipmentManager.Rank.B1 && equipLevel == 1)
        {
            equipRank = EquipmentManager.Rank.A;
            equipLevel = 1;
        }
        // A의 2레벨은 A1로
        else if (equipRank == EquipmentManager.Rank.A && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.A1;
            equipLevel = 1;
        }
        // A1의 2레벨은 A2로
        else if (equipRank == EquipmentManager.Rank.A1 && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.A2;
            equipLevel = 1;
        }
        // A2의 1레벨은 S로
        else if (equipRank == EquipmentManager.Rank.A2 && equipLevel == 1)
        {
            equipRank = EquipmentManager.Rank.S;
            equipLevel = 1;
        }
        // S의 2레벨은 S1로
        else if (equipRank == EquipmentManager.Rank.S && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.S1;
            equipLevel = 1;
        }
        // S1의 2레벨은 S2로
        else if (equipRank == EquipmentManager.Rank.S1 && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.S2;
            equipLevel = 1;
        }
        // S2의 1레벨은 SS로
        else if (equipRank == EquipmentManager.Rank.S2 && equipLevel == 1)
        {
            equipRank = EquipmentManager.Rank.SS;
            equipLevel = 1;
        }
        // SS의 2레벨은 SS1로
        else if (equipRank == EquipmentManager.Rank.SS && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.SS1;
            equipLevel = 1;
        }
        // SS1의 2레벨은 SS2로
        else if (equipRank == EquipmentManager.Rank.SS1 && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.SS2;
            equipLevel = 1;
        }
        // SS2의 2레벨은 SS3로
        else if (equipRank == EquipmentManager.Rank.SS2 && equipLevel == 2)
        {
            equipRank = EquipmentManager.Rank.SS3;
            equipLevel = 1;
        }
        // SS3은 이미 최고 등급이므로 더 이상 상승하지 않음
        else if (equipRank == EquipmentManager.Rank.SS3)
        {
            equipLevel = 1;  // 또는 다른 로직을 적용할 수 있습니다.
        }
    }

    
    
    
   

}

