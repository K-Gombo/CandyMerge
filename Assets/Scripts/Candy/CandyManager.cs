using UnityEngine;
using UnityEngine.UI;

public class CandyManager : MonoBehaviour
{
    public static CandyManager instance;
    public Sprite[] candySprites;
    public Text candyCountText;
    public int MaxCandyCount; // 최대 캔디 개수

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
        // 시작할 때 안에 Locked 오브젝트가 없는 박스의 개수를 구하여 MaxCandyCount를 설정합니다.
        MaxCandyCount = GameObject.FindGameObjectsWithTag("Box").Length - GameObject.FindGameObjectsWithTag("Locked").Length;
        UpdateCandyCountText();
    }

    public void SetAppearance(GameObject candy)
    {
        SpriteRenderer renderer = candy.GetComponent<SpriteRenderer>();
        CandyStatus status = candy.GetComponent<CandyStatus>();
        int level = status.level;
        renderer.sprite = candySprites[level - 1];

        currentCandyCount++;
        UpdateCandyCountText();
    }

    public void CandyDestroyed()
    {
        currentCandyCount--;
        UpdateCandyCountText();
    }

    public void LockedTileRemoved()  // 나중에 스킬로 캔디 생성 영역 개수를 늘리는거 할떄 사용
    {
        MaxCandyCount++; // Locked 오브젝트가 제거될 때마다 MaxCandyCount를 증가시킵니다.
        UpdateCandyCountText();
    }

    private void UpdateCandyCountText()
    {
        candyCountText.text = $"{currentCandyCount}/{MaxCandyCount}"; // 텍스트 업데이트
    }
}