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
    
    public bool isSpecialQuest = false; // 특별 퀘스트 여부 확인
    public long reward; // 보상
    private int avatarIndex = -1;
    
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
        int numberOfCandyTypes = Random.Range(1, 3);
        int candyLevel1 = QuestManager.instance.RandomCandyLevel();
        int candyCount1 = Random.Range(3, 10);
        Sprite avatar = QuestManager.instance.GetRandomHumanAvatar(out avatarIndex); 
        Sprite candySprite1 = CandyManager.instance.candySprites[candyLevel1 - 1];
        Sprite candySprite2 = null;
        int candyCount2 = 0;

        reward = QuestManager.instance.candyPriceByLevel[candyLevel1] * candyCount1;

        if (numberOfCandyTypes == 2)
        {
            int candyLevel2;
            do
            {
                candyLevel2 = QuestManager.instance.RandomCandyLevel();
            } while (candyLevel2 == candyLevel1);

            // 여기서 candyCount2의 최대값을 maxCandyCount - candyCount1로 지정
            candyCount2 = Random.Range(3, QuestManager.instance.maxCandyCount - candyCount1);
            candySprite2 = CandyManager.instance.candySprites[candyLevel2 - 1];
            reward += QuestManager.instance.candyPriceByLevel[candyLevel2] * candyCount2;
        }
        
        CalculateSpecialQuest(ref reward); // 특별 퀘스트 계산


      

        SetupQuest(quest, avatar, candySprite1, candyCount1, candySprite2, candyCount2, QuestManager.instance.FormatGold(reward));
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
        }
        else
        {
            questObject.requestCandy2.sprite = null;
            questObject.candyCountText2.text = "";
        }

        questObject.rewardText.text = formattedReward;
        QuestManager.instance.activeQuests.Add(questObject);
    }
    
    public void UpdateRequirements()
    {
        

        int numberOfCandyTypes = Random.Range(1, 3);
        int candyLevel1 = QuestManager.instance.RandomCandyLevel();
        int candyCount1 = Random.Range(3, 10);
        
        
        // 아바타 스프라이트 할당
        Sprite avatar = QuestManager.instance.GetRandomHumanAvatar(out avatarIndex); 
        Sprite candySprite1 = CandyManager.instance.candySprites[candyLevel1 - 1];
        Sprite candySprite2 = null;
        int candyCount2 = 0;
        
        // 해당 아바타 스프라이트를 humanAvatar 필드에 할당합니다.
        humanAvatar.sprite = avatar;

        long reward = QuestManager.instance.candyPriceByLevel[candyLevel1] * candyCount1;

        if (numberOfCandyTypes == 2)
        {
            int candyLevel2;
            do
            {
                candyLevel2 = QuestManager.instance.RandomCandyLevel();
            } while (candyLevel2 == candyLevel1);
            
            candyCount2 = Random.Range(3, QuestManager.instance.maxCandyCount - candyCount1);
            candySprite2 = CandyManager.instance.candySprites[candyLevel2 - 1];
            reward += QuestManager.instance.candyPriceByLevel[candyLevel2] * candyCount2;
        }
        CalculateSpecialQuest(ref reward);
      
        humanAvatar.sprite = avatar; // humanAvatar에 스프라이트 할당
        requestCandy1.sprite = candySprite1;
        requestCandy2.sprite = candySprite2;

        candyCountText1.text = $"0/{candyCount1}";
        candyCountText2.text = candySprite2 != null ? $"0/{candyCount2}" : "";

        rewardText.text = QuestManager.instance.FormatGold(reward); // rewardText 업데이트
    }
    
    public void CalculateSpecialQuest(ref long reward)
    {
        // QuestManager의 IsSpecialQuest 메서드를 호출하여 특별 퀘스트 여부를 판단
        isSpecialQuest = QuestManager.instance.IsSpecialQuest();

        if (isSpecialQuest)
        {
            reward *= 3; // 특별 퀘스트일 경우 보상을 3배로 함
            Debug.Log("특별퀘스트 등장!"); 
        }
    }
    
    // 퀘스트가 완료되면 호출되는 메서드
    public void OnQuestComplete()
    {
        if (avatarIndex >= 0)
        {
            QuestManager.instance.ReturnAvatarIndex(avatarIndex);
        }
    }

}
    
  
 
    
