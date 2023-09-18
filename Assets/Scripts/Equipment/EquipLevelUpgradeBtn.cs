using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipLevelUpgradeBtn : MonoBehaviour
{
  // 현재 클릭된 장비
    public EquipmentManager equipmentManager; // 장비 매니저 레퍼런스
    public CurrencyManager currencyManager;  // 화폐 매니저 레퍼런스

    private Button button; // 이 게임 오브젝트에 부착된 버튼 컴포넌트

    void Start()
    {
        button = GetComponent<Button>(); // 현재 게임 오브젝트에서 버튼 컴포넌트를 찾습니다.
        button.onClick.AddListener(OnEquipLevelUpgradeBtnClick); // 버튼 클릭 이벤트에 메서드를 연결합니다.
    }

    private void Update()
    {
        if (EquipmentController.instance.clickedEquipForUpgrade != null) // 클릭된 장비가 있다면
        {
            int userGold = currencyManager.GetCurrencyAmount("Gold"); // 유저의 현재 골드 양을 가져옵니다.
            if (userGold >= EquipmentController.instance.clickedEquipForUpgrade.upgradeGoldCost)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }

    public void OnEquipLevelUpgradeBtnClick()
    {
        if (EquipmentController.instance.clickedEquipForUpgrade != null) 
        {
            equipmentManager.EquipLevelUpgrade(EquipmentController.instance.clickedEquipForUpgrade); 
        }
    }
}