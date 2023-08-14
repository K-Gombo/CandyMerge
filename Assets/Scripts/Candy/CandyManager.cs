using UnityEngine;

public class CandyManager : MonoBehaviour
{
    public static CandyManager instance;
    public Sprite[] candySprites;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAppearance(GameObject candy)
    {
        SpriteRenderer renderer = candy.GetComponent<SpriteRenderer>();
        CandyStatus status = candy.GetComponent<CandyStatus>();
        int level = status.level;
        renderer.sprite = candySprites[level - 1]; // 레벨에 해당하는 스프라이트를 할당
    }
}