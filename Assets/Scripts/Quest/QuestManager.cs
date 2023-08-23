using System;
using UnityEngine;
using System.Collections.Generic;
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
    private List<int> usedAvatars = new List<int>();

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
        
        quest.InstanceQuest();
        InitializePool();
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
        quest.gameObject.SetActive(false); // 객체를 비활성화
        questPool.Enqueue(quest); // 풀에 다시 추가
        activeQuests.Remove(quest); // activeQuests 리스트에서 해당 퀘스트 제거

        CreateNewQuest(); // 새로운 퀘스트 생성
    }

    public Sprite GetRandomHumanAvatar()
    {
        int index;

        if (usedAvatars.Count == humanAvatars.Length)
        {
            usedAvatars.Clear();
        }

        do
        {
            index = Random.Range(0, humanAvatars.Length);
        } while (usedAvatars.Contains(index));

        usedAvatars.Add(index);

        return humanAvatars[index];
    }

    public int RandomCandyLevel()
    {
        return Random.Range(1, 4);
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
    
    
}
