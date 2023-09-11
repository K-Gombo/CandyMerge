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
    public Text equipLevelText;
    public Text rankLevelText;
    
    
    void Awake()
    {
        EquipArrangeManager equipArrangeManager = FindObjectOfType<EquipArrangeManager>();
        if (equipArrangeManager != null)
        {
            equipArrangeManager.AddEquipment(this);
        }
    }
    
    public void UpdateUI()
    {
       
        equipLevelText.text = "Lv. " + equipLevel;
        rankLevelText.text = rankLevel.ToString();
    }
    

}

