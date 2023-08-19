using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GiftBoxController : MonoBehaviour
{
    [SerializeField] private CandyController candyController;
    public GameObject candyPrefab;
    public Transform boxTile;
    public Image giftBoxFill;
    public Text createCandyText;
    private int candiesRemaining = 0;
    private int maxCandies = 11;
    public bool autoCreateEnabled = false;
    private Coroutine autoCreateCoroutine;
    private float lastClickTime = 0f; // 마지막 클릭 시간
    private float clickCooldown = 0.3f; // 클릭 쿨타임 (초)
    public List<Transform> availableBoxes = new List<Transform>();
    public GameObject transparentObjectPrefab; // 투명한 오브젝트 프리팹

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
        if (Time.time - lastClickTime < clickCooldown) return; // 쿨타임이 아직 지나지 않았으면 리턴

        if (candiesRemaining > 0 && IsSpaceAvailableInBox())
        {
            CreateCandy();
            candiesRemaining--;
            createCandyText.text = candiesRemaining + "/" + maxCandies;
        }

        lastClickTime = Time.time; // 마지막 클릭 시간 갱신
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
        availableBoxes.Clear();

        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform child = boxTile.GetChild(i);
            if (child.childCount == 0 && child.CompareTag("Box"))
            {
                availableBoxes.Add(child);
            }
        }

        int startIndex = candyController.GetBoxIndexFromPosition(candyController.startPosition); // 드래그 중인 캔디의 시작 위치에 해당하는 인덱스

        List<int> validIndexes = new List<int>(); // 유효한 인덱스만 담을 리스트
        for (int i = 0; i < availableBoxes.Count; i++)
        {
            if (i != startIndex) // 시작 위치 인덱스와 다른 인덱스만 추가
            {
                validIndexes.Add(i);
            }
        }

        if (validIndexes.Count > 0)
        {
            int randomIndex = validIndexes[Random.Range(0, validIndexes.Count)]; // 유효한 인덱스 목록에서 랜덤 인덱스 선택

            Transform selectedBox = availableBoxes[randomIndex];
            GameObject transparentObject = TransCandyPooler.Instance.SpawnFromPool(selectedBox.position, Quaternion.identity);
            transparentObject.transform.SetParent(selectedBox);
            GameObject candy = CandyManager.instance.SpawnFromPool(transform.position, Quaternion.identity);
            candy.transform.localScale = Vector3.one; // 로컬 스케일을 1로 설정
            candy.transform.position = transform.position; // 선물상자의 위치로 설정
            StartCoroutine(MoveCandy(candy.transform, selectedBox.position, selectedBox, transparentObject));
        }
    }

    
    


    
    private IEnumerator MoveCandy(Transform candy, Vector3 targetPosition, Transform targetBox, GameObject transparentObject)
    {
        float timeElapsed = 0f;
        float duration = 0.2f; // 이동에 걸리는 시간 (1초)

        Vector3 startPosition = candy.position;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            candy.position = Vector3.Lerp(startPosition, targetPosition, t); // 선형 보간을 사용해 부드럽게 이동
            yield return null;
        }

        candy.SetParent(targetBox); // 최종 위치에 도달하면 부모를 설정
        candy.localPosition = Vector3.zero; // 로컬 위치를 0으로 설정
        candy.localScale = Vector3.one; // 로컬 크기를 1로 설정
        transparentObject.transform.SetParent(null); // 부모 관계 끊기
        TransCandyPooler.Instance.ReturnToPool(transparentObject); // 투명한 오브젝트 풀로 반환
        
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