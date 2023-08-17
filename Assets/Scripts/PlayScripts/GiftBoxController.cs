using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GiftBoxController : MonoBehaviour
{
    public GameObject candyPrefab;
    public Transform boxTile;
    public Image giftBoxFill;
    public Text createCandyText;
    private int candiesRemaining = 0;
    private int maxCandies = 11;
    public bool autoCreateEnabled = false;
    private Coroutine autoCreateCoroutine;

    private void Start()
    {
        StartCoroutine(FillAndCreateCandies());
        createCandyText.text = candiesRemaining + "/" + maxCandies;
    }

    private IEnumerator FillAndCreateCandies()
    {
        while (true)
        {
            if (candiesRemaining < maxCandies)
            {
                float timeElapsed = 0f;

                while (timeElapsed < 1f)
                {
                    timeElapsed += Time.deltaTime;
                    giftBoxFill.fillAmount = timeElapsed / 1f;
                    yield return null;
                }

                candiesRemaining++;
                createCandyText.text = candiesRemaining + "/" + maxCandies;

                if (candiesRemaining == maxCandies)
                {
                    giftBoxFill.fillAmount = 1f;
                }
                else
                {
                    giftBoxFill.fillAmount = 0f;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator AutoCreateCandy(int timesPer10Seconds)
    {
        while (true)
        {
            for (int i = 0; i < timesPer10Seconds; i++)
            {
                if (candiesRemaining > 0 && IsSpaceAvailableInBox())
                {
                    CreateCandy();
                    candiesRemaining--;
                    createCandyText.text = candiesRemaining + "/" + maxCandies;
                }

                yield return new WaitForSeconds(1f / timesPer10Seconds);
            }
        }
    }

    public void OnGiftBoxClick()
    {
        if (candiesRemaining > 0 && IsSpaceAvailableInBox())
        {
            CreateCandy();
            candiesRemaining--;
            createCandyText.text = candiesRemaining + "/" + maxCandies;
        }
    }

    private bool IsSpaceAvailableInBox()
    {
        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform child = boxTile.GetChild(i);
            if (child.childCount == 0 && child.CompareTag("Box"))
            {
                return true;
            }
        }
        return false;
    }

    private void CreateCandy()
    {
        List<Transform> availableBoxes = new List<Transform>();

        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform child = boxTile.GetChild(i);
            if (child.childCount == 0 && child.CompareTag("Box"))
            {
                availableBoxes.Add(child);
            }
        }

        if (availableBoxes.Count > 0)
        {
            int randomIndex = Random.Range(0, availableBoxes.Count);
            Transform selectedBox = availableBoxes[randomIndex];
            Instantiate(candyPrefab, selectedBox.position, Quaternion.identity, selectedBox);
        }
    }

    public void ToggleFastAutoCreate(bool isEnabled) 
    {
        if (autoCreateCoroutine != null)
        {
            StopCoroutine(autoCreateCoroutine); // 이미 실행 중인 코루틴이 있다면 중지
        }

        if (isEnabled)
        {
            autoCreateCoroutine = StartCoroutine(DelayedAutoCreateCandy(1));
        }
        else
        {
            StopCoroutine(autoCreateCoroutine);
        }
    }

    private IEnumerator DelayedAutoCreateCandy(int timesPer10Seconds)
    {
        yield return new WaitForSeconds(1f); // 1초의 딜레이
        yield return AutoCreateCandy(timesPer10Seconds); // 생성 시작
    }
}