using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public Transform boxTile;
    private Dictionary<int, int> candyLevelsCount;
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
    }

    public int GetCurrentTotalCandyCount()
    {
        return totalCandyCount;
    }
}