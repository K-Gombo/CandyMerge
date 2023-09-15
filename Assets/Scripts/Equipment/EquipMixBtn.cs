using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가

public class EquipMixBtn : MonoBehaviour
{
    public EquipmentManager equipmentManager; // EquipmentManager의 인스턴스를 연결

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickMix);
    }
    

    void OnClickMix()
    {
        equipmentManager.UpdateRankLevelOnMerge();
    }
}