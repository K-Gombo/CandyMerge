using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CandyManager : MonoBehaviour
{
    public static CandyManager instance;
    public Sprite[] candySprites;
    public Text candyCountText;
    public int MaxCandyCount; // 최대 캔디 개수

    [SerializeField]
    private GameObject candyPrefab;
    private Queue<GameObject> candyPool;
    [SerializeField]
    private int poolSize = 10;

    private int currentCandyCount = 0; // 현재 생성된 캔디 개수를 추적
    
    public Transform CandyPool; // 캔디 풀 위치

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
        MaxCandyCount = GameObject.FindGameObjectsWithTag("Box").Length - GameObject.FindGameObjectsWithTag("Locked").Length;
        UpdateCandyCountText();

        candyPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject candy = Instantiate(candyPrefab, CandyPool); // CandyPool 위치에서 생성
            candy.SetActive(false);
            candyPool.Enqueue(candy);
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        if (candyPool.Count == 0)
        {
            GameObject candy = Instantiate(candyPrefab, CandyPool); // CandyPool 위치에서 생성
            candy.SetActive(false);
            candyPool.Enqueue(candy);
        }

        GameObject objectToSpawn = candyPool.Dequeue();
        objectToSpawn.transform.SetParent(CandyPool); // CandyPool을 부모로 설정
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // 캔디의 레벨을 현재 기본 레벨로 설정합니다.
        CandyStatus status = objectToSpawn.GetComponent<CandyStatus>();
        if (status != null)
        {
            status.level = CandyStatus.baseLevel;
            status.UpdateLevelText();
        }

        // 캔디의 외형을 업데이트합니다.
        SetAppearance(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.transform.SetParent(CandyPool); // CandyPool을 부모로 설정
        objectToReturn.SetActive(false);
        candyPool.Enqueue(objectToReturn);

        // 캔디의 레벨을 baseLevel로 초기화합니다.
        CandyStatus status = objectToReturn.GetComponent<CandyStatus>();
        if (status != null)
        {
            status.level = CandyStatus.baseLevel;
            status.UpdateLevelText();
        }
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
