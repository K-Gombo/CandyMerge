using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics; // BigInteger를 사용하기 위해 필요

public class NormalGachaBtn : MonoBehaviour
{
    public Button normalGachaBtn;
    public GachaManager GachaManager;
    public EquipmentManager equipmentManager;
    public Transform equipGachaSpawnLocation;
    public Transform equipSpawnLocation;
    public Button gachaResultCloseBtn;
    public GameObject gachaResultPanel;
    private int normalGachaCost = 80;

    private void Start()
    {
        normalGachaBtn.onClick.AddListener(() =>
        {
            string UserDia = CurrencyManager.instance.GetCurrencyAmount("Dia");
            BigInteger currentDiaAmount = BigInteger.Parse(UserDia);

            if (currentDiaAmount < normalGachaCost)
            {
                Debug.Log("다이아가 부족합니다.");
                return;
            }

            int desiredTotalLevel = 996;

            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);

            // 장비를 5개 생성합니다.
            for (int i = 0; i < 5; i++)
            {
                equipmentManager.CreateEquipPrefab(equipGachaSpawnLocation, probabilities);
            }
            gachaResultPanel.SetActive(true);
            CurrencyManager.instance.SubtractCurrency("Dia", normalGachaCost);
        });

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
        EquipmentManager.instance.CheckMixAvailability();
    }

}
