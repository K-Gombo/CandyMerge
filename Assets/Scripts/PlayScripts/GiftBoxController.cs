using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GiftBox : MonoBehaviour
{
    public GameObject candyPrefab; // Candy 프리팹을 참조
    public Transform boxTile; // Boxtile 오브젝트의 Transform을 참조
    public Image giftBoxFill; // GiftBox(fill)의 Image 컴포넌트를 참조
    public Text createCandyText; // CreateCandy 텍스트를 참조
    private int candiesRemaining = 0;

    private void Start()
    {
        StartCoroutine(FillAndCreateCandies());
        createCandyText.text = candiesRemaining + "/11";
    }

    public void OnGiftBoxClick()
    {
        if (candiesRemaining > 0)
        {
            CreateCandy();
            candiesRemaining--;
            createCandyText.text = candiesRemaining + "/11";
        }
    }

    private IEnumerator FillAndCreateCandies()
    {
        while (candiesRemaining < 11)
        {
            float timeElapsed = 0f;

            while (timeElapsed < 1f)
            {
                timeElapsed += Time.deltaTime;
                giftBoxFill.fillAmount = timeElapsed / 1f;
                yield return null;
            }

            giftBoxFill.fillAmount = 0f;
            candiesRemaining++;
            createCandyText.text = candiesRemaining + "/11";
        }
    }

    private void CreateCandy()
    {
        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform child = boxTile.GetChild(i);
            if (child.childCount == 0)
            {
                Instantiate(candyPrefab, child.position, Quaternion.identity, child);
                return; // 캔디를 생성했으므로 함수를 빠져나옵니다.
            }
        }
    }
}