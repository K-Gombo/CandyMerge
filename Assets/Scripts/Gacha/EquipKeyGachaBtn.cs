using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class EquipKeyGachaBtn : MonoBehaviour
{
    public Button equipKeyGachaBtn;
    public GachaManager GachaManager;
    public EquipmentManager equipmentManager;
    public Transform equipGachaSpawnLocation;
    public Transform equipSpawnLocation;
    public Button gachaResultCloseBtn;
    public GameObject equipResultPanel;

    private void Start()
    {
        equipKeyGachaBtn.onClick.AddListener(() =>
        {
            if (equipmentManager.totalEquipScore == 0)
            {
                return;
            }
            // 버튼 활성화/비활성화 로직
            equipKeyGachaBtn.interactable = (GiftBoxController.instance.keyCount >= 5);

            // totalEquipScore에 따른 desiredTotalLevel 설정
            int desiredTotalLevel = 0; // 초기값
            if (equipmentManager.totalEquipScore >= 1 && equipmentManager.totalEquipScore <= 7)
            {
                desiredTotalLevel = 15;
            }
            else if (equipmentManager.totalEquipScore >= 8 && equipmentManager.totalEquipScore <= 14)
            {
                desiredTotalLevel = 45;
            }
            else if (equipmentManager.totalEquipScore >= 15 && equipmentManager.totalEquipScore <= 21)
            {
                desiredTotalLevel = 85;
            }
            else if (equipmentManager.totalEquipScore >= 22 && equipmentManager.totalEquipScore <= 28)
            {
                desiredTotalLevel = 135;
            }

            // 기존의 가챠 로직
            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);

            // keyCount가 5 이상일 때까지 아이템을 생성
            while (GiftBoxController.instance.keyCount >= 5)
            {
                equipmentManager.CreateEquipPrefab(equipGachaSpawnLocation, probabilities);
                GiftBoxController.instance.keyCount -= 5;
            }
            GachaManager.EquipGachaTextUpdate();
            
            GiftBoxController.instance.keyBoxCountText.text = GiftBoxController.instance.keyCount+" / 5";

            equipResultPanel.SetActive(true);

            // 기존의 결과 닫기 버튼 로직
            gachaResultCloseBtn.onClick.AddListener(() =>
            {
                List<Transform> children = new List<Transform>();
                foreach (Transform child in equipGachaSpawnLocation)
                {
                    children.Add(child);
                }

                foreach (Transform child in children)
                {
                    child.SetParent(equipSpawnLocation);
                }
            });

        });
    }



}