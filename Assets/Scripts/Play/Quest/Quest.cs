using System;
using System.Collections.Generic;
using Keiwando.BigInteger;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quest : MonoBehaviour
{
    public Image humanAvatar;
    public Image requestCandy1;
    public Image requestCandy2;
    public GameObject rewardTextGameObject;
    public Text rewardText;
    public Text candyCountText1;
    public Text candyCountText2;
    public RewardButton rewardButton;
    public static Quest instance;
    public Image questImage;
    public GameObject RequestCandy2;
    
    public bool isSpecialQuest = false; // 특별 퀘스트 여부 확인
    public BigInteger reward; // 보상
    private int avatarIndex = -1;
    public GameObject Dia;
    public bool isDiaQuest = false;
    public float diaQuestProbability = 0f;
    
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
            // "Box" 태그를 가진 게임 오브젝트의 참조를 미리 저장
            QuestManager.instance.boxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
            
            CreateQuest(questObject);
            
        }
    }
    
    
    
    public float GetEquipLuckyDiaQuestUp()
    {   
        return diaQuestProbability;
    }

    public void SetEquipLuckyDiaQuestUp(float newLuckyDiaQuestProbability)
    {
        newLuckyDiaQuestProbability = Mathf.Round(newLuckyDiaQuestProbability * 100f) / 100f;
        diaQuestProbability = newLuckyDiaQuestProbability;
    }
    
    
    public void ResetEquipLuckyDiaQuestUp(EquipmentStatus equipment)
    {
        float currentEquipLuckyDiaQuestUp = GetEquipLuckyDiaQuestUp();
        float newEquipLuckyDiaQuestUp = currentEquipLuckyDiaQuestUp;
        bool skillIdExists = false;

        // skillId가 6, 7, 8, 9, 10 중에 있는지 확인
        int[] targetSkillIds = { 21, 22, 23, 24 };

        for (int i = 0; i < equipment.skillIds.Length; i++)
        {
            if (Array.Exists(targetSkillIds, element => element == equipment.skillIds[i])&& equipment.skillUnlocked[i])
            {
                // 해당 번호가 있음을 표시
                skillIdExists = true;

                // 해당 skillId의 skillPoints를 빼기
                newEquipLuckyDiaQuestUp -= equipment.skillPoints[i];
                Debug.Log($"skillId {equipment.skillIds[i]} 찾음. skillPoints는 {equipment.skillPoints[i]}");
            }
        }

        if (!skillIdExists)  // 해당 번호가 없을 경우
        {
            Debug.Log("대상 skillId 없음.");
        }

        // 새로운 값을 설정
        SetEquipLuckyDiaQuestUp(newEquipLuckyDiaQuestUp);
        Debug.Log($"다이아 퀘스트 생성 확률 초기화!: {newEquipLuckyDiaQuestUp}");
    }
    
    
    
    public void CreateQuest(Quest quest)
{
    float randProbability = Random.Range(0f, 100f);
    if (randProbability <= diaQuestProbability)
    {   
        Debug.Log($"다이아 퀘스트 생성확률은 지금 : {diaQuestProbability}");
        quest.isDiaQuest = true;
        quest.Dia.SetActive(true);
        // x-position을 15로 설정
        Vector2 pos = quest.rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 15;
        quest.rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition = pos;
    }
    else
    {
        quest.isDiaQuest = false;
        quest.Dia.SetActive(false);
        Vector3 pos = quest.rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 0;
        quest.rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    int numberOfCandyTypes;
    if (HappyLevel.instance.CurrentLevel < 4)
    {
        numberOfCandyTypes = 1;
        RequestCandy2.SetActive(false);
    }
    else
    {
        numberOfCandyTypes = Random.Range(1, 3);
        if (numberOfCandyTypes == 1)
        {
            RequestCandy2.SetActive(false);
        }
        else
        {
            RequestCandy2.SetActive(true);
        }
    }

    int candyLevel1 = QuestManager.instance.RandomCandyLevel();
    int candyCount1;
    if (HappyLevel.instance.CurrentLevel < 3)
    {
        candyCount1 = Random.Range(2, 5);
    }
    else
    {
        candyCount1 = Random.Range(3, 8);
    }

    int candyLevel2 = -1;
    int candyCount2 = 0;
    QuestManager.instance.AddQuestInfo(candyLevel1, candyCount1, candyLevel2, candyCount2);

    Sprite avatar = QuestManager.instance.GetRandomHumanAvatar(out avatarIndex);
    Sprite candySprite1 = CandyManager.instance.candySprites[candyLevel1 - 1];
    Sprite candySprite2 = null;

    // 특별 퀘스트 여부를 결정
    if (quest.isDiaQuest)
    {
        quest.isSpecialQuest = false;
    }
    else
    {
        quest.isSpecialQuest = QuestManager.instance.IsSpecialQuest();
    }
    
    
    quest.reward = QuestManager.instance.candyPriceByLevel[candyLevel1] * candyCount1;
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
                quest.reward += QuestManager.instance.candyPriceByLevel[candyLevel2] * candyCount2;
            }
        }
        
        // 보상 계산
    if (quest.isDiaQuest)
    {
        quest.reward = 5;
    }

    if (quest.isSpecialQuest)
    {
        quest.reward *= 3;
    }

    if (quest.isDiaQuest && quest.questImage != null)
    {
        quest.questImage.color = new Color(0.55f, 0.94f, 1.0f, 1.0f);
    }
    else if(quest.isSpecialQuest)
    {
        quest.questImage.color = new Color(1f, 0.8f, 1f, 1.0f);
    }
    else
    {
        quest.questImage.color = Color.white;
    }
    
    if (quest.isDiaQuest)
    {
        quest.isDiaQuest = true; 
    }
    else
    {
        quest.isDiaQuest = false;
    }
    Debug.Log($"Before Special Quest reward: {quest.reward}, isSpecialQuest: {quest.isSpecialQuest}");
    SetupQuest(quest, avatar, candySprite1, candyCount1, candySprite2, candyCount2);
    Debug.Log($"After Special Quest reward: {quest.reward}");
    QuestManager.instance.UpdateQuestCandyCount(quest);
}


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
    

    public void SetupQuest(Quest questObject, Sprite avatar, Sprite candySprite1, int candyCount1, Sprite candySprite2, int candyCount2)
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

        questObject.rewardText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(questObject.reward.ToString());
        QuestManager.instance.activeQuests.Add(questObject);
    }

    
    public void UpdateRequirements()
{
    float randProbability = Random.Range(0f, 100f);
    if (randProbability <= diaQuestProbability)
    {   Debug.Log($"다이아 퀘스트 생성확률은 지금 : {diaQuestProbability}");
        isDiaQuest = true;
        Vector3 pos = rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 15;
        rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        Dia.SetActive(true);
    }
    else
    {
        isDiaQuest = false;
        Vector3 pos = rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 0;
        rewardTextGameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        Dia.SetActive(false);
    }

    int numberOfCandyTypes;
    if (HappyLevel.instance.CurrentLevel < 4)
    {
        numberOfCandyTypes = 1;
        RequestCandy2.SetActive(false);
    }
    else
    {
        numberOfCandyTypes = Random.Range(1, 3);
        if (numberOfCandyTypes == 1)
        {
            RequestCandy2.SetActive(false);
        }
        else
        {
            RequestCandy2.SetActive(true);
        }
    }

    int candyLevel1 = QuestManager.instance.RandomCandyLevel();
    int candyCount1;
    if (HappyLevel.instance.CurrentLevel < 3)
    {
        candyCount1 = Random.Range(2, 5);
    }
    else
    {
        candyCount1 = Random.Range(3, 8);
    }

    int candyLevel2 = -1;
    int candyCount2 = 0;
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
        
    if (isDiaQuest)
    {
        reward = 5;
    }
    
    // 특별 퀘스트 여부를 결정
    if (isDiaQuest)
    {
        isSpecialQuest = false;
    }
    else
    {
        isSpecialQuest = QuestManager.instance.IsSpecialQuest();
    }
    
    if (isSpecialQuest)
    {
        reward *= 3;
    }

    if (isDiaQuest)
    {
        if (questImage != null)
        {
            questImage.color = new Color(0.55f, 0.94f, 1.0f, 1.0f);
        }
    }
    else
    {
        if (questImage != null)
        {
            if (isSpecialQuest)
            {
                questImage.color = new Color(1f, 0.8f, 1f, 1.0f);
            }
            else
            {
                questImage.color = Color.white;
            }
        }
    }

    humanAvatar.sprite = avatar;
    requestCandy1.sprite = candySprite1;
    requestCandy2.sprite = candySprite2;
    candyCountText1.text = $"0/{candyCount1}";
    candyCountText2.text = candySprite2 != null ? $"0/{candyCount2}" : "";
    rewardText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(reward.ToString());
}


    
    public void OnQuestComplete()
    {
        if (avatarIndex >= 0)
        {
            QuestManager.instance.ReturnAvatarIndex(avatarIndex);
        }
    }
    
    
    
    

}
    
  
 
    
