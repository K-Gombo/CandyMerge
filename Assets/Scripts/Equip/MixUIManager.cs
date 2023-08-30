using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MixUIManager : MonoBehaviour
{
    public Text LevelSumText; // LevelSum을 표시할 Text UI

    // Update is called once per frame
    void Update()
    {
        UpdateLevelSum();
    }

    void UpdateLevelSum()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox");
        int totalLevelSum = 0; // 모든 MixBox에 있는 캔디 레벨의 합

        foreach (GameObject mixBox in mixBoxes)
        {
            CandyStatus[] candies = mixBox.GetComponentsInChildren<CandyStatus>(); // 각 MixBox에 있는 CandyStatus 컴포넌트들
            
            foreach (CandyStatus candy in candies)
            {
                totalLevelSum += candy.level; // 캔디 레벨 합산
            }
        }

        LevelSumText.text = "캔디 총합 \n" + totalLevelSum.ToString(); // LevelSum 텍스트 업데이트
    }
}