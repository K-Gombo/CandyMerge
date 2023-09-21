using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;



public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest quest;
    public Transform questGrid;
    public Sprite[] humanAvatars;
    public int maxQuests = 4;
    public int maxCandyCount = 10;
    public static QuestManager instance;
    public CandyStatus candyStatus;
    public int poolSize = 10;
    private Queue<Quest> questPool = new Queue<Quest>();
    public Transform questPoolParent;
    public Dictionary<int, long> candyPriceByLevel = new Dictionary<int, long>();
    public Dictionary<int, long> candySellPriceByLevel = new Dictionary<int, long>();
    public List<GameObject> boxes = new List<GameObject>();
    public TextAsset CsvData { get; set; }
    public List<Quest> activeQuests;
    private Queue<int> availableAvatars = new Queue<int>();
    public HappyLevel happyLevel;
    private int specialQuestCounter = 0; // 특별 퀘스트 카운팅
    public int specialQuestProbability = 15; // 특별 퀘스트 확률 (15회 중 1회)
    public float luckyExperienceUpProbability = 0f; // 추가된 변수

    
    public Dictionary<string, int> activeQuestsInfo = new Dictionary<string, int>(); //활성화된 퀘스트 담는 딕셔너리

    [Header("[ 퀘스트버튼 공용변수 ]")]
    public float goldIncreaseRate = 0f;
    public float maxGoldIncreaseRate = 30f;

    public float equipGoldIncreaseRate = 0f;

    public float equipLuckyGoldProbability = 0;

    public float luckyGoldProbability = 0;
    public float maxLuckyGoldProbability = 40f;

    private void Awake()
    {
        instance = this;

        CsvData = Resources.Load<TextAsset>("CandyPrice");
        var csvText = CsvData.text;
        var csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 1; i < csvData.Length - 1; i++)
        {
            var line = csvData[i];
            var data = line.Split(',');
            var level = int.Parse(data[0]);
            var gold = double.Parse(data[1]);
            var sellGold = double.Parse(data[2]);
            var goldAsLong = (long)gold;
            var sellGoldAsLong = (long)sellGold;
            candyPriceByLevel[level] = goldAsLong;
            candySellPriceByLevel[level] = sellGoldAsLong;
        }

        InitializePool();
    }

    private void Start()
    {
        // 처음에 모든 인덱스를 사용 가능한 인덱스로 초기화
        for (int i = 0; i < humanAvatars.Length; i++)
        {
            availableAvatars.Enqueue(i);
        }
        quest.InstanceQuest();
        
    }
    
    void Update()
    {   
        
        foreach (Quest quest in activeQuests)
        {
            RewardButton rewardButton = quest.GetComponentInChildren<RewardButton>();
            if (rewardButton == null)
            {
                continue; // 다음 퀘스트로 넘어갑니다.
            }

            if (rewardButton.parentQuest != null)
            {
                rewardButton.UpdateButtonState();
            }
        }
    }
    

    public string FormatGold(long goldValue)
    {
        char[] unitChars = { ' ', 'a', 'b', 'c', 'd', 'e', 'f' };
        const long unit = 1000;
        int unitIndex = 0;

        float displayValue = goldValue;
        while (displayValue >= unit)
        {
            displayValue /= unit;
            unitIndex++;
        }

        // 소수점 이하가 0인 경우 소수점 없이 표시
        if (displayValue == (int)displayValue)
            return $"{displayValue:0}{unitChars[unitIndex]}";
        else
            return $"{displayValue:0.0}{unitChars[unitIndex]}";
    }


    public void CompleteQuest(Quest quest)
    {
        RewardButton rewardButton = quest.GetComponentInChildren<RewardButton>();
        if (rewardButton != null)
        {
            rewardButton.parentQuest = null; // 참조 끊기
        }

        int completedIndex = activeQuests.IndexOf(quest);
        for (int i = completedIndex + 1; i < activeQuests.Count; i++)
        {
            activeQuests[i].transform.SetSiblingIndex(i - 1);
        }

        // 특별 퀘스트의 색상을 초기화
        if (quest.questImage != null)
        {
            quest.questImage.color = Color.white;
        }
        

        quest.transform.SetParent(questPoolParent, false); // 부모를 questPoolParent로 설정
        rewardButton.GetComponent<Button>().interactable = false;
        quest.gameObject.SetActive(false); // 객체를 비활성화
        questPool.Enqueue(quest); // 풀에 다시 추가
        activeQuests.Remove(quest); // activeQuests 리스트에서 해당 퀘스트 제거

        quest.OnQuestComplete();
        CreateNewQuest(); // 새로운 퀘스트 생성

        float randomChance = Random.Range(0f, 100f);

        // 경험치 추가 로직
        if (randomChance < luckyExperienceUpProbability)
        {
            happyLevel.AddExperience(2); // 확률로 경험치가 2 추가
        }
        else
        {
            happyLevel.AddExperience(1); // 그 외에는 경험치가 1 추가
        }
        happyLevel.UpdateHappinessBar();
    }


    public Sprite GetRandomHumanAvatar(out int avatarIndex)
    {
        // 사용 가능한 인덱스가 없으면 null 반환
        if (availableAvatars.Count == 0)
        {
            avatarIndex = -1;
            return null;
        }

        // 사용 가능한 인덱스 큐에서 인덱스 가져오기
        avatarIndex = availableAvatars.Dequeue();

        return humanAvatars[avatarIndex];
    }

    public void ReturnAvatarIndex(int index)
    {
        // 사용된 인덱스 반환
        availableAvatars.Enqueue(index);
    }

    public int RandomCandyLevel()
    {
        int baseLevel = candyStatus.GetBaseLevel(); // 기본 제작 캔디 레벨 가져오기
        int randomLevel;

        if (HappyLevel.instance.CurrentLevel < 3)
        {
            randomLevel = Random.Range(baseLevel, baseLevel + 2); // n, n+1 중 하나를 랜덤하게 선택
        }
        else
        {
            randomLevel = Random.Range(baseLevel, baseLevel + 4); // n, n+1, n+2, n+3 중 하나를 랜덤하게 선택
        }

        return Mathf.Min(randomLevel, candyStatus.GetMaxCandyLevel()); // 선택된 레벨과 최대 캔디 레벨 중 작은 값 반환
    }
    
    public void UpdateQuestCandyCount(Quest quest)
    {
        int level1 = CandyManager.instance.GetLevelBySprite(quest.requestCandy1.sprite);
        int count1 = BoxManager.instance.GetCandyCountByLevel(level1);

        string[] requiredCount1Text = quest.candyCountText1.text.Split('/');
        int requiredCount1 = int.Parse(requiredCount1Text[1]);
        quest.candyCountText1.text = $"{count1}/{requiredCount1}";

        int count2 = 0; // 여기서 count2 변수를 선언하고 초기화합니다.

        if (quest.requestCandy2.sprite != null)
        {
            int level2 = CandyManager.instance.GetLevelBySprite(quest.requestCandy2.sprite);
            count2 = BoxManager.instance.GetCandyCountByLevel(level2);

            string[] requiredCount2Text = quest.candyCountText2.text.Split('/');
            int requiredCount2 = int.Parse(requiredCount2Text[1]);
            quest.candyCountText2.text = $"{count2}/{requiredCount2}";
        }
    }

    private void CreateNewQuest()
    {
        if (activeQuests.Count < maxQuests && questPool.Count > 0)
        {
            Quest newQuest = questPool.Dequeue();
            newQuest.transform.SetParent(questGrid, false);
            newQuest.gameObject.SetActive(true);

            RewardButton rewardButton = newQuest.GetComponentInChildren<RewardButton>();
            if (rewardButton != null)
            {
                rewardButton.parentQuest = newQuest;  // parentQuest 재설정
            }

            newQuest.UpdateRequirements(); // 랜덤한 요구사항 할당
            UpdateQuestCandyCount(newQuest); 
            activeQuests.Add(newQuest);
            
        }
    }



    
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {   
            Quest newQuest = Instantiate(quest, questPoolParent);
            newQuest.gameObject.SetActive(false);
            questPool.Enqueue(newQuest);
        }
    }
    
    public bool IsSpecialQuest()
    {
        bool isSpecial = Random.Range(1, specialQuestProbability + 1 - specialQuestCounter) == 1 || specialQuestCounter == 14;

        if (isSpecial) 
        {   
            Debug.Log("특별퀘스트 " + (specialQuestCounter + 1) + "번째에서 등장!"); // 몇 번째에서 특별퀘스트가 등장했는지 출력
            

            specialQuestCounter = 0; // 특별 퀘스트 카운팅 초기화
        }
        else 
        {
            
            specialQuestCounter++; // 퀘스트 카운팅 증가
        }

        if (specialQuestCounter == 0)
        {
            Debug.Log("특별 퀘스트 생성조건 초기화!"); // 특별 퀘스트 생성조건 초기화 메시지 출력
        }

        return isSpecial;
    }

    
    public void CheckAndReturnImpossibleQuests()
    {
        List<Quest> questsToRemove = new List<Quest>();
        int baseLevel = candyStatus.GetBaseLevel(); // 현재 제작되는 캔디의 기본 레벨

        foreach (Quest quest in activeQuests)
        {
            int requiredLevel1 = CandyManager.instance.GetLevelBySprite(quest.requestCandy1.sprite);
            int requiredLevel2 = quest.requestCandy2.sprite != null ? CandyManager.instance.GetLevelBySprite(quest.requestCandy2.sprite) : 0;

            // 제작되는 캔디의 기본 레벨보다 낮은 레벨의 캔디가 요구되는 경우
            if (requiredLevel1 < baseLevel || (requiredLevel2 > 0 && requiredLevel2 < baseLevel))
            {
                questsToRemove.Add(quest);
            }
        }

        foreach (Quest quest in questsToRemove)
        {
            RewardButton rewardButton = quest.GetComponentInChildren<RewardButton>();
            if (rewardButton != null)
            {
                rewardButton.parentQuest = null; // 참조 끊기
            }

            int completedIndex = activeQuests.IndexOf(quest);
            for (int i = completedIndex + 1; i < activeQuests.Count; i++)
            {
                activeQuests[i].transform.SetSiblingIndex(i - 1);
            }
            //quest.transform.SetParent(questPoolParent, false); // 부모를 questPoolParent로 설정
            rewardButton.GetComponent<Button>().interactable = false;
            quest.gameObject.SetActive(false); // 객체를 비활성화
            questPool.Enqueue(quest); // 풀에 다시 추가
            activeQuests.Remove(quest); // activeQuests 리스트에서 해당 퀘스트 제거

            quest.OnQuestComplete();
            CreateNewQuest(); // 새로운 퀘스트 생성
        }
    }
    
    public bool IsQuestValid(int candyLevel1, int candyCount1, int candyLevel2 = -1, int candyCount2 = -1)
    {
        // 캔디 정보를 문자열 키로 변환
        string questKey = $"{candyLevel1}-{candyCount1}";
        if (candyLevel2 != -1)
        {
            questKey += $"-{candyLevel2}-{candyCount2}";
        }

        // HappyLevel이 3 미만일 경우 중복 퀘스트를 허용
        if (HappyLevel.instance.CurrentLevel < 3)
        {
            return true;
        }

        // 같은 키가 이미 존재하는지 확인
        return !activeQuestsInfo.ContainsKey(questKey);
    }


    public void AddQuestInfo(int candyLevel1, int candyCount1, int candyLevel2 = -1, int candyCount2 = -1)
    {
        string questKey = $"{candyLevel1}-{candyCount1}";
        if (candyLevel2 != -1)
        {
            questKey += $"-{candyLevel2}-{candyCount2}";
        }
        activeQuestsInfo[questKey] = 1;  // 값은 무엇이든 될 수 있습니다.
    }
    
    
    public int GetCurrentQuestMaxLevel()
    {
        int maxLevel = 0;
        foreach (Quest quest in activeQuests)
        {
            int requiredLevel1 = CandyManager.instance.GetLevelBySprite(quest.requestCandy1.sprite);
            int requiredLevel2 = quest.requestCandy2.sprite != null ? CandyManager.instance.GetLevelBySprite(quest.requestCandy2.sprite) : 0;
        
            maxLevel = Mathf.Max(maxLevel, requiredLevel1, requiredLevel2);
        }
        return maxLevel;
    }
    
    
    
    public float GetEquipLuckyExperienceUp()
    {   
        return luckyExperienceUpProbability;
    }

    public void SetEquipLuckyExperienceUp(float newExperienceUpProbability)
    {
        newExperienceUpProbability = Mathf.Round(newExperienceUpProbability * 10f) / 10f;
        luckyExperienceUpProbability = newExperienceUpProbability;
    }
    
    
    public void ResetEquipLuckyExperienceUp(EquipmentStatus equipment)
    {
        float currentEquipLuckyExperienceUp = GetEquipLuckyExperienceUp();
        float newEquipLuckyExperienceUp = currentEquipLuckyExperienceUp;
        bool skillIdExists = false;
        
        int[] targetSkillIds = { 11, 12, 13, 14, 15 };

        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i])&& equipment.skillUnlocked[i])
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;

                // 해당 skillId의 skillPoints를 빼기
                newEquipLuckyExperienceUp -= equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }

        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }

        // 새로운 값을 설정
        SetEquipLuckyExperienceUp(newEquipLuckyExperienceUp);
        Debug.Log($"퀘스트 완료 경험치 2배 확률 초기화: {newEquipLuckyExperienceUp}");
    }



    // 아래 두 함수는 RewardBtn.cs 에서 옮긴 코드
    public float GetEquipLuckyGoldUp()
    {
        return equipLuckyGoldProbability;
    }
    public void SetEquipLuckyGoldUp(float newDoubleGoldProbability)
    {
        newDoubleGoldProbability = Mathf.Round(newDoubleGoldProbability * 10f) / 10f;
        equipLuckyGoldProbability = newDoubleGoldProbability;
    }


}



    

