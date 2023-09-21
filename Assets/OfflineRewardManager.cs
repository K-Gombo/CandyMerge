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

    public void OfflineAdsRewardCleck()
    {
        Debug.Log("광고 봤으니 실행 해볼까?!");
        RewardMovingManager.instance.RequestMovingCurrency(5, CurrencyType.Gold, (goldReward * 2).ToString());

        offlineRewardPanel.SetActive(false);
    }
}
