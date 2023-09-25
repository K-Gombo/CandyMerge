using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    private float targetProgress = 0f;

    private void Awake()
    {
        SetProgress(1, 2);
    }
    public void SetProgress(float value, float duration)
    {
        StopAllCoroutines();  // 이미 진행중인 Coroutine이 있으면 중지
        StartCoroutine(FillProgress(value, duration));
    }

    private IEnumerator FillProgress(float targetValue, float duration)
    {
        float startValue = loadingBar.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadingBar.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            yield return null;
        }
        loadingBar.value = targetValue;  // 마지막으로 목표값에 도달하게 설정

        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
        CurrencyManager.instance.LoadCurrencies();
        BoxManager.instance.UpdateCandyCount();
        OfflineRewardManager.instance.Start_Offline();
    }

}
