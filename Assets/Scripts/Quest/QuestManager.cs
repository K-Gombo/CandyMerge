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
    public int maxCandyCount = 15;
    public static QuestManager instance;

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
    }
    
    void Update()
    {
        foreach (Quest quest in activeQuests)
        {
            RewardButton rewardButton = quest.GetComponentInChildren<RewardButton>();
            if (rewardButton != null)
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

        return $"{displayValue:0.0}{unitChars[unitIndex]}";
    }

    public void CompleteQuest(Quest quest)
    {
        int completedIndex = activeQuests.IndexOf(quest);
        for (int i = completedIndex + 1; i < activeQuests.Count; i++)
        {
            activeQuests[i].transform.SetSiblingIndex(i - 1);
        }

        activeQuests.Remove(quest);
        quest.gameObject.SetActive(false);
        quest.CreateQuest();
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
        int count1 = 0;
        int count2 = 0;

        foreach (GameObject box in boxes)
        {
            foreach (Transform child in box.transform)
            {
                CandyStatus status = child.GetComponent<CandyStatus>();
                if (status != null && child.gameObject.activeInHierarchy)
                {
                    Sprite candySprite = CandyManager.instance.candySprites[status.level - 1];
                    if (candySprite == quest.requestCandy1.sprite)
                    {
                        count1++;
                    }
                    else if (candySprite == quest.requestCandy2.sprite)
                    {
                        count2++;
                    }
                }
            }
        }

        string[] requiredCount1Text = quest.candyCountText1.text.Split('/');
        int requiredCount1 = int.Parse(requiredCount1Text[1]);
        quest.candyCountText1.text = $"{count1}/{requiredCount1}";

        if (quest.requestCandy2.sprite != null)
        {
            string[] requiredCount2Text = quest.candyCountText2.text.Split('/');
            int requiredCount2 = int.Parse(requiredCount2Text[1]);
            quest.candyCountText2.text = $"{count2}/{requiredCount2}";
        }
        
        Debug.Log($"Count1: {count1}, Count2: {count2}");

    }
    
}
