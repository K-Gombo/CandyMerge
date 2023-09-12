using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

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
}

public class RewardMovingManager : MonoBehaviour
{
    public static RewardMovingManager instance;

    [SerializeField] private Transform currencyIconPrefab;
    [SerializeField] private Transform currencyIconContainer;
    [SerializeField] private List<CurrencyMetaData> currencyMetaDatas;
    [SerializeField] private int initialPoolSize = 10;

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
            var icon = Instantiate(currencyIconPrefab, currencyIconContainer);
            icon.gameObject.SetActive(false);
            currencyIconPool.Add(icon);
        }

        initialPos = new Vector2[initialPoolSize];
        initialRot = new Quaternion[initialPoolSize];

        for (int i = 0; i < initialPoolSize; i++)
        {
            initialPos[i] = currencyIconPool[i].GetComponent<RectTransform>().anchoredPosition;
            initialRot[i] = currencyIconPool[i].GetComponent<RectTransform>().rotation;
        }
    }

    public void RequestMovingCurrency(int count, CurrencyType type)
    {
        MovingCurrency(count, type);
    }
    public void RequestMovingCurrency(int count, CurrencyType type, Vector2 dynamicStartPosition)
    {
        MovingCurrency(count, type, dynamicStartPosition);
    }

    private void MovingCurrency(int currencyCount, CurrencyType type)
    {
        currencyIconContainer.gameObject.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < currencyCount; i++)
        {
            var icon = GetIconFromPool();
            var currencyData = currencyMetaDatas.Find(data => data.type == type);

            icon.GetComponent<Image>().sprite = currencyData.sprite;

            icon.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            icon.GetComponent<RectTransform>().DOAnchorPos(currencyData.targetPosition, 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            icon.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                .SetEase(Ease.Flash);
            icon.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack).OnComplete(() => {
                icon.gameObject.SetActive(false);
                currencyIconPool.Add(icon);
            });
            delay += 0.1f;

            icon.GetComponent<RectTransform>().anchoredPosition = initialPos[i];
            icon.GetComponent<RectTransform>().rotation = initialRot[i];
        }
    }

    private void MovingCurrency(int currencyCount, CurrencyType type, Vector2 dynamicStartPosition)
    {
        currencyIconContainer.gameObject.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < currencyCount; i++)
        {
            var icon = GetIconFromPool();

            // 랜덤한 위치 오프셋을 생성합니다.
            float randomX = Random.Range(-50f, 50f);  // 예시 값, 원하는 범위로 조정
            float randomY = Random.Range(-50f, 50f);  // 예시 값, 원하는 범위로 조정

            Vector2 randomizedStartPosition = dynamicStartPosition + new Vector2(randomX, randomY);

            // 아이콘의 초기 위치와 회전을 설정합니다.
            icon.GetComponent<RectTransform>().anchoredPosition = randomizedStartPosition;
            icon.GetComponent<RectTransform>().rotation = Quaternion.identity;

            var currencyData = currencyMetaDatas.Find(data => data.type == type);
            icon.GetComponent<Image>().sprite = currencyData.sprite;

            icon.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            icon.GetComponent<RectTransform>().DOAnchorPos(currencyData.targetPosition, 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            icon.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                .SetEase(Ease.Flash);
            icon.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack).OnComplete(() => {
                icon.gameObject.SetActive(false);
                currencyIconPool.Add(icon);
            });
            delay += 0.1f;
        }
    }


    private Transform GetIconFromPool()
    {
        if (currencyIconPool.Count == 0)
        {
            var icon = Instantiate(currencyIconPrefab, currencyIconContainer);
            icon.gameObject.SetActive(false);
            currencyIconPool.Add(icon);
            return icon;
        }

        var pooledIcon = currencyIconPool[0];
        currencyIconPool.RemoveAt(0);
        pooledIcon.gameObject.SetActive(true);
        return pooledIcon;
    }
}