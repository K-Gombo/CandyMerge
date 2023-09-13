using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가

public class EquipMixBtn : MonoBehaviour
{
    public EquipmentManager equipmentManager; // EquipmentManager의 인스턴스를 연결
    public EquipmentStatus mainEquipment; // 메인으로 사용할 EquipmentStatus
    public GameObject[] mergedEquipments; // 합성에 사용될 장비들

    // Start is called before the first frame update
    void Start()
    {
        // Button 컴포넌트에 이벤트 리스너를 추가
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickMix);
    }
    

    // OnClick 이벤트가 발생했을 때 호출될 메서드
    void OnClickMix()
    {
        // EquipmentManager의 UpdateRankLevelOnMerge 메서드를 호출
        equipmentManager.UpdateRankLevelOnMerge(mainEquipment, mergedEquipments);
    }
}