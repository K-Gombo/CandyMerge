using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class RewardMovingManager : MonoBehaviour
{
    public static RewardMovingManager instance;

    [SerializeField] private Transform pileOfCurrency;

    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRot;

    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite diaSprite;

    [SerializeField] private Vector2 goldPos = new Vector2(-295, 1400);
    [SerializeField] private Vector2 diaPos = new Vector2(120, 1400);

    private Vector2 targetPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        initialPos = new Vector2[pileOfCurrency.childCount];
        initialRot = new Quaternion[pileOfCurrency.childCount];

        for (int i=0; i< pileOfCurrency.childCount; i++)
        {
            initialPos[i] = pileOfCurrency.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRot[i] = pileOfCurrency.GetChild(i).GetComponent<RectTransform>().rotation;
        }
    }

    public void MovingCurrency(int currencyCount, CurrencyType type)
    {
        pileOfCurrency.gameObject.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < currencyCount; i++) 
        {
            var child = pileOfCurrency.transform.GetChild(i);
            switch (type)
            {
                case CurrencyType.Gold:
                    child.GetComponent<Image>().sprite = goldSprite;
                    targetPos = goldPos;
                    break;
                case CurrencyType.Dia:
                    child.GetComponent<Image>().sprite = diaSprite;
                    targetPos = diaPos;
                    break;
            }
            child.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            child.GetComponent<RectTransform>().DOAnchorPos(targetPos, 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);

            child.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                .SetEase(Ease.Flash);

            child.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);
            delay += 0.1f;

            child.GetComponent<RectTransform>().anchoredPosition = initialPos[i];
            child.GetComponent<RectTransform>().rotation = initialRot[i];
        }
    }
}
