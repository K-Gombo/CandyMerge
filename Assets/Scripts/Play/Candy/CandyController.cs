using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;



public class CandyController : MonoBehaviour
{
    [SerializeField] private GiftBoxController giftBoxController;
    RaycastHit2D hit;
    public Vector3 startPosition;
    private Transform originalParent; // 원래 있던 박스를 저장하기 위한 변수
    private List<Transform> boxTransforms;
    private int originalSortingOrder;
    private Coroutine autoMergeCoroutine;
    private bool isAutoMergeEnabled = false;
    private bool isMergingInProgress = false;
    private Transform currentlyDraggingCandy; // 현재 드래그 중인 캔디
    public GameObject mergeEffectPrefab; 
    private int draggedBoxIndex = -1;// 병합 이펙트 프리팹
    public BoxManager boxManager; // BoxManager 참조
    private Vector3 mouseDownPosition; // 마우스 버튼을 누를 때의 위치
    
    private WaitForSeconds passiveDelay;
    public float passiveWaiting = 0f;  // 
    public float maxPassiveWating = 10f;
    private const float fixedTime = 10f; // 고정된 10초 시간
    
    public bool mergeLocked = false;
    
    public Transform draggingParentCanvas; // 드래그 중에 Candy가 소속될 Canvas

    Vector3 startPos;
    Vector3 currentPos;

    private void Start()
    {
        boxTransforms = new List<Transform>();
        foreach (var box in GameObject.FindGameObjectsWithTag("Box"))
        {
            boxTransforms.Add(box.transform);
        }

        UpdatePassiveWaiting(passiveWaiting);  // 초기값 설정
        Invoke("StartAutoMerge", 0.1f);
        

    }
    
    void StartAutoMerge()
    {   Debug.Log("Initial passiveDelay: " + passiveDelay);
        StartCoroutine(PassiveAutoMerge());
    }



    private void Update()
    {
        if (isMergingInProgress) return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyDraggingCandy == null)
            {
                hit = Physics2D.Raycast(worldPoint, transform.forward, Mathf.Infinity);
                if (hit.collider != null && hit.collider.CompareTag("Candy"))
                {
                    StartDraggingCandy();
                }
            }
        }

        if (currentlyDraggingCandy != null)
        {
            if (Input.GetMouseButton(0))
            {
                currentlyDraggingCandy.position = new Vector3(worldPoint.x, worldPoint.y, 90);
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopDraggingCandy();
            }
        }
    }


    private void StartDraggingCandy()
    {
        startPosition = hit.collider.transform.position;
        originalParent = hit.collider.transform.parent;
        hit.collider.transform.SetParent(draggingParentCanvas); // 드래그 중에는 Canvas를 부모로 설정
        originalSortingOrder = hit.collider.GetComponent<SpriteRenderer>().sortingOrder;
        hit.collider.GetComponent<SpriteRenderer>().sortingOrder = 5;
        currentlyDraggingCandy = hit.collider.transform;
        draggedBoxIndex = GetBoxIndexFromPosition(startPosition);
    }


private void StopDraggingCandy()
{
    hit.collider.GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrder;
    Transform mergeTarget = GetMergeTarget(hit.collider.transform);
    Transform closestBox = FindClosestEmptyBox(hit.collider.transform);
    float thresholdDistance = 0.5f;

    if (mergeTarget != null && hit.collider.GetComponent<CandyStatus>().level ==
        mergeTarget.GetComponent<CandyStatus>().level)
    {
        MergeCandies(hit.collider.transform, mergeTarget);
        hit.collider.transform.position = startPosition;
    }
    else
    {
        float distanceToBox = closestBox != null
            ? Vector3.Distance(hit.collider.transform.position, closestBox.position)
            : Mathf.Infinity;

        if (mergeTarget == null && distanceToBox < thresholdDistance)
        {
            hit.collider.transform.SetParent(closestBox);
            hit.collider.transform.position = closestBox.position;
        }
        else
        {
            ReturnToOriginalBox(hit.collider.transform);
        }
    }

    startPosition = Vector3.zero;
    draggedBoxIndex = -1;
    currentlyDraggingCandy = null;
}

   
     public int GetBoxIndexFromPosition(Vector3 position)
     {
         for (int i = 0; i < giftBoxController.availableBoxes.Count; i++)
         {
             if (i == draggedBoxIndex) continue; // 드래그 중인 박스 인덱스 건너뜀
             if (giftBoxController.availableBoxes[i].position == position)
             {
                 return i;
             }
         }
         return -1;
     }
     
     

    private Transform FindClosestEmptyBox(Transform candy)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = candy.position;

        foreach (Transform t in boxTransforms)
        {
            if (t.childCount == 0)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }

        return tMin;
    }

    private Transform GetMergeTarget(Transform candy)
    {
        foreach (Transform t in boxTransforms)
        {
            if (t.childCount > 0 && t.GetChild(0).name == candy.name)
            {
                float dist = Vector3.Distance(t.position, candy.position);
                if (dist < 0.2f)
                {
                    return t.GetChild(0);
                }
            }
        }

        return null;
    }

    private void MergeCandies(Transform candy1, Transform candy2)
    {   
        
        if (mergeLocked)
        {   
            Transform originalParent1 = candy1.parent;
            Transform originalParent2 = candy2.parent;

            // 부모에서 분리
            candy1.SetParent(null);
            candy2.SetParent(null);

            // 시작 위치로 이동
            candy1.position = startPosition;
            candy2.position = startPosition;

            // 원래의 부모로 복구
            candy1.SetParent(originalParent1);
            candy2.SetParent(originalParent2);
    
            return;
        }

        
        CandyStatus candyStatus1 = candy1.GetComponent<CandyStatus>();
        CandyStatus candyStatus2 = candy2.GetComponent<CandyStatus>();
        
        if (candyStatus1.level == candyStatus2.level)
        {
            // 캔디 레벨이 최대 레벨인지 확인
            if (candyStatus1.level >= 60) 
            {
                // 최대 레벨일 경우 병합을 하지 않습니다.
                return;
            }

            // 병합 이펙트 인스턴스화
            EffectPooler.Instance.SpawnFromPool("MergeEffect", new Vector3(candy2.position.x, candy2.position.y, 0), Quaternion.identity);

            candyStatus2.level++;
            CandyManager.instance.SetAppearance(candy2.gameObject); // 이미지를 먼저 바꾸기
            candy2.position = candy2.parent.position;

            candy2.parent.GetComponent<Box>().SetCandy(candyStatus2.level);

            boxTransforms.Remove(candy1);
            CandyManager.instance.CandyDestroyed();
            CandyManager.instance.ReturnToPool(candy1.gameObject); // 캔디 반환
    
            // 새로운 이미지에 대한 Scale 변경 코루틴 시작
            StartCoroutine(AnimateScale(candy2));
        }
        else
        {
            candy1.position = startPosition;
        }
    
        BoxManager.instance.UpdateCandyCount();
    }
    
    private IEnumerator AnimateScale(Transform candy)
    {
       
        isMergingInProgress = true; // 병합 중임을 표시
       
        Vector3 originalScale = candy.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        float duration = 0.2f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
       
                candy.localScale = Vector3.Lerp(originalScale, targetScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            candy.localScale = Vector3.Lerp(targetScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        candy.localScale = originalScale;

        isMergingInProgress = false; // 병합 중이 아님을 표시
        
    }

    private void ReturnToOriginalBox(Transform candy)
    {
        candy.transform.SetParent(originalParent); // 원래 부모 박스로 설정
        candy.transform.position = startPosition;
       
    }


    private IEnumerator AutoMergeCandies(int timesPerNSeconds, int n)
{
    while (true)
    {
        if (isMergingInProgress)
        {
            yield return null; // 병합 중일 경우 대기
            continue;
        }

        if (isAutoMergeEnabled || isMergingInProgress )
        {
            for (int i = 0; i < timesPerNSeconds; i++)
            {
                for (int targetLevel = 1; targetLevel <= 60; targetLevel++)
                {
                    Transform lowestLevelCandy = null;
                    Transform mergeTarget = null;
                    float closestDistance = float.MaxValue;

                    foreach (Transform box in boxTransforms)
                    {
                        if (box.childCount > 0 && box.GetChild(0) != currentlyDraggingCandy)
                        {
                            CandyStatus candyStatus = box.GetChild(0).GetComponent<CandyStatus>();
                            if (candyStatus != null && candyStatus.level == targetLevel)
                            {
                                if (lowestLevelCandy == null)
                                {
                                    lowestLevelCandy = box.GetChild(0);
                                }
                                else
                                {
                                    float distance = Vector3.Distance(lowestLevelCandy.position,
                                        box.GetChild(0).position);
                                    if (distance < closestDistance)
                                    {
                                        closestDistance = distance;
                                        mergeTarget = box.GetChild(0);
                                    }
                                }
                            }
                        }
                    }

                    if (lowestLevelCandy != null && mergeTarget != null)
                    {
                        isMergingInProgress = true;

                        float duration = 0.03f;
                        float elapsedTime = 0f;
                        Vector3 startPosition = lowestLevelCandy.position;
                        Vector3 endPosition = mergeTarget.position;

                        while (elapsedTime < duration)
                        {
                            float t = elapsedTime / duration;
                            lowestLevelCandy.position = Vector3.Lerp(startPosition, endPosition, t);
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }

                        MergeCandies(lowestLevelCandy, mergeTarget);
                        isMergingInProgress = false;

                        if (!isAutoMergeEnabled)
                        {
                            yield break; // 자동 병합이 비활성화된 경우 코루틴 종료
                        }
                        break;
                    }
                }

                yield return new WaitForSeconds((float)n / timesPerNSeconds);
            }
        }
        else
        {
            yield return null; // 자동 병합이 활성화되지 않았을 경우 대기
        }
    }
}

private IEnumerator PassiveAutoMerge()
{
    
    while (true)
    {
        if (isMergingInProgress || mergeLocked)
        {
            yield return null; // 병합 중일 경우 대기
            continue;
        }

        for (int targetLevel = 1; targetLevel <= 60; targetLevel++)
        {
            Transform lowestLevelCandy = null;
            Transform mergeTarget = null;
            float closestDistance = float.MaxValue;

            foreach (Transform box in boxTransforms)
            {
                if (box.childCount > 0 && box.GetChild(0) != currentlyDraggingCandy)
                {
                    CandyStatus candyStatus = box.GetChild(0).GetComponent<CandyStatus>();
                    if (candyStatus != null && candyStatus.level == targetLevel)
                    {
                        if (lowestLevelCandy == null)
                        {
                            lowestLevelCandy = box.GetChild(0);
                        }
                        else
                        {
                            float distance = Vector3.Distance(lowestLevelCandy.position,
                                box.GetChild(0).position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                mergeTarget = box.GetChild(0);
                            }
                        }
                    }
                }
            }

            if (lowestLevelCandy != null && mergeTarget != null)
            {   Debug.Log("Current passiveDelay time: " + passiveDelay);
                isMergingInProgress = true;
                MergeCandies(lowestLevelCandy, mergeTarget);
                isMergingInProgress = false;
                yield return passiveDelay;  // 병합이 성공적으로 이루어진 후 1초의 딜레이를 준다.
                break; // 병합이 일어났으면 루프를 빠져나옵니다.
            }
        }
        yield return null; // 병합이 일어나지 않았으면 다음 프레임을 기다립니다.
    }
}

   
 
    
   

    public void ToggleFastAutoMerge(bool isEnabled)
    {
                if (autoMergeCoroutine != null && !isMergingInProgress)
                {
                    StopCoroutine(autoMergeCoroutine);
                }

                isAutoMergeEnabled = isEnabled;

                if (isEnabled)
                {
                    autoMergeCoroutine = StartCoroutine(DelayedAutoMerge(1, 1));
                }
    }

    private IEnumerator DelayedAutoMerge(int timesPerNSeconds, int n)
    {
            yield return new WaitForSeconds(1f);
            yield return AutoMergeCandies(timesPerNSeconds, n);
            
    }
    
    public void UpdatePassiveWaiting(float newWaitingTime)
    {
        passiveWaiting = newWaitingTime;
        float calculatedDelay = fixedTime / newWaitingTime;
        passiveDelay = new WaitForSeconds(calculatedDelay);  // 변경된 부분: 계산된 딜레이 시간을 passiveDelay에 저장
    }

    public float GetPassiveWating()
    {
        return passiveWaiting;
    }

    public void SetPassiveWating(float newPassiveWating)
    {
        newPassiveWating = Mathf.Round(newPassiveWating * 10f) / 10f; // 소수 둘째자리에서 반올림
        passiveWaiting = Mathf.Min(newPassiveWating, maxPassiveWating);
    }


    
  
    
    
    
}


