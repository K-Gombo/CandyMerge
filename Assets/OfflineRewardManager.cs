using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Numerics;
using Keiwando.BigInteger;

public class OfflineRewardManager : MonoBehaviour
{
    private TimeSpan totalAccumulatedTime;
    private const double MAX_ACCUMULATION_TIME_IN_MINUTES = 3 * 60;//24 * 60;  // 24 hours in minutes
    BigInteger goldReward;
    
    [SerializeField] GameObject offlineRewardPanel;
    [SerializeField] Text goldText;
    
    public float offLineRewardIncreament = 0f;
    public float equipOffLineRewardIncreament = 0f;

    public GameObject openBtnList;
    public GameObject button_Get;

    public static OfflineRewardManager instance;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        if (ES3.KeyExists("LAST_LOGIN") && !DidUserClaimReward())
        {
            DateTime lastLogIn = ES3.Load<DateTime>("LAST_LOGIN");
            TimeSpan ts = DateTime.Now - lastLogIn;

            // Load accumulated time if it exists, otherwise default to a TimeSpan of 0
            totalAccumulatedTime = ES3.KeyExists("TOTAL_ACCUMULATED_TIME") ? ES3.Load<TimeSpan>("TOTAL_ACCUMULATED_TIME") : new TimeSpan();

            // Add the recent offline time to the total accumulated time
            totalAccumulatedTime += ts;

            // Limit accumulation to 24 hours
            if (totalAccumulatedTime.TotalMinutes > MAX_ACCUMULATION_TIME_IN_MINUTES)
                totalAccumulatedTime = TimeSpan.FromMinutes(MAX_ACCUMULATION_TIME_IN_MINUTES);

            //Debug.Log(string.Format("{0} Days {1} Hours {2} Minutes {3} Seconds Ago", ts.Days, ts.Hours, ts.Minutes, ts.Seconds));
            Debug.Log(GetCurrentRewardTimeAsString());

            if (totalAccumulatedTime.Seconds >= 1)
            {
                GetOfflineReward();
            }
            Debug.Log((int)ts.TotalMinutes + "g");
        }
        else
        {
            Debug.Log("WELCOME");
            Debug.Log("0g");
            totalAccumulatedTime = new TimeSpan();
        }
    }

    void GetOfflineReward()
    {
        goldReward = (QuestManager.instance.candyPriceByLevel[CandyStatus.baseLevel] * 40) * totalAccumulatedTime.Seconds;

        // 두 증가율을 더한 값으로 적용합니다.
        float totalIncreament = offLineRewardIncreament + equipOffLineRewardIncreament;

        // BigInteger를 float로 변환
        float goldRewardFloat = float.Parse(goldReward.ToString());

        // 증가율을 적용
        goldRewardFloat *= (1 + (totalIncreament / 100));

        // 다시 BigInteger로 변환
        goldReward = new BigInteger(Math.Round(goldRewardFloat).ToString());

        Debug.Log("얼마 나왔니? : " + goldReward);
        goldText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(goldReward.ToString());
        offlineRewardPanel.SetActive(true);
    }


    public string GetCurrentRewardTimeAsString()
    {
        return string.Format("{0} Days {1} Hours {2} Minutes {3} Seconds",
                             totalAccumulatedTime.Days,
                             totalAccumulatedTime.Hours,
                             totalAccumulatedTime.Minutes,
                             totalAccumulatedTime.Seconds);
    }

    public TimeSpan GetCurrentRewardTime()
    {
        return totalAccumulatedTime;
    }

    private bool DidUserClaimReward()
    {
        return ES3.KeyExists("CLAIMED_REWARD") && ES3.Load<bool>("CLAIMED_REWARD");
    }

    public void ClaimReward()
    {
        // This method is called when the user claims their offline reward
        ES3.Save<bool>("CLAIMED_REWARD", true);
        totalAccumulatedTime = new TimeSpan(); // Reset accumulated time
    }

    private void OnApplicationQuit()
    {
        ES3.Save<DateTime>("LAST_LOGIN", DateTime.Now);
        ES3.Save<TimeSpan>("TOTAL_ACCUMULATED_TIME", totalAccumulatedTime);
        if (DidUserClaimReward())
            ES3.Save<bool>("CLAIMED_REWARD", false);  // Reset the reward claim flag
    }


    public void OfflineRewardCleck()
    {
        RewardMovingManager.instance.RequestMovingCurrency(5, CurrencyType.Gold, goldReward.ToString());

        offlineRewardPanel.SetActive(false);

    }

    public void ShowAds()
    {
        AdsManager.instance.ShowRewarded(RewardType.OffLine);
    }

    public void ShowAdsRwardPanel()
    {
        goldText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney((goldReward * 2).ToString());

        openBtnList.SetActive(false);
        button_Get.SetActive(true);
    }

    public void OfflineAdsRewardCleck()
    {
        RewardMovingManager.instance.RequestMovingCurrency(5, CurrencyType.Gold, (goldReward * 2).ToString());

        offlineRewardPanel.SetActive(false);
    }
    
    
    
    
    
    
    public float GetEquipOffLineRewardUp()
    {   
        return equipOffLineRewardIncreament;
    }

    public void SetEquipOffLineRewardUp(float newequipOffLineRewardIncreament)
    {
        newequipOffLineRewardIncreament = Mathf.Round(newequipOffLineRewardIncreament * 10f) / 10f;
        equipOffLineRewardIncreament = newequipOffLineRewardIncreament;
    }
    
    
    public void ResetEquipOffLineRewardUp(EquipmentStatus equipment)
    {
        float currentEquipOffLineRewardUp = GetEquipOffLineRewardUp();
        float newEquipOffLineRewardUp = currentEquipOffLineRewardUp;
        bool skillIdExists = false;
        
        int[] targetSkillIds = { 1, 2, 3, 4, 5 };

        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i])&& equipment.skillUnlocked[i])
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;

                // 해당 skillId의 skillPoints를 빼기
                newEquipOffLineRewardUp -= equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }

        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }

        // 새로운 값을 설정
        SetEquipOffLineRewardUp(newEquipOffLineRewardUp);
        Debug.Log($"캔디 레벨 2단계 상승 확률 초기화: {newEquipOffLineRewardUp}");
    }
    
    
    
    public float GetOffLineRewardBonusUp()
    {
        return offLineRewardIncreament;
    }

    public void SetOffLineRewardBonusUp(float newoffLineRewardIncreament)
    {
        newoffLineRewardIncreament = Mathf.Round(newoffLineRewardIncreament * 10f) / 10f; // 소수 둘째자리에서 반올림
        offLineRewardIncreament = Mathf.Min(newoffLineRewardIncreament);
    }
    
    
    
}
