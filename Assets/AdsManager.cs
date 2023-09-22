using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RewardType
{
    None,
    AutoCreate,
    OffLine,
    BoxOpen,
    LevelUp
}

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private const string MaxSdkKey = "QqxxU259hFqPcXv2vIq4mtdJVaJ7Dt7G3WBsXWONa76yR9urWDB9M55t5o7ZlHG602Od1bEFcTGNOmxNxsEv1L";
    private const string BannerAdUnitId = "BannerAdUnitId";
    private const string InterstitialAdUnitId = "InterstitialAdUnitId";
    private const string RewardedAdUnitId = "be9f3f2745eb9ddc";

    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;

    [SerializeField] OfflineRewardManager offlineRewardManager;
    [SerializeField] AdsGachaBtn adsGachaBtn;
    [SerializeField] AutoCreateBtn autoCreateBtn;

    RewardType currentRewardType;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // AppLovinMax 초기화
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.Log("MAX SDK Initialized");

            InitializeRewardedAds();
        };

        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
    }



    #region Rewarded Ad Methods

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
    }

    //광고를 가져온다.
    private void LoadRewardedAd()
    {
        Debug.Log("Loading...");
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    //가져온 광고를 보여준다.
    private void ShowRewardedAd()
    {
        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
        {
            Debug.Log("Showing");
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);
        }
        else
        {
            Debug.Log("Ad not ready");
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        Debug.Log("Rewarded ad loaded");

        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

        Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {

        AdsReward();

        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Rewarded ad revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

    }

    public void ShowRewarded(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.AutoCreate:
                currentRewardType = RewardType.AutoCreate;
                break;
            case RewardType.OffLine:
                currentRewardType = RewardType.OffLine;
                break;
            case RewardType.BoxOpen:
                currentRewardType = RewardType.BoxOpen;
                break;
            case RewardType.LevelUp:
                currentRewardType = RewardType.LevelUp;
                break;
            default:
                return;
        }
        ShowRewardedAd();
    }

    void AdsReward()
    {
        switch (currentRewardType)
        {
            case RewardType.AutoCreate:
                autoCreateBtn.WatchAd();
            break;
            case RewardType.OffLine:
                offlineRewardManager.ShowAdsRwardPanel();
            break;
            case RewardType.BoxOpen:
                adsGachaBtn.BoxOpenReward();
            break;
            case RewardType.LevelUp:
                HappyLevel.instance.AdsComprate();
            break;
            default:
                return;
        }
    }

    #endregion

}
