using UnityEngine;
using UnityEngine.UI;

public class CandyManager : MonoBehaviour
{
    public static CandyManager instance;
    public Sprite[] candySprites;
    public Text candyCountText; // TextUI를 가리키는 변수
    public int MaxCandyCount = 15; // 최대 캔디 개수

    private int currentCandyCount = 0; // 현재 생성된 캔디 개수를 추적

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

    private void Start()
    {
        UpdateCandyCountText(); // 초기화 시에도 텍스트 업데이트
    }

    public void SetAppearance(GameObject candy)
    {
        SpriteRenderer renderer = candy.GetComponent<SpriteRenderer>();
        CandyStatus status = candy.GetComponent<CandyStatus>();
        int level = status.level;
        renderer.sprite = candySprites[level - 1]; // 레벨에 해당하는 스프라이트를 할당

        currentCandyCount++; // 캔디가 생성될 때마다 개수 증가
        UpdateCandyCountText(); // 텍스트 업데이트
    }

    public void CandyDestroyed()
    {
        currentCandyCount--; // 캔디가 파괴될 때마다 개수 감소
        UpdateCandyCountText(); // 텍스트 업데이트
    }

    private void UpdateCandyCountText()
    {
        candyCountText.text = $"{currentCandyCount}/{MaxCandyCount}"; // 텍스트 업데이트
    }
}