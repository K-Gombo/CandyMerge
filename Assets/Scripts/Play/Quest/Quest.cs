using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quest : MonoBehaviour
{
    public Image humanAvatar;
    public Image requestCandy1;
    public Image requestCandy2;
    public Text rewardText;
    public Text candyCountText1;
    public Text candyCountText2;
    public RewardButton rewardButton;
    public static Quest instance;
    public Image questImage;
    public GameObject RequestCandy2;
    
    public bool isSpecialQuest = false; // 특별 퀘스트 여부 확인
    public long reward; // 보상
    private int avatarIndex = -1;


    private void Awake()
    {
        instance = this;
    }
    
    private void Start() {
        
        rewardButton.parentQuest = this;
    }
    
    public void InstanceQuest()
    {    
        QuestManager.instance.activeQuests = new List<Quest>(QuestManager.instance.maxQuests);
        for (int i = 0; i < QuestManager.instance.maxQuests; i++)
        {   
            var questObject = Instantiate(this, QuestManager.instance.questGrid);
            CreateQuest(questObject);
            // "Box" 태그를 가진 게임 오브젝트의 참조를 미리 저장
            QuestManager.instance.boxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
        }
    }
    
    
    
    public void CreateQuest(Quest quest)
{
    
    int numberOfCandyTypes;
    if (HappyLevel.instance.CurrentLevel < 4)
    {
        numberOfCandyTypes = 1;
        RequestCandy2.SetActive(false); // 1종류만 생기는 경우에는 RequestCandy2를 비활성화
    }
    else
    {
        numberOfCandyTypes = Random.Range(1, 3);
        if (numberOfCandyTypes == 1)
        {
            RequestCandy2.SetActive(false); // 1종류만 생기는 경우에는 RequestCandy2를 비활성화
        }
        else
        {
            RequestCandy2.SetActive(true); // 2종류가 생기는 경우에는 RequestCandy2를 활성화
        }
    }

    int candyLevel1 = QuestManager.instance.RandomCandyLevel();
    int candyCount1;

    if (HappyLevel.instance.CurrentLevel < 3)
    {
        candyCount1 = Random.Range(2, 5); // 2개~4개 사이
    }
    else
    {
        candyCount1 = Random.Range(3, 8); 
    }

    int candyLevel2 = -1;
    int candyCount2 = 0;

    // while (!QuestManager.instance.IsQuestValid(candyLevel1, candyCount1, candyLevel2, candyCount2))
    // {
    //     candyLevel1 = QuestManager.instance.RandomCandyLevel();
    //     if (HappyLevel.instance.CurrentLevel < 3)
    //     {
    //         candyCount1 = Random.Range(2, 5); // 2개~4개 사이
    //     }
    //     else
    //     {
    //         candyCount1 = Random.Range(3, 8); 
    //     }
    // }

    QuestManager.instance.AddQuestInfo(candyLevel1, candyCount1, candyLevel2, candyCount2);

    Sprite avatar = QuestManager.instance.GetRandomHumanAvatar(out avatarIndex);
    Sprite candySprite1 = CandyManager.instance.candySprites[candyLevel1 - 1];
    Sprite candySprite2 = null;

    reward = QuestManager.instance.candyPriceByLevel[candyLevel1] * candyCount1;

    if (numberOfCandyTypes == 2)
    {
        do
        {
            candyLevel2 = QuestManager.instance.RandomCandyLevel();
        } while (candyLevel2 == candyLevel1);

        int remainingCandyCount = QuestManager.instance.maxCandyCount - candyCount1;
        if (remainingCandyCount > 2)
        {
            candyCount2 = Random.Range(3, remainingCandyCount + 1);
            candySprite2 = CandyManager.instance.candySprites[candyLevel2 - 1];
            reward += QuestManager.instance.candyPriceByLevel[candyLevel2] * candyCount2;
        }
    }

    // 특별 퀘스트 여부를 결정
    quest.isSpecialQuest = QuestManager.instance.IsSpecialQuest();
    
    // 특별 퀘스트일 경우 보상을 3배
    if (quest.isSpecialQuest)
    {
        reward *= 3;
    }

    // 특별 퀘스트일 경우 색상 변경
    if (quest.isSpecialQuest && quest.questImage != null)
    {
        quest.questImage.color = new Color(1f, 0.8f, 1f, 1.0f);
    }
    else if (quest.questImage != null)
    {
        quest.questImage.color = Color.white;
    }

    SetupQuest(quest, avatar, candySprite1, candyCount1, candySprite2, candyCount2, QuestManager.instance.FormatGold(reward));
    QuestManager.instance.UpdateQuestCandyCount(quest);
    
    
}


    

    public void SetupQuest(Quest questObject, Sprite avatar, Sprite candySprite1, int candyCount1, Sprite candySprite2, int candyCount2, string formattedReward)
    {
        questObject.humanAvatar.sprite = avatar;
        questObject.requestCandy1.sprite = candySprite1;
        questObject.candyCountText1.text = $"0/{candyCount1}";

        if (candySprite2 != null)
        {
            questObject.requestCandy2.sprite = candySprite2;
            questObject.candyCountText2.text = $"0/{candyCount2}";
            questObject.requestCandy2.gameObject.SetActive(true);  // 2종류일 때 활성화
        }
        else
        {
            questObject.requestCandy2.sprite = null;
            questObject.candyCountText2.text = "";
            questObject.requestCandy2.gameObject.SetActive(false); // 1종류일 때 비활성화
        }

        questObject.rewardText.text = formattedReward;
        QuestManager.instance.activeQuests.Add(questObject);
    }

    
    public void UpdateRequirements()
{
    int numberOfCandyTypes;
    if (HappyLevel.instance.CurrentLevel < 4)
    {
        numberOfCandyTypes = 1;
        RequestCandy2.SetActive(false); // 1종류만 생기는 경우에는 RequestCandy2를 비활성화
    }
    else
    {
        numberOfCandyTypes = Random.Range(1, 3);
        if (numberOfCandyTypes == 1)
        {
            RequestCandy2.SetActive(false); // 1종류만 생기는 경우에는 RequestCandy2를 비활성화
        }
        else
        {
            RequestCandy2.SetActive(true); // 2종류가 생기는 경우에는 RequestCandy2를 활성화
        }
    }

    int candyLevel1 = QuestManager.instance.RandomCandyLevel();
    int candyCount1;

    if (HappyLevel.instance.CurrentLevel < 3)
    {
        candyCount1 = Random.Range(2, 5); // 2개~4개 사이
    }
    else
    {
        candyCount1 = Random.Range(3, 8); 
    }

    int candyLevel2 = -1;
    int candyCount2 = 0;

    // while (!QuestManager.instance.IsQuestValid(candyLevel1, candyCount1, candyLevel2, candyCount2))
    // {
    //     candyLevel1 = QuestManager.instance.RandomCandyLevel();
    //     if (HappyLevel.instance.CurrentLevel < 3)
    //     {
    //         candyCount1 = Random.Range(2, 5); // 2개~4개 사이
    //     }
    //     else
    //     {
    //         candyCount1 = Random.Range(3, 8); 
    //     }
    // }

    QuestManager.instance.AddQuestInfo(candyLevel1, candyCount1, candyLevel2, candyCount2);

    Sprite avatar = QuestManager.instance.GetRandomHumanAvatar(out avatarIndex);
    Sprite candySprite1 = CandyManager.instance.candySprites[candyLevel1 - 1];
    Sprite candySprite2 = null;

    long reward = QuestManager.instance.candyPriceByLevel[candyLevel1] * candyCount1;

    if (numberOfCandyTypes == 2)
    {
        do
        {
            candyLevel2 = QuestManager.instance.RandomCandyLevel();
        } while (candyLevel2 == candyLevel1);

        int remainingCandyCount = QuestManager.instance.maxCandyCount - candyCount1;
        if (remainingCandyCount > 2)
        {
            candyCount2 = Random.Range(3, remainingCandyCount + 1);
            candySprite2 = CandyManager.instance.candySprites[candyLevel2 - 1];
            reward += QuestManager.instance.candyPriceByLevel[candyLevel2] * candyCount2;
        }
    }

    // 특별 퀘스트 여부를 결정
    isSpecialQuest = QuestManager.instance.IsSpecialQuest();

    // 특별 퀘스트일 경우 보상을 3배로 함
    if (isSpecialQuest)
    {
        reward *= 3;
    }

    // 특별 퀘스트일 경우 색상 변경
    if (isSpecialQuest && questImage != null)
    {
        questImage.color = new Color(1f, 0.8f, 1f, 1.0f);
    }
    else if (questImage != null)
    {
        questImage.color = Color.white;
    }

    humanAvatar.sprite = avatar;
    requestCandy1.sprite = candySprite1;
    requestCandy2.sprite = candySprite2;

    candyCountText1.text = $"0/{candyCount1}";
    candyCountText2.text = candySprite2 != null ? $"0/{candyCount2}" : "";

    rewardText.text = QuestManager.instance.FormatGold(reward);
}

    
    public void OnQuestComplete()
    {
        if (avatarIndex >= 0)
        {
            QuestManager.instance.ReturnAvatarIndex(avatarIndex);
        }
    }
    

}
    
  
 
    
