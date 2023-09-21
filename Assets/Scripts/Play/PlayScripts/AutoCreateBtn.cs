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