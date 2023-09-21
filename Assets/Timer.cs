using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public Text cooldownText;  // Unity Editor에서 연결할 Text UI
    private const string LastAdTimeKey = "LastAdTime";
    private const int CooldownMinutes = 10;

    [SerializeField] Button adsButton;


    void Update()
    {
        CheckCooldown();
    }

    public void OnAdWatched()
    {
        // 현재 시간을 저장
        DateTime now = DateTime.UtcNow;
        ES3.Save<DateTime>(LastAdTimeKey, now);
    }

    public void CheckCooldown()
    {
        if (!ES3.KeyExists(LastAdTimeKey))
        {
            EnableAdButton();
            return;
        }

        Debug.Log("시간이 다!");
        DateTime lastAdTime = ES3.Load<DateTime>(LastAdTimeKey);
        DateTime now = DateTime.UtcNow;
        TimeSpan diff = now - lastAdTime;

        if (diff.TotalMinutes >= CooldownMinutes)
        {
            EnableAdButton();
        }
        else
        {
            DisableAdButton();
            TimeSpan remainingTime = TimeSpan.FromMinutes(CooldownMinutes) - diff;
            cooldownText.text = string.Format("{0:D2}:{1:D2}", remainingTime.Minutes, remainingTime.Seconds);
        }
    }

    private void EnableAdButton()
    {
        // 광고 버튼 활성화 로직
        gameObject.SetActive(false);
        adsButton.gameObject.SetActive(true);
        
    }

    private void DisableAdButton()
    {
        // 광고 버튼 비활성화 로직
        gameObject.SetActive(true);
        adsButton.gameObject.SetActive(false);
    }
}
