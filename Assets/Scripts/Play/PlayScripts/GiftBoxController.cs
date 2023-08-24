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
    private Coroutine autoCreateCoroutine;
    private float lastClickTime = 0f; // 마지막 클릭 시간
    private float clickCooldown = 0.3f; // 클릭 쿨타임 (초)
    public List<Transform> availableBoxes = new List<Transform>();
    public GameObject transparentObjectPrefab; // 투명한 오브젝트 프리팹
    public BoxManager boxManager; // BoxManager 참조
    private float fillTime = 0.5f; // 초기값 설정
    private bool isLocked = false; // 작동 우선순위 락
    private float passiveCreateTry = 2.4f; // 10동안 n번 생성 
    private float luckyCreate = 100f; // 20% 확률로 2개의 캔디 생성

    private void Start()
    {
        StartCoroutine(FillAndCreateCandies());
        createCandyText.text = candiesRemaining + "/" + maxCandies;
        // 새로운 코루틴 호출
        StartCoroutine(PassiveAutoCreateCandy());
    }

    private IEnumerator FillAndCreateCandies()
    {
        while (true)
        {
            if (candiesRemaining < maxCandies)
            {
                float timeElapsed = 0f;

                while (timeElapsed < fillTime)
                {
                    timeElapsed += Time.deltaTime;
                    giftBoxFill.fillAmount = timeElapsed / fillTime;
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
    
    public float GetFillTime()
    {
        return fillTime; // 현재 fillTime 값 반환
    }
    public void SetFillTime(float newFillTime)
    {
        fillTime = newFillTime; // 새로운 값을 적용
    }
    
  
    private IEnumerator AutoCreateCandy(int timesPer10Seconds)
    {
        while (true)
        {
            for (int i = 0; i < timesPer10Seconds; i++)
            {
                if (candiesRemaining > 0 && IsSpaceAvailableInBox())
                {
                    isLocked = true; // 락 설정

                    CreateCandy();
                    candiesRemaining--;
                    createCandyText.text = candiesRemaining + "/" + maxCandies;

                    isLocked = false; // 락 해제
                }

                yield return new WaitForSeconds(1f / timesPer10Seconds);
            }
        }
    }
    
    private IEnumerator PassiveAutoCreateCandy()
    {
        float totalTime = 10f; // 총 시간 (10초)
        float passiveCreateInterval = totalTime / passiveCreateTry; 
        while (true) // 무한 루프로 계속 실행
        {
            if (candiesRemaining > 0 && IsSpaceAvailableInBox())
            {
                while (isLocked) // AutoCreateCandy가 락을 해제할 때까지 대기
                {
                    yield return null;
                }

                isLocked = true; // 락 설정

                CreateCandy();
                candiesRemaining--;
                createCandyText.text = candiesRemaining + "/" + maxCandies;

                isLocked = false; // 락 해제
            }

            yield return new WaitForSeconds(passiveCreateInterval); // passiveCreateInterval의 값에 따라 대기 시간 조절
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

        // 드래그 중인 캔디의 시작 위치에 해당하는 인덱스
        int startIndex = candyController.GetBoxIndexFromPosition(candyController.startPosition);

        // 시작 위치의 박스를 사용 가능한 목록에서 제거
        if (startIndex >= 0 && startIndex < availableBoxes.Count)
        {
            availableBoxes.RemoveAt(startIndex);
        }

        int numberOfCandiesToCreate = luckyCreate >= 100f ? 2 : (Random.Range(0f, 100f) < luckyCreate ? 2 : 1);

        // availableBoxes.Count가 1개라면 1개만 생성
        if (availableBoxes.Count == 1) numberOfCandiesToCreate = 1;

        // 2개의 캔디를 생성해야 하지만 사용 가능한 박스가 2개 미만인 경우 반환
        if (numberOfCandiesToCreate == 2 && availableBoxes.Count < 2) return;

        for (int n = 0; n < numberOfCandiesToCreate; n++)
        {
            int randomIndex = Random.Range(0, availableBoxes.Count);
            Transform selectedBox = availableBoxes[randomIndex];
            availableBoxes.RemoveAt(randomIndex); // 선택한 박스를 사용 가능한 목록에서 제거
            GameObject transparentObject = TransCandyPooler.Instance.SpawnFromPool(selectedBox.position, Quaternion.identity);
            transparentObject.transform.SetParent(selectedBox);
            GameObject candy = CandyManager.instance.SpawnFromPool(transform.position, Quaternion.identity);
            candy.transform.localScale = Vector3.one;
            candy.transform.position = transform.position;
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
        BoxManager.instance.UpdateCandyCount(); // 여기에서 업데이트 호출
        
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