using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위한 네임스페이스

public class EquipArrangeBtnManager : MonoBehaviour
{
    public Button cookArrangeBtn; // Unity 인스펙터에서 할당
    public Button capArrangeBtn;  // Unity 인스펙터에서 할당
    public Button clothArrangeBtn; // Unity 인스펙터에서 할당
    public Button shoesArrangeBtn; // Unity 인스펙터에서 할당
    public Button AllArrangeBtn;
    public Button cookArrangeBtn2; // Unity 인스펙터에서 할당
    public Button capArrangeBtn2;  // Unity 인스펙터에서 할당
    public Button clothArrangeBtn2; // Unity 인스펙터에서 할당
    public Button shoesArrangeBtn2; // Unity 인스펙터에서 할당
    public Button AllArrangeBtn2;
    public Button RankArrangeBtn;
    public Button SlotArrangeBtn;
    public Button BackPanelBtn;
    
    public EquipArrangeManager equipArrangeManager; // Unity 인스펙터에서 할당

    // Start is called before the first frame update
    void Start()
    {
        
        cookArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByCook(); });
        capArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByCap(); });
        clothArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByCloth(); });
        shoesArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByShoes(); });
        AllArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.SortByRank(); });
        
        cookArrangeBtn2.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByCook(); });
        capArrangeBtn2.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByCap(); });
        clothArrangeBtn2.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByCloth(); });
        shoesArrangeBtn2.onClick.AddListener(delegate { equipArrangeManager.FilterAndSortByShoes(); });
        AllArrangeBtn2.onClick.AddListener(delegate { equipArrangeManager.SortByRank(); });
        
        RankArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.SortByRank(); });
        SlotArrangeBtn.onClick.AddListener(delegate { equipArrangeManager.SortBySlotType(); });
        BackPanelBtn.onClick.AddListener(delegate { equipArrangeManager.SortByRank(); });
    }
    
}