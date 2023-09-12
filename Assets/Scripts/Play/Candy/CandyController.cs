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
    public bool isDragEnabled = true; // 드래그가 가능한지를 나타내는 변수

    
    public bool mergeLocked = false;
    public Transform draggingParentCanvas; // 드래그 중에 Candy가 소속될 Canvas

    Vector3 startPos;
    Vector3 currentPos;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        boxTransforms = new List<Transform>();
        foreach (var box in GameObject.FindGameObjectsWithTag("Box"))
        {
            boxTransforms.Add(box.transform);
        }
        
    }
    

    private void Update()
    {
        if (isMergingInProgress || !isDragEnabled) return; // isDragEnabled를 체크

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
    
    public void EnableDrag(bool enable)
    {
        isDragEnabled = enable;
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

        Transform mergeTarget = mergeLocked ? null : GetMergeTarget(hit.collider.transform); // mergeLocked 체크
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
                ES3.Save(closestBox.gameObject.name, hit.collider.GetComponent<CandyStatus>().level);
                ES3.Save(originalParent.gameObject.name, -1);
                Debug.Log("이동! : " + closestBox.gameObject.name + "\n" + originalParent.gameObject.name);
                hit.collider.transform.SetParent(closestBox);
                hit.collider.transform.position = closestBox.position;

            }
            else
            {
                Debug.Log("돌아간단다");
                if (!CandySellBox.isBox)
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
    
    public void UpdateBoxTransforms()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox");
    
        // 기존의 MixBox들을 리스트에서 제거
        boxTransforms.RemoveAll(t => t.CompareTag("MixBox"));

        // 새로 찾은 MixBox들을 리스트에 추가
        foreach (GameObject mixBox in mixBoxes)
        {
            if (mixBox != null)
            {
                boxTransforms.Add(mixBox.transform);
            }
        }
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
        // Null 체크
        if (candy1 == null || candy2 == null)
        {
            Debug.LogError("candy1 또는 candy2가 null입니다.");
            return;
        }

        if(EffectPooler.Instance == null) 
        {
            Debug.LogError("EffectPooler.Instance가 null입니다.");
            return;
        }

        // 병합 이펙트 인스턴스화
        EffectPooler.Instance.SpawnFromPool("MergeEffect", new Vector3(candy2.position.x, candy2.position.y, 0), Quaternion.identity);

        CandyStatus candyStatus1 = candy1.GetComponent<CandyStatus>();
        CandyStatus candyStatus2 = candy2.GetComponent<CandyStatus>();
        
        if (candyStatus1 == null || candyStatus2 == null)
        {
            Debug.LogError("candyStatus1 또는 candyStatus2가 null입니다.");
            return;
        }
        
        if (candyStatus1.level == candyStatus2.level)
        {
            // 캔디 레벨이 최대 레벨인지 확인
            if (candyStatus1.level >= 60 && candyStatus2.level >= 60) 
            {
                // 최대 레벨일 경우 병합을 하지 않습니다.
                return;
            }
            

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
        CandyManager.instance.AddCount();
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
    
    
    public void MoveToMixBox(Transform mixBox)
    {
        currentlyDraggingCandy.SetParent(mixBox); // MixBox로 이동
    }


  
    public void MoveToRandomBox()
    {
       
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox");
        List<Transform> candyPrefabs = new List<Transform>();

        // MixBox 내의 candyPrefab들을 찾기
        foreach (var mixBox in mixBoxes)
        {
            foreach (Transform child in mixBox.transform)
            {
                candyPrefabs.Add(child);
            }
        }

        // 비어있는 Box 태그를 가진 오브젝트를 찾기
        List<Transform> emptyBoxes = new List<Transform>();
        foreach (var box in boxTransforms)
        {
            if (box.childCount == 0)
            {
                emptyBoxes.Add(box);
            }
        }

        // candyPrefabs과 비어있는 Box가 모두 있을 경우, 랜덤한 Box로 candyPrefab을 이동
        if (candyPrefabs.Count > 0 && emptyBoxes.Count >= candyPrefabs.Count)
        {
            // 모든 candyPrefabs를 랜덤한 비어있는 Box로 이동
            foreach (Transform candyPrefab in candyPrefabs)
            {
                int randomBoxIndex = UnityEngine.Random.Range(0, emptyBoxes.Count);
                Transform randomEmptyBox = emptyBoxes[randomBoxIndex];

                candyPrefab.SetParent(randomEmptyBox);
                candyPrefab.position = randomEmptyBox.position;

                emptyBoxes.RemoveAt(randomBoxIndex); // 이미 채운 박스는 목록에서 제거
            }
        }
        else
        {
            Debug.Log("비어있는 박스 또는 이동 가능한 캔디가 부족합니다.");
        }
    }



}


