using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBackBtn : MonoBehaviour
{
    public EquipmentController equipmentController;
    public EquipmentManager equipmentManager;

    private void Start()
    {
        Button btn = this.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        // equipStatusPanel을 비활성화
        equipmentController.equipStatusPanel.SetActive(false);

        // 생성된 클론을 제거
        if (equipmentController.currentClone != null)
        {
            Destroy(equipmentController.currentClone);
            equipmentController.currentClone = null; // 클론 제거 후 null로 설정
        }
        equipmentManager.CheckMixAvailability();
    }
}