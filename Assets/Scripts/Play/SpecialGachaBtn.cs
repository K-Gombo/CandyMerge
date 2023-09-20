using System.Numerics; // BigInteger를 사용하기 위해 필요
using UnityEngine;
using UnityEngine.UI;

public class SpecialGachaBtn : MonoBehaviour
{
    public Button specialGachaBtn;
    public GachaManager GachaManager;
    public EquipmentManager equipmentManager;
    public Transform equipSpawnLocation;
    private int specialGachaCost = 1000;

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
            
            equipmentManager.CreateEquipPrefab(equipSpawnLocation, probabilities);
            
            CurrencyManager.instance.SubtractCurrency("Dia", specialGachaCost);
        });
    }
}