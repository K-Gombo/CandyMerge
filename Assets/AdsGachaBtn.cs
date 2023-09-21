using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class AdsGachaBtn : MonoBehaviour
{
    public Button adsGachaBtn;
    public GachaManager GachaManager;
    public EquipmentManager equipmentManager;
    public Transform equipGachaSpawnLocation;
    public Transform equipSpawnLocation;
    public Button gachaResultCloseBtn;
    public GameObject gachaResultPanel;

    public Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        adsGachaBtn.onClick.AddListener(ShowAds);

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

    void ShowAds()
    {
        AdsManager.instance.ShowRewarded(RewardType.BoxOpen);
    }

    public void BoxOpenReward()
    {
        string UserDia = CurrencyManager.instance.GetCurrencyAmount("Dia");
        BigInteger currentDiaAmount = BigInteger.Parse(UserDia);


        int desiredTotalLevel = 996;

        float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);

        // 장비를 5개 생성합니다.
        for (int i = 0; i < 5; i++)
        {
            equipmentManager.CreateEquipPrefab(equipGachaSpawnLocation, probabilities);
        }
        gachaResultPanel.SetActive(true);

        adsGachaBtn.gameObject.SetActive(false);
        timer.gameObject.SetActive(true);
        timer.OnAdWatched();
    }

}
