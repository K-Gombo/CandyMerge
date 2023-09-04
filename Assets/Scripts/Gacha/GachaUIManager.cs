using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaUIManager : MonoBehaviour
{
    public Text LevelSumText; // LevelSum을 표시할 Text UI
    public int totalLevelSum { get; private set; } // 여기에 totalLevelSum를 멤버 변수로 선언

    // Update is called once per frame
    void Update()
    {
        UpdateLevelSum();
    }

    void UpdateLevelSum()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox");
        totalLevelSum = 0; // 이 부분 수정. totalLevelSum를 지역 변수에서 멤버 변수로 변경

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