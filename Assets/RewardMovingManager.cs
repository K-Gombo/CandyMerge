using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Keiwando.BigInteger;

public enum CurrencyType
{
    Gold,
    Dia
    // ... 추가적인 재화들
}

[System.Serializable]
public struct CurrencyMetaData
{
    public CurrencyType type;
    public Sprite sprite;
    public Vector2 targetPosition;
    public GameObject fadeOut;
}


public class RewardMovingManager : MonoBehaviour
{
    public static RewardMovingManager instance;

    [SerializeField] private Transform currencyIconPrefab;
    [SerializeField] private Transform currencyIconContainer;
    [SerializeField] private List<CurrencyMetaData> currencyMetaDatas;
    [SerializeField] private int initialPoolSize = 10;

    private List<GameObject> fadeOutPool = new List<GameObject>();

    private List<Transform> currencyIconPool = new List<Transform>();
    private Vector2[] initialPos;
    private Quaternion[] initialRot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddIconToPool(Instantiate(currencyIconPrefab, currencyIconContainer));
        }

        initialPos = new Vector2[initialPoolSize];
        initialRot = new Quaternion[initialPoolSize];

        for (int i = 0; i < initialPoolSize; i++)
        {
            RectTransform iconTransform = currencyIconPool[i].GetComponent<RectTransform>();
            initialPos[i] = iconTransform.anchoredPosition;
            initialRot[i] = iconTransform.rotation;
        }
    }

    //public void RequestMovingCurrency(int count, CurrencyType type, int reward, Vector2? dynamicStartPosition = null)
    public void RequestMovingCurrency(int count, CurrencyType type, string reward, Vector2? dynamicStartPosition = null)
    {
        currencyIconContainer.gameObject.SetActive(true);
        var delay = 0f;
        var currencyData = currencyMetaDatas.Find(data => data.type == type);

        for (int i = 0; i < count; i++)
        {
            var icon = GetIconFromPool();

            SetupIcon(icon, currencyData.sprite, delay, currencyData.targetPosition, type);

            if (dynamicStartPosition.HasValue)
            {
                SetDynamicStartPosition(icon, dynamicStartPosition.Value);
            }
            else
            {
                ResetIconPositionAndRotation(icon, i);
            }
            delay += 0.1f;
        }

        GameObject activeFadeOut = RequestFadeOutInstance(currencyData);
        activeFadeOut.GetComponent<FadeOutController>().SetFadeOutText($"+{BigIntegerCtrl_global.bigInteger.ChangeMoney(reward)}");

        BigInteger Bigreward = new BigInteger(reward);
        CurrencyManager.instance.AddCurrency(type.ToString(), Bigreward);

    }

    private void SetupIcon(Transform icon, Sprite sprite, float delay, Vector2 targetPosition, CurrencyType type)
    {
        Image iconImage = icon.GetComponent<Image>();
        RectTransform iconTransform = icon.GetComponent<RectTransform>();

        iconImage.sprite = sprite;

        icon.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
        iconTransform.DOAnchorPos(targetPosition, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            SoundManager.Instance.PlaySoundEffect(type.ToString());
        });
        icon.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
        icon.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            icon.gameObject.SetActive(false);
            currencyIconPool.Add(icon);
        });
    }

    private void SetDynamicStartPosition(Transform icon, Vector2 startPosition)
    {
        float randomX = Random.Range(-50f, 50f);  // 예시 값, 원하는 범위로 조정
        float randomY = Random.Range(-50f, 50f);  // 예시 값, 원하는 범위로 조정
        Vector2 randomizedStartPosition = startPosition + new Vector2(randomX, randomY);

        RectTransform iconTransform = icon.GetComponent<RectTransform>();
        iconTransform.anchoredPosition = randomizedStartPosition;
        iconTransform.rotation = Quaternion.identity;
    }

    private void ResetIconPositionAndRotation(Transform icon, int index)
    {
        RectTransform iconTransform = icon.GetComponent<RectTransform>();
        iconTransform.anchoredPosition = initialPos[index];
        iconTransform.rotation = initialRot[index];
    }

    private Transform GetIconFromPool()
    {
        if (currencyIconPool.Count == 0)
        {
            return AddIconToPool(Instantiate(currencyIconPrefab, currencyIconContainer));
        }

        var pooledIcon = currencyIconPool[0];
        currencyIconPool.RemoveAt(0);
        pooledIcon.gameObject.SetActive(true);
        return pooledIcon;
    }

    private Transform AddIconToPool(Transform icon)
    {
        icon.gameObject.SetActive(false);
        currencyIconPool.Add(icon);
        return icon;
    }

    // FadeOut Pool 관리
    private void InitializeFadeOutPool(CurrencyMetaData currencyData, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject newFadeOut = Instantiate(currencyData.fadeOut, currencyData.fadeOut.transform.parent);
            newFadeOut.SetActive(false);
            fadeOutPool.Add(newFadeOut);
        }
    }


    private GameObject GetFadeOutFromPool(CurrencyMetaData currencyData)
    {
        if (fadeOutPool.Count == 0)
        {
            InitializeFadeOutPool(currencyData, 5); // 필요에 따라 추가로 5개의 fadeOut 오브젝트를 생성
        }

        
        var pooledFadeOut = fadeOutPool[0];
        fadeOutPool.RemoveAt(0);
        pooledFadeOut.transform.parent = currencyData.fadeOut.transform.parent;
        pooledFadeOut.SetActive(true);
        return pooledFadeOut;
    }


    public void ReturnFadeOutToPool(GameObject fadeOutObject)
    {
        fadeOutObject.SetActive(false);
        fadeOutPool.Add(fadeOutObject);
    }

    public GameObject RequestFadeOutInstance(CurrencyMetaData currencyData)
    {
        GameObject activeFadeOut = GetFadeOutFromPool(currencyData);
        activeFadeOut.GetComponent<FadeOutController>().Manager = this; // Setting the manager reference
        return activeFadeOut;
    }


}
