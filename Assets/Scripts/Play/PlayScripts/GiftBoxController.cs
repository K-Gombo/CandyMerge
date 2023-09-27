using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GiftBoxController : MonoBehaviour
{
    [SerializeField] private CandyController candyController;
    public GameObject candyPrefab;
    public Transform boxTile;
    public Button giftBoxButton;  // Gift box의 버튼
    public Image giftBoxFill;
    public Transform giftBoxTransform;
    public Text createCandyText;
    public int candiesRemaining = 0;
    private int maxCandies = 10;
    public int realMaxCandies = 40;
    private Coroutine autoCreateCoroutine;
    private Coroutine passiveAutoCreateCoroutine;
    private float lastClickTime = 0f; // 마지막 클릭 시간
    private float clickCooldown = 0.3f; // 클릭 쿨타임 (초)
    public List<Transform> availableBoxes = new List<Transform>();
    public GameObject transparentObjectPrefab; // 투명한 오브젝트 프리팹
    public BoxManager boxManager; // BoxManager 참조
    public float fillTime = 10f; // 초기값 설정
    public float minimumFillTime = 1.2f;
    private bool isLocked = false; // 작동 우선순위 락
    public float passiveCreateTry = 0f; // 10동안 n번 생성
    public float maxPassiveCreateTry = 10f;
    public float luckyCreate = 0f; // 20% 확률로 2개의 캔디 생성
    public float maxLuckyCreate = 20f;
    private GameObject transparentObject;
    public bool isPassiveAutoCreateRunning = false;
    private bool delayCompleted = true;
    public static GiftBoxController instance;
    
    public GameObject equipKeyPrefab;
    public float equipKeyCreatProbability = 0f;
    public float equipKeyDoubleProbability = 0f;
    public Transform keyBox;
    public Text keyBoxCountText;
    public int keyCount;
    
    public GameObject equipKeyPool;  // EquipKey를 담을 부모 GameObject
    private List<GameObject> equipKeyPoolList;  // EquipKey 객체를 담을 리스트
    public int initialEquipKeyPoolSize = 10;  // 초기 풀 크기
    private int giftBoxClickCount = 0; 


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        StartCoroutine(FillAndCreateCandies());
        createCandyText.text = candiesRemaining + "/" + maxCandies;
        TogglePassiveAutoCreate(true);
        giftBoxTransform = GameObject.Find("GiftBox").transform;
        giftBoxButton.onClick.AddListener(OnGiftBoxClick);
        InitializeEquipKeyPool();
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
                
                if (TutorialManager.instance.isTutorialActive)
                {
                    candiesRemaining = maxCandies -1;
                }

                if (candiesRemaining < maxCandies)
                {
                    candiesRemaining++;
                }

                createCandyText.text = candiesRemaining + "/" + maxCandies;
            
                // 최종적으로 값을 검증한 후에 저장
                if (candiesRemaining <= maxCandies)
                {
                    DataController.instance.CandiesRemaining_Save();
                }
                
                
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
        return fillTime; // 현재 fillTime 값 반환                                                                                ㅡㅡㅡㅡㅡ 
    }
    public void SetFillTime(float newFillTime)
    {
        newFillTime = Mathf.Round(newFillTime * 100f) / 100f; // 소수점 둘째 자리까지만 고려
        if (newFillTime > minimumFillTime)
        {
            fillTime = newFillTime; // 새로운 값을 적용
        }
        else
        {
            fillTime = minimumFillTime; // 최소치를 넘으면 최소치로 설정
        }
    }
    
    private void InitializeEquipKeyPool()
    {
        equipKeyPoolList = new List<GameObject>();

        for (int i = 0; i < initialEquipKeyPoolSize; i++)
        {
            GameObject obj = Instantiate(equipKeyPrefab, equipKeyPool.transform);
            obj.SetActive(false);
            equipKeyPoolList.Add(obj);
        }
    }

    private GameObject GetPooledEquipKey()
    {
        foreach (GameObject obj in equipKeyPoolList)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // 풀에 사용 가능한 객체가 없을 경우 새로 생성
        GameObject newObj = Instantiate(equipKeyPrefab, equipKeyPool.transform);
        newObj.SetActive(false);
        equipKeyPoolList.Add(newObj);
        
        return newObj;
    }
    
    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(equipKeyPool.transform);
        obj.transform.position = equipKeyPool.transform.position;
    }
    

    public float GetLuckyCreate()
    {
        return luckyCreate;
    }

    public void SetLuckyCreate(float newLuckyCreate)
    {
        newLuckyCreate = Mathf.Round(newLuckyCreate * 100f) / 100f; // 소수 둘째자리에서 반올림
        luckyCreate = Mathf.Min(newLuckyCreate, maxLuckyCreate);
    }

    public int GetMaxCandies()
    {
        return maxCandies;
    }

    public void SetMaxCandies(int newMaxCandies)
    {
        if (newMaxCandies <= realMaxCandies)
        {
            maxCandies = newMaxCandies;
        }
        else
        {
            maxCandies = realMaxCandies;
        }
    }

    public float GetPassiveCreateTry()
    {
        return passiveCreateTry;
    }

    public void SetPassiveCreateTry(float newPassiveCreateTry)
    {
        newPassiveCreateTry = Mathf.Round(newPassiveCreateTry * 100f) / 100f; // 소수 둘째자리에서 반올림
        passiveCreateTry = Mathf.Min(newPassiveCreateTry, maxPassiveCreateTry);
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

                    DataController.instance.CandiesRemaining_Save();

                    isLocked = false; // 락 해제
                }

                yield return new WaitForSeconds(1f / timesPer10Seconds);
            }
        }
    }
    
    public IEnumerator PassiveAutoCreateCandy()
    {
        

        float totalTime = 10f; // 총 시간 (10초)
        while (isPassiveAutoCreateRunning) // 무한 루프로 계속 실행
        {   
            if (passiveCreateTry <= 0f)
            {
                yield return new WaitForSeconds(1f); // 1초 대기하고 다시 체크
                continue; // 이번 반복은 건너뛰고 다음 반복으로
            }

            float passiveCreateInterval = totalTime / passiveCreateTry;
            if (candiesRemaining > 0 && IsSpaceAvailableInBox())
            {
                while (isLocked) // AutoCreateCandy가 락을 해제할 때까지 대기
                {
                    yield return null;
                }
        
                CreateCandy();

                candiesRemaining--;

                createCandyText.text = candiesRemaining + "/" + maxCandies;

                DataController.instance.CandiesRemaining_Save();

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
            SoundManager.Instance.PlaySoundEffect("Merge");
            CreateCandy();
            candiesRemaining--;
            createCandyText.text = candiesRemaining + "/" + maxCandies;

            DataController.instance.CandiesRemaining_Save();

            // 튜토리얼이 활성화된 경우에만 아래 로직 실행
            if (TutorialManager.instance.isTutorialActive)
            {       Debug.Log("isTutorialActive: " + TutorialManager.instance.isTutorialActive);

                // 튜토리얼 상태가 TapGiftBox 또는 TapGiftBoxAgain인 경우 클릭 횟수를 증가
                if (TutorialManager.instance.currentState == TutorialState.TapGiftBox ||
                    TutorialManager.instance.currentState == TutorialState.TapGiftBoxAgain)
                {  

                    giftBoxClickCount++;

                    // 클릭 횟수가 2회면 튜토리얼의 다음 단계로 이동
                    if (giftBoxClickCount >= 2)
                    {   Debug.Log("나 실행된다!");
                        TutorialManager.instance.NextTutorialStep();
                        giftBoxClickCount = 0;  // 클릭 횟수 초기화
                    }
                }
            }
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
    
    private IEnumerator TransParentCandy(Transform selectedBox)
    {   
        transparentObject = TransCandyPooler.Instance.SpawnFromPool(selectedBox.position, Quaternion.identity);
        transparentObject.transform.SetParent(selectedBox);
        yield return null;
    }
    
    
    public float GetEquipLuckyCreatKeyUp()
    {   
        return equipKeyCreatProbability;
    }

    public void SetEquipLuckyCreatKeyUp(float newLuckyEquipKeyCreatProbability)
    {
        newLuckyEquipKeyCreatProbability = Mathf.Round(newLuckyEquipKeyCreatProbability * 100f) / 100f;
        equipKeyCreatProbability = newLuckyEquipKeyCreatProbability;
    }
    
    
    public void ResetEquipLuckyCreatKeyUp(EquipmentStatus equipment)
    {
        float currentEquipLuckyCreatKeyUp = GetEquipLuckyCreatKeyUp();
        float newEquipLuckyCreatKeyUp = currentEquipLuckyCreatKeyUp;
        bool skillIdExists = false;
        
        int[] targetSkillIds = { 28, 29, 30, 31 };

        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i])&& equipment.skillUnlocked[i])
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;

                // 해당 skillId의 skillPoints를 빼기
                newEquipLuckyCreatKeyUp -= equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }

        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }

        // 새로운 값을 설정
        SetEquipLuckyCreatKeyUp(newEquipLuckyCreatKeyUp);
        Debug.Log($"장비 열쇠 획득 확률 초기화: {newEquipLuckyCreatKeyUp}");
    }
    
    
    public float GetEquipKeyDoubleUp()
    {   
        return equipKeyDoubleProbability;
    }

    public void SetEquipKeyDoubleUp(float newEquipKeyDoubleUpProbability)
    {
        newEquipKeyDoubleUpProbability = Mathf.Round(newEquipKeyDoubleUpProbability * 100f) / 100f;
        equipKeyDoubleProbability = newEquipKeyDoubleUpProbability;
    }
    
    
    public void ResetEquipKeyDoubleUp(EquipmentStatus equipment)
    {
        float currentEquipKeyDoubleUp = GetEquipKeyDoubleUp();
        float newEquipKeyDoubleUp = currentEquipKeyDoubleUp;
        bool skillIdExists = false;
        
        int[] targetSkillIds = { 32, 33, 34, 35 };

        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i])&& equipment.skillUnlocked[i])
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;

                // 해당 skillId의 skillPoints를 빼기
                newEquipKeyDoubleUp -= equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }

        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }

        // 새로운 값을 설정
        SetEquipKeyDoubleUp(newEquipKeyDoubleUp);
        Debug.Log($"장비 열쇠 2개 획득 확률 초기화: {newEquipKeyDoubleUp}");
    }

    

    private void CreateCandy()
    {
        availableBoxes.Clear();

        // 튜토리얼 상태가 TapGiftBox 일 때
        if (TutorialManager.instance.isTutorialActive && TutorialManager.instance.currentState == TutorialState.TapGiftBox)
        {
            availableBoxes.Add(boxTile.GetChild(0));
            availableBoxes.Add(boxTile.GetChild(2));
        }
        // 튜토리얼 상태가 TapGiftBoxAgain 이거나 튜토리얼이 아닐 때
        else if (TutorialManager.instance.currentState == TutorialState.TapGiftBoxAgain || !TutorialManager.instance.isTutorialActive)
        {
            for (int i = 0; i < boxTile.childCount; i++)
            {
                Transform child = boxTile.GetChild(i);
                if (child.childCount == 0 && child.CompareTag("Box"))
                {
                    availableBoxes.Add(child);
                }
            }
        }

        int startIndex = candyController.GetBoxIndexFromPosition(candyController.startPosition);
            
        if (startIndex >= 0 && startIndex < availableBoxes.Count)
        {
            availableBoxes.RemoveAt(startIndex);
        }

        float randomValue = Random.Range(0f, 100f);
            
        // equipKeyPrefab을 생성할 확률
        if (randomValue < equipKeyCreatProbability)
        { 
            int numberOfKeysToCreate = Random.Range(0f, 100f) < equipKeyDoubleProbability ? 2 : 1;

            for (int i = 0; i < numberOfKeysToCreate; i++) 
            {   
                GameObject equipKey = GetPooledEquipKey();
                equipKey.transform.position = giftBoxTransform.position;
                equipKey.transform.localScale = Vector3.one;
                Vector3 vec3 = giftBoxTransform.transform.position;
                float value = Random.Range(-0.5f, 0.5f);
                vec3.x = vec3.x - value;
                equipKey.transform.position = vec3;
                equipKey.SetActive(true);

                StartCoroutine(MoveEquipKey(equipKey.transform, keyBox.position, i * 0.2f));
            
                keyCount++;
            }
            
            keyBoxCountText.text = keyCount + " / 5";
            return;
        }

        int numberOfCandiesToCreate = luckyCreate >= 100f ? 2 : (Random.Range(0f, 100f) < luckyCreate ? 2 : 1);
        if (availableBoxes.Count == 1) numberOfCandiesToCreate = 1;
        if (numberOfCandiesToCreate == 2 && availableBoxes.Count < 2) return;

        for (int n = 0; n < numberOfCandiesToCreate; n++)
        {
            Transform selectedBox = null;

            if (TutorialManager.instance.isTutorialActive && TutorialManager.instance.currentState == TutorialState.TapGiftBox)
            {
                for (int i = 0; i < availableBoxes.Count; i++)
                {
                    selectedBox = availableBoxes[i];
                    if (selectedBox.childCount == 0)
                    {
                        availableBoxes.RemoveAt(i); // 선택된 박스를 리스트에서 제거
                        break;
                    }
                }
            }
            else if (TutorialManager.instance.currentState == TutorialState.TapGiftBoxAgain || !TutorialManager.instance.isTutorialActive)
            {
                if (availableBoxes.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableBoxes.Count);
                    selectedBox = availableBoxes[randomIndex];
                    availableBoxes.RemoveAt(randomIndex);
                }
                else
                {
                    continue;
                }
            }


            if (selectedBox != null && selectedBox.childCount == 0) // 이 부분은 선택된 박스가 null이거나 이미 채워져 있다면 스킵
            {
                StartCoroutine(TransParentCandy(selectedBox));
                GameObject candy = CandyManager.instance.SpawnFromPool(giftBoxTransform.position, Quaternion.identity);
                candy.GetComponent<CandyStatus>().boxName = selectedBox.gameObject.name;
                selectedBox.GetComponent<Box>().SetCandy(candy.GetComponent<CandyStatus>().level);
                candy.transform.localScale = Vector3.one;
                candy.transform.position = transform.position;
                CandyManager.instance.AddCount();
                StartCoroutine(MoveCandy(candy.transform, selectedBox.position, selectedBox, transparentObject));
            }
        }
    }


    
    
    private IEnumerator MoveEquipKey(Transform equipKey, Vector3 targetPosition, float delay = 0f)
    {
        yield return new WaitForSeconds(delay); // 여기에 딜레이 적용

        float timeElapsed = 0f;
        float duration = 0.5f; // 원하는 지속 시간을 설정
        Vector3 startPosition = giftBoxTransform.position; // 시작 위치를 giftBoxTransform.position으로 설정
        Vector3 startScale = equipKey.localScale;
        Vector3 minScale = new Vector3(0.4f, 0.4f, 0.4f); // 최소 크기를 설정

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // 위치를 업데이트
            equipKey.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 크기를 점점 줄임
            equipKey.localScale = Vector3.Lerp(startScale, minScale, t);

            yield return null;
        }

        // 여기에서 풀로 반환
        equipKey.gameObject.SetActive(false);
        equipKey.SetParent(equipKeyPool.transform);
        equipKey.localScale = startScale;
    }
    
    private IEnumerator MoveCandy(Transform candy, Vector3 targetPosition, Transform targetBox, GameObject transparentObject)
    {
        float timeElapsed = 0f;
        float duration = 0.3f;
    
        Vector3 startPosition = giftBoxTransform.position; // 시작 위치를 GiftBox의 위치로 설정
    
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            candy.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        
        candy.SetParent(targetBox);
        candy.localPosition = Vector3.zero;
        candy.localScale = Vector3.one;
        transparentObject.transform.SetParent(null);
        TransCandyPooler.Instance.ReturnToPool(transparentObject);
        BoxManager.instance.UpdateCandyCount();
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
            if (autoCreateCoroutine == null) return;
            StopCoroutine(autoCreateCoroutine);
        }
    }
    
    public void TogglePassiveAutoCreate(bool isEnabled)
    {
        if (isEnabled)
        {
            if (!isPassiveAutoCreateRunning && delayCompleted)
            {
                // 코루틴 시작
                StartCoroutine(DelayStartPassiveAutoCreate());
            }
            else if (isPassiveAutoCreateRunning && !delayCompleted)
            {
                // 이미 실행 중이지만 딜레이가 끝나지 않았을 때
                // 현재 실행 중인 코루틴을 멈추고 새로 시작
                StopCoroutine(passiveAutoCreateCoroutine);
                StartCoroutine(DelayStartPassiveAutoCreate());
            }
        }
        else
        {
            if (isPassiveAutoCreateRunning)
            {
                // 코루틴 멈춤
                isPassiveAutoCreateRunning = false;
                StopCoroutine(passiveAutoCreateCoroutine);
            }
            // 선택적: 딜레이가 끝나지 않았다면 reset
            if (!delayCompleted)
            {
                delayCompleted = true;
            }
        }
    }

    
    IEnumerator DelayStartPassiveAutoCreate()
    {
        yield return new WaitForSeconds(0.1f); // 1초 딜레이
        isPassiveAutoCreateRunning = true;
        passiveAutoCreateCoroutine = StartCoroutine(PassiveAutoCreateCandy());
        delayCompleted = true;  // 딜레이가 끝났음을 표시
    }

    private IEnumerator DelayedAutoCreateCandy(int timesPer10Seconds)
    {
        yield return new WaitForSeconds(1f); // 1초의 딜레이
        yield return AutoCreateCandy(timesPer10Seconds); // 생성 시작
    }
    
    
    
    

}