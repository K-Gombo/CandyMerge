using System;
using UnityEngine;
using UnityEngine.UI;

public class AutoCreateBtn : MonoBehaviour
{
    public Button ACOffBtn;
    public Button ACOnBtn;
    public GiftBoxController giftBoxController; // GiftBoxController 참조
    public CandyController candycontroller; // candyController 참조

    public bool isOn = false;
    public Text cooldownText;

    private DateTime pauseTime; // 앱이 비활성화되거나 종료될 때의 시간
    private TimeSpan timeElapsedWhilePaused = TimeSpan.Zero; // 앱이 비활성화되는 동안 지난 시간을 저장

    private void Start()
    {

        // ES3에서 마지막 광고 시간과 버프 상태를 불러옴
        if (ES3.KeyExists("LastAdTime"))
        {
            lastAdTime = ES3.Load<DateTime>("LastAdTime");
        }
        if (ES3.KeyExists("HasCooldown"))
        {
            hasCooldown = ES3.Load<bool>("HasCooldown");
        }

        ACOffBtn.gameObject.SetActive(true); // ACOffBtn을 활성화
        ACOnBtn.gameObject.SetActive(false); // ACOnBtn을 비활성화

        ACOffBtn.onClick.AddListener(OnACOffBtnClick);
        ACOnBtn.onClick.AddListener(OnACOnBtnClick);

        if (ES3.KeyExists("LastPauseTime"))
        {
            DateTime lastPauseTime = ES3.Load<DateTime>("LastPauseTime");
            TimeSpan timeElapsedWhilePaused = DateTime.Now - lastPauseTime;

            // 지난 시간만큼 lastAdTime을 조정하여 버프 지속시간을 유지
            lastAdTime += timeElapsedWhilePaused;
        }
    }


    private void OnApplicationQuit()
    {
        // 앱이 종료될 때 현재 시간을 저장
        ES3.Save("LastPauseTime", DateTime.Now);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // 앱이 비활성화될 때 현재 시간을 저장
            ES3.Save("LastPauseTime", DateTime.Now);
        }
        else
        {
            // 앱이 다시 활성화될 때, 마지막으로 저장된 시간이 있는지 확인
            if (ES3.KeyExists("LastPauseTime"))
            {
                DateTime lastPauseTime = ES3.Load<DateTime>("LastPauseTime");
                TimeSpan timeElapsedWhilePaused = DateTime.Now - lastPauseTime;

                // 지난 시간만큼 lastAdTime을 조정하여 버프 지속시간을 유지
                lastAdTime += timeElapsedWhilePaused;
            }
        }
    }


    public void OnACOffBtnClick()
    {
       
        ACOffBtn.gameObject.SetActive(false);
        ACOnBtn.gameObject.SetActive(true);
        giftBoxController.ToggleFastAutoCreate(true); // 빠른 자동 생성 활성화
        candycontroller.ToggleFastAutoMerge(true); // 빠른 자동 머지 활성화
        isOn = false;
        ToggleBuff();
        DataController.instance.Auto_Create_Save();

    }

    public void OnACOnBtnClick() {
       
        ACOffBtn.gameObject.SetActive(true);
        ACOnBtn.gameObject.SetActive(false);
        giftBoxController.ToggleFastAutoCreate(false); // 빠른 자동 생성 비활성화
        candycontroller.ToggleFastAutoMerge(false); // 빠른 자동 머지 비활성화
        isOn = true;
        ToggleBuff();
        DataController.instance.Auto_Create_Save();
    }

    public void OnACOffGachaClick() {

        giftBoxController.ToggleFastAutoCreate(true); // 빠른 자동 생성 활성화
        candycontroller.ToggleFastAutoMerge(true); // 빠른 자동 머지 활성화
        isBuffActive = true;
    }

    public void OnACOnGachaClick() {
       
        giftBoxController.ToggleFastAutoCreate(false); // 빠른 자동 생성 비활성화
        candycontroller.ToggleFastAutoMerge(false); // 빠른 자동 머지 비활성화
        isBuffActive = false;
    }



    private DateTime lastAdTime;
    private TimeSpan buffDuration = TimeSpan.FromMinutes(10);
    private bool isBuffActive = false;
    private bool hasCooldown = false;



    // 광고를 보고 쿨타임을 얻는 메서드
    public void WatchAd()
    {
        lastAdTime = DateTime.Now;
        hasCooldown = true;

        // ES3로 마지막 광고 시간과 버프 상태 저장
        ES3.Save("LastAdTime", lastAdTime);
        ES3.Save("HasCooldown", hasCooldown);
    }

    // 버프를 켜고 끄는 메서드
    public void ToggleBuff()
    {
        if (isBuffActive)
        {
            DeactivateBuff();
        }
        else
        {
            ActivateBuff();
        }
    }

    // 버프 활성화
    private void ActivateBuff()
    {
        if (hasCooldown)
        {
            isBuffActive = true;
        }
        else
        {
            AdsManager.instance.ShowRewarded(RewardType.AutoCreate);
            Debug.Log("쿨타임이 없습니다. 광고를 봐주세요.");
        }
    }

    // 버프 비활성화
    private void DeactivateBuff()
    {
        isBuffActive = false;
    }

    // 업데이트에서 쿨타임과 버프 상태를 확인
    void Update()
    {
        Debug.Log($"기록을 보자 {hasCooldown}  /  {isBuffActive}");
        if (hasCooldown && isBuffActive)
        {
            cooldownText.gameObject.SetActive(true);
            TimeSpan remainingTime = buffDuration - (DateTime.Now - lastAdTime);
            cooldownText.text = $"{remainingTime.Minutes}:{remainingTime.Seconds}";

            if (DateTime.Now - lastAdTime >= buffDuration)
            {
                DeactivateBuff();
                hasCooldown = false;
                ES3.Save("HasCooldown", hasCooldown);
                cooldownText.text = "Buff has ended.";
            }
        }
        else
        {
            cooldownText.gameObject.SetActive(false);
        }
    }
}