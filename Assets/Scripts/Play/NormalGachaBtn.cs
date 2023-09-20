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
    public Transform equipSpawnLocation;
    private int specialGachaCost = 100;

    private void Start()
    {
        normalGachaBtn.onClick.AddListener(() =>
        {
            
            string UserDia = CurrencyManager.instance.GetCurrencyAmount("Dia");
            BigInteger currentDiaAmount = BigInteger.Parse(UserDia);
            
            if (currentDiaAmount < specialGachaCost)
            {
                Debug.Log("다이아가 부족합니다.");
                return;
            }
            
            int desiredTotalLevel = 996;
            
            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);
            
            equipmentManager.CreateEquipPrefab(equipSpawnLocation, probabilities);
            
            CurrencyManager.instance.SubtractCurrency("Dia", specialGachaCost);
        });
    }
}