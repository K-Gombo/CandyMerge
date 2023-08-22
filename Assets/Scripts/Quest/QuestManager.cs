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
    public Button rewardBtn; // RewardBtn을 할당하기 위한 공개 참조
    public static QuestManager instance; // 인스턴스 변수 선언

    // 레벨과 가격을 저장할 딕셔너리 생성
    public Dictionary<int, long> candyPriceByLevel = new Dictionary<int, long>();

    public List<GameObject> boxes = new List<GameObject>(); // "Box" 태그를 가진 게임 오브젝트의 참조를 저장하는 리스트

    public TextAsset CsvData { get; set; }

    public List<Quest> activeQuests;
    private List<int> usedAvatars = new List<int>(); // 이미 사용된 아바타의 인덱스를 저장

    private void Awake()
    {
        instance = this;
        
        CsvData = Resources.Load<TextAsset>("CandyPrice");
        var csvText = CsvData.text;
        var csvData = csvText.Split(new[] { "\r\n", "\r", "\n",  }, StringSplitOptions.None);
    
        
        for (int i = 1; i < csvData.Length-1; i++)
        {
            var line = csvData[i];
            var data = line.Split(','); // 쉼표로 데이터 분리
            
            var level = int.Parse(data[0]); // 레벨 값 추출
            var gold = double.Parse(data[1]); // 가격 값 추출
            var goldAsLong = (long)gold;
            // 딕셔너리에 저장
            candyPriceByLevel[level] = goldAsLong;
        }
        
        quest.InstanceQuest();
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

        // 모든 아바타가 사용된 경우 사용 기록을 초기화
        if (usedAvatars.Count == humanAvatars.Length)
        {
            usedAvatars.Clear();
        }

        // 겹치지 않는 인덱스를 찾을 때까지 반복
        do
        {
            index = Random.Range(0, humanAvatars.Length);
        } while (usedAvatars.Contains(index));

        usedAvatars.Add(index); // 선택한 인덱스를 사용 기록에 추가

        return humanAvatars[index];
    }
    
    

    public int RandomCandyLevel()
    {
        return Random.Range(1, 4); // 예: 5레벨부터 10레벨까지만 생성
    }
    
    
    public void UpdateQuestCandyCount(Quest quest, Sprite requestCandy1, int requiredCount1, Sprite requestCandy2, int requiredCount2)
    {
        int count1 = 0;
        int count2 = 0;

        // "Box" 태그를 가진 게임 오브젝트 내부의 캔디를 탐색
        foreach (GameObject box in boxes)
        {
            foreach (Transform child in box.transform)
            {
                CandyStatus status = child.GetComponent<CandyStatus>();
                if (status != null && child.gameObject.activeInHierarchy)
                {
                    Sprite candySprite = CandyManager.instance.candySprites[status.level - 1];
                    if (candySprite == requestCandy1)
                    {
                        count1++;
                    }
                    else if (candySprite == requestCandy2)
                    {
                        count2++;
                    }
                }
            }
        }

        // 퀘스트의 candyCountText 업데이트
        quest.candyCountText1.text = $"{count1}/{requiredCount1}";
        if (requestCandy2 != null)
        {
            quest.candyCountText2.text = $"{count2}/{requiredCount2}";
        }
    }
    
    public void CheckQuestCompletion(Quest quest, Sprite requestCandy1, int requiredCount1, Sprite requestCandy2, int requiredCount2)
    {
        // 캔디 카운트 업데이트
        UpdateQuestCandyCount(quest, requestCandy1, requiredCount1, requestCandy2, requiredCount2);

        // 완료 조건 검사
        string[] countText1 = quest.candyCountText1.text.Split('/');
        int currentCount1 = int.Parse(countText1[0]);
        int currentCount2 = 0;

        if (requestCandy2 != null)
        {
            string[] countText2 = quest.candyCountText2.text.Split('/');
            currentCount2 = int.Parse(countText2[0]);
        }

        // 조건 충족 확인
        if (currentCount1 >= requiredCount1 && (requestCandy2 == null || currentCount2 >= requiredCount2))
        {   
          
            // 보상 지급
            int reward = int.Parse(quest.rewardText.text); // 문자열에서 보상 양 읽기
            CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();
            currencyManager.AddCurrency("Gold", reward);

            // 퀘스트 완료 처리
            Debug.Log("퀘스트 완료! 보상 지급!");
            CompleteQuest(quest);
        }
    }
    

    
    
    
}
