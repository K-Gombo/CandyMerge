using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 사용하기 위해 추가

public class EquipmentStatus : MonoBehaviour
{
    public EquipmentManager equipmentManager;
    public int equipId;
    public EquipmentManager.SlotType slotType;
    public string equipName;
    public string equipExplain;
    public int[] skillIds = new int[4];
    public EquipmentManager.Rank[] skillRanks = new EquipmentManager.Rank[4];
    public EquipmentManager.Rank equipRank;
    public Image imageComponent;
    public string[] skillNames = new string[4];
    public float[] skillPoints = new float[4];
    public Image backgroundImageComponent;
    public Image levelCircleComponent;
    public Image slotImageComponent;
    public Image slotBarComponent;
    public int equipLevel = 1;
    public int maxEquipLevel; 
    public int rankLevel;
    public float goldIncrement;
    public float upgradeGoldIncrement;
    public float maxGoldIncrement;
    public float upgradetGoldCost;
    public Text equipLevelText;
    public Text rankLevelText;
    public Button EquipButton; 
    public GameObject myClone;
    public bool isOriginal = true;
    public GameObject touchLock1;
    public GameObject touchLock2;
    public GameObject check;
    public EquipmentStatus originalEquipment;
    public GameObject rankLevelSlot;
    public GameObject mixAvailable;
    void Awake()
    {
        EquipArrangeManager equipArrangeManager = FindObjectOfType<EquipArrangeManager>();
        if (equipArrangeManager != null)
        {
            equipArrangeManager.AddEquipment(this); 
        }
    }
    
    void Start()
        {
            EquipButton = GetComponent<Button>(); // Button 컴포넌트 가져오기
            EquipButton.onClick.AddListener(() => EquipmentController.instance.OnEquipmentClick(this));
            UpdateLevelUI();
        }
    
    
    public void UpdateLevelUI()
    {
        equipLevelText.text = "Lv. " + equipLevel;
        rankLevelText.text = rankLevel.ToString();
    }
    
    public void OnCloneDeleted()
    {
        if (isOriginal)
        {
            touchLock1.SetActive(false);
            touchLock2.SetActive(false);
            check.SetActive(false);
        }
    }
    

}
    


