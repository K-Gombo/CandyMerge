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
    
    public void InstanceQuest()
    {
        QuestManager.instance.activeQuests = new List<Quest>(QuestManager.instance.maxQuests);
        for (int i = 0; i < QuestManager.instance.maxQuests; i++)
        {
            CreateQuest();
            // "Box" 태그를 가진 게임 오브젝트의 참조를 미리 저장
            QuestManager.instance.boxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
        }
    }
    
    public void CreateQuest()
    {
        int numberOfCandyTypes = Random.Range(1, 3);
        int candyLevel1 = QuestManager.instance.RandomCandyLevel();
        int candyCount1 = Random.Range(3, 10);

        Sprite avatar = QuestManager.instance.GetRandomHumanAvatar();
        Sprite candySprite1 = CandyManager.instance.candySprites[candyLevel1 - 1];
        Sprite candySprite2 = null;
        int candyCount2 = 0;

        long reward = QuestManager.instance.candyPriceByLevel[candyLevel1] * candyCount1;

        if (numberOfCandyTypes == 2)
        {
            int candyLevel2;
            do
            {
                candyLevel2 = QuestManager.instance.RandomCandyLevel();
            } while (candyLevel2 == candyLevel1);

            candyCount2 = Random.Range(3, Mathf.Min(10, QuestManager.instance.maxCandyCount - candyCount1 + 1));
            candySprite2 = CandyManager.instance.candySprites[candyLevel2 - 1];
            reward += QuestManager.instance.candyPriceByLevel[candyLevel2] * candyCount2;
        }

        SetupQuest(avatar, candySprite1, candyCount1, candySprite2, candyCount2, QuestManager.instance.FormatGold(reward));
    }

    public void SetupQuest(Sprite avatar, Sprite candySprite1, int candyCount1, Sprite candySprite2, int candyCount2, string formattedReward)
    {
        var questObject = Instantiate(this, QuestManager.instance.questGrid);

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
}