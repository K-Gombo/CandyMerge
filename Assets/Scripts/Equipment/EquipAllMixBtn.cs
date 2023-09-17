using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipAllMixBtn : MonoBehaviour
{
    public EquipmentManager equipmentManager; // EquipmentManager의 인스턴스를 연결
    public EquipArrangeManager equipArrangeManager;
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickAllMix);
    }
    

    void OnClickAllMix()
    {
        equipmentManager.AllMergeEquipments();
        equipArrangeManager.SortByRank();
    }
    
}
