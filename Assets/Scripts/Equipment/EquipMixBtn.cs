using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button을 사용하기 위해 추가

public class EquipMixBtn : MonoBehaviour
{
    public EquipmentManager equipmentManager; // EquipmentManager의 인스턴스를 연결
    private EquipmentStatus mainEquipment; // 메인으로 사용할 EquipmentStatus
    private GameObject[] mergedEquipments; // 합성에 사용될 장비들

  
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickMix);
    }
    

   
    void OnClickMix()
    {
        // EquipmentController를 통해 모든 EquipMixBoxes가 채워져 있는지 확인
        if (!EquipmentController.instance.AreAllEquipMixBoxesFilled())
        {
            Debug.Log("모든 EquipMixBox가 채워지지 않았습니다.");
            return;
        }

        // EquipMixBoxes에서 장비의 클론을 가져와 mergedEquipments에 할당
        for (int i = 0; i < 3; i++)
        {
            mergedEquipments[i] = EquipmentController.instance.equipMixBoxes[i].GetChild(0).gameObject;
        }

        // 첫 번째 장비의 원본을 mainEquipment으로 설정
        mainEquipment = mergedEquipments[0].GetComponent<EquipmentStatus>().originalEquipment;

        // UpdateRankLevelOnMerge 호출
        equipmentManager.UpdateRankLevelOnMerge(mainEquipment, mergedEquipments);


        foreach (GameObject clone in mergedEquipments)
        {
            EquipmentStatus original = clone.GetComponent<EquipmentStatus>().originalEquipment;
            if (original != null)
            {
                // 첫 번째 원본 장비는 풀로 반환하지 않음
                if (original != mainEquipment)
                {
                    // 원본을 풀로 반환
                    equipmentManager.ReturnEquipToPool(original.gameObject);
                }

                // 원본의 상태를 복구
                original.touchLock1.SetActive(false);
                original.touchLock2.SetActive(false);
                original.check.SetActive(false);
            }

            // 클론 파괴
            Destroy(clone);
        }
    }

}