using System.Collections.Generic;
using System.Numerics; // BigInteger를 사용하기 위해 필요
using UnityEngine;
using UnityEngine.UI;

public class SpecialGachaBtn : MonoBehaviour
{
    public Button specialGachaBtn;
    public GachaManager GachaManager;
    public EquipmentManager equipmentManager;
    public Transform equipGachaSpawnLocation;
    public Transform equipSpawnLocation;
    public Button gachaResultCloseBtn;
    public GameObject equipResultPanel;
    private int specialGachaCost = 200;

    private void Start()
    {
        specialGachaBtn.onClick.AddListener(() =>
        {
            string UserDia = CurrencyManager.instance.GetCurrencyAmount("Dia");
            BigInteger currentDiaAmount = BigInteger.Parse(UserDia);

            if (currentDiaAmount < specialGachaCost)
            {
                Debug.Log("다이아가 부족합니다.");
                return;
            }

            int desiredTotalLevel = 999;

            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);

            // 장비를 5개 생성합니다.
            for (int i = 0; i < 5; i++)
            {
                equipmentManager.CreateEquipPrefab(equipGachaSpawnLocation, probabilities);
            }
            equipResultPanel.SetActive(true);

            CurrencyManager.instance.SubtractCurrency("Dia", specialGachaCost);
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

    }
}