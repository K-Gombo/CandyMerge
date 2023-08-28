using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public Transform boxTile;
    public Dictionary<int, int> candyLevelsCount;
    private int totalCandyCount;


    public static BoxManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        candyLevelsCount = new Dictionary<int, int>();
        UpdateCandyCount();
    }

    public void UpdateCandyCount() 
    {
        candyLevelsCount.Clear();
        totalCandyCount = 0;

        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform box = boxTile.GetChild(i);
            if (box.CompareTag("Box") && box.childCount > 0)
            {
                CandyStatus candyStatus = box.GetChild(0).GetComponent<CandyStatus>();
                if (candyStatus != null)
                {
                    int level = candyStatus.level;
                    if (candyLevelsCount.ContainsKey(level))
                    {
                        candyLevelsCount[level]++;
                    }
                    else
                    {
                        candyLevelsCount[level] = 1;
                    }
                    totalCandyCount++;
                }
            }
        }

        // 활성 퀘스트의 상태 업데이트
        foreach (Quest quest in QuestManager.instance.activeQuests)
        {
            QuestManager.instance.UpdateQuestCandyCount(quest);
        }
    }

    public int GetCurrentTotalCandyCount()
    {
        return totalCandyCount;
    }
    
    public int GetCandyCountByLevel(int level)
    {
        if (candyLevelsCount.ContainsKey(level))
        {
            return candyLevelsCount[level];
        }
        return 0;
    }
    
    public int GetMaxCandyLevel()
    {
        if (candyLevelsCount == null)
        {
            Debug.LogError("candyLevelsCount가 아직 초기화되지 않았습니다.");
            return 0; // 아직 초기화되지 않았으므로 0 반환
        }

        int maxLevel = 0;
        foreach (int level in candyLevelsCount.Keys)
        {
            if (level > maxLevel)
            {
                maxLevel = level;
            }
        }
        return maxLevel;
    }
    
    




    
    
}