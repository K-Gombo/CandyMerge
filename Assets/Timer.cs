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

    // 이 메서드는 광고가 성공적으로 보여진 후에 호출됩니다.
    public void OnAdWatched()
    {
        // 현재 시간을 저장
        DateTime now = DateTime.UtcNow;
        PlayerPrefs.SetString(LastAdTimeKey, now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    // 앱이 활성화되거나, 쿨타임을 체크하려는 경우에 호출됩니다.
    public void CheckCooldown()
    {
        // 마지막 광고 시간을 불러옴
        string lastAdTimeStr = PlayerPrefs.GetString(LastAdTimeKey, string.Empty);

        if (string.IsNullOrEmpty(lastAdTimeStr))
        {
            // 쿨타임 정보가 없으면 광고를 볼 수 있음
            EnableAdButton();
            return;
        }

        DateTime lastAdTime = DateTime.FromBinary(Convert.ToInt64(lastAdTimeStr));
        DateTime now = DateTime.UtcNow;

        // 시간 차이 계산
        TimeSpan diff = now - lastAdTime;

        if (diff.TotalMinutes >= CooldownMinutes)
        {
            // 쿨타임이 지났으므로 광고를 볼 수 있음
            EnableAdButton();
        }
        else
        {
            // 쿨타임이 아직 지나지 않았으므로 광고를 볼 수 없음
            DisableAdButton();

            // 남은 시간을 Text UI로 표시
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
