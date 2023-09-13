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
    public List<GameObject> boxes; // "Box" 태그를 가진 게임 오브젝트의 참조를 저장하는 리스트
    public Transform CandyPool; // 캔디 풀 위치
    
    public BoxManager boxManager; // BoxManager 참조

    private void Awake()
    {
        candyPool = new Queue<GameObject>();

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
        boxes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Box")); // "Box" 태그를 가진 게임 오브젝트의 참조를 저장
        MaxCandyCount = GameObject.FindGameObjectsWithTag("Box").Length - GameObject.FindGameObjectsWithTag("Locked").Length;
        UpdateCandyCountText();
        
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
        status.NullSaveCandy();
        if (status != null)
        {
            status.level = CandyStatus.baseLevel;
            status.UpdateLevelText();
        }
    
        BoxManager.instance.UpdateCandyCount();
        
        // 기존의 캔디가 사라지므로 CandyDestroyed() 메서드 호출
        CandyDestroyed();
    }
    


    public void AddCount()
    {
        currentCandyCount++;
        UpdateCandyCountText();
    }

    public void SetAppearance(GameObject candy)
    {
        SpriteRenderer renderer = candy.GetComponent<SpriteRenderer>();
        CandyStatus status = candy.GetComponent<CandyStatus>();
        int level = status.level;

        if (level > 0 && level <= candySprites.Length) // level이 배열의 크기를 벗어나지 않도록 체크
        {
            renderer.sprite = candySprites[level - 1];
        }
        else if (level > candySprites.Length) // level이 배열의 크기를 벗어났을 때의 처리
        {
            Debug.Log($"캔디의 최대레벨입니다.");
        }

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
        Debug.Log($"보유 캔디 증가!:{MaxCandyCount}");
        UpdateCandyCountText();
    }

    private void UpdateCandyCountText()
    {
        int totalCandyCountInBoxes = BoxManager.instance.GetCurrentTotalCandyCount(); // BoxManager를 참조해서 총 캔디 개수를 가져옴
        if (totalCandyCountInBoxes == 1)
        {
            candyCountText.text = $"{currentCandyCount}/{MaxCandyCount}";
        }
        else
        {
            candyCountText.text = $"{currentCandyCount}/{MaxCandyCount} "; // 그 외의 경우, 괄호와 괄호 안의 숫자를 표시
        }
    }
    
    public int GetLevelBySprite(Sprite sprite)
    {
        for (int i = 0; i < candySprites.Length; i++)
        {
            if (candySprites[i] == sprite)
            {
                return i + 1; // 레벨은 1부터 시작하므로 인덱스에 1을 더합니다.
            }
        }
        return -1; // 일치하는 스프라이트가 없을 경우 -1 반환
    }
    
    
}
