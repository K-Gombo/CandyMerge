using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가

public class EquipMixBtn : MonoBehaviour
{
    public EquipmentManager equipmentManager; // EquipmentManager의 인스턴스를 연결
    public EquipArrangeManager equipArrangeManager;
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickMix);
    }
    

    void OnClickMix()
    {
        equipmentManager.UpdateRankLevelOnMerge();
        foreach (Transform equipMixBox in EquipmentController.instance.equipMixBoxes)
        {
            EquipmentController.instance.EquipMixAfter(equipMixBox);
        }
        EquipmentController.instance.mixLockedPanel.SetActive(false); 
        EquipmentController.instance.equipMixBtn.SetActive(false);
        equipArrangeManager.SortByRank();
    }
}