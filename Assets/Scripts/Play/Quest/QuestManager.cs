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
    public int poolSize = 10;
    private Queue<Quest> questPool = new Queue<Quest>();
    public Transform questPoolParent;

    public Dictionary<int, long> candyPriceByLevel = new Dictionary<int, long>();
    public List<GameObject> boxes = new List<GameObject>();
    public TextAsset CsvData { get; set; }
    public List<Quest> activeQuests;
    private Queue<int> availableAvatars = new Queue<int>();
    public HappyLevel happyLevel;
    private int specialQuestCounter = 0; // 특별 퀘스트 카운팅
    public int specialQuestProbability = 15; // 특별 퀘스트 확률 (15회 중 1회)
    

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
            var goldAsLong = (long)gold;
            candyPriceByLevel[level] = goldAsLong;
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

        quest.transform.SetParent(questPoolParent, false); // 부모를 questPoolParent로 설정
        rewardButton.GetComponent<Button>().interactable = false;
        quest.gameObject.SetActive(false); // 객체를 비활성화
        questPool.Enqueue(quest); // 풀에 다시 추가
        activeQuests.Remove(quest); // activeQuests 리스트에서 해당 퀘스트 제거
        
        quest.OnQuestComplete();
        CreateNewQuest(); // 새로운 퀘스트 생성
        
        happyLevel.AddExperience(1);
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
        int maxLevel = BoxManager.instance.GetMaxCandyLevel(); // BoxManager에서 최대 레벨을 가져옵니다.
        int minLevel = Math.Max(maxLevel - 2, 1); // 최대 레벨보다 2 레벨 낮은 값과 2 중에서 더 큰 값을 최소 레벨로 설정합니다.
        return Random.Range(minLevel, maxLevel + 3); // n+2, n+1, n, n-1, n-2 중 하나 선택
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

            newQuest.UpdateRequirements(); // 랜덤한 요구사항 할당
        
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
        int baseLevel = CandyStatus.baseLevel;

        foreach (Quest quest in activeQuests)
        {
            int requiredLevel1 = CandyManager.instance.GetLevelBySprite(quest.requestCandy1.sprite);
            int requiredLevel2 = quest.requestCandy2.sprite != null ? CandyManager.instance.GetLevelBySprite(quest.requestCandy2.sprite) : 0;

            int availableCount1 = BoxManager.instance.GetCandyCountByLevel(requiredLevel1);
            int availableCount2 = requiredLevel2 > 0 ? BoxManager.instance.GetCandyCountByLevel(requiredLevel2) : 0;

            Debug.Log($"Available Count 1: {availableCount1}, Available Count 2: {availableCount2}");
            Debug.Log($"Required Level 1: {requiredLevel1}, Required Level 2: {requiredLevel2}");
            Debug.Log($"Base Level: {baseLevel}");

            if ((availableCount1 == 0 && requiredLevel1 < baseLevel) || (availableCount2 == 0 && requiredLevel2 < baseLevel))
            {
                questsToRemove.Add(quest);
                Debug.Log("Quest added to remove list");
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

            quest.transform.SetParent(questPoolParent, false); // 부모를 questPoolParent로 설정
            rewardButton.GetComponent<Button>().interactable = false;
            quest.gameObject.SetActive(false); // 객체를 비활성화
            questPool.Enqueue(quest); // 풀에 다시 추가
            activeQuests.Remove(quest); // activeQuests 리스트에서 해당 퀘스트 제거

            quest.OnQuestComplete();
            CreateNewQuest(); // 새로운 퀘스트 생성
        }
    }


}



    

