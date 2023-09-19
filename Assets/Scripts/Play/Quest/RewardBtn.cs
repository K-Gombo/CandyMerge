using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public Quest parentQuest;

    [SerializeField] public Button rewardButton;
    
    public static RewardButton instance;

    public float goldIncreaseRate = 0f;
    public float maxGoldIncreaseRate = 30f;

    public float luckyGoldProbability = 0;
    public float maxLuckyGoldProbability = 40f;

    [SerializeField] private GameObject Check1;
    [SerializeField] private GameObject Check2;
   
    private void Awake()
    {
        instance = this;
        if (GetComponent<Button>() == null)
        {
            return;
        }
        rewardButton = GetComponent<Button>();
        rewardButton.interactable = false;
        rewardButton.onClick.AddListener(OnRewardButtonClicked);
    }

    public void UpdateButtonState()
    {
        if (parentQuest == null)
        {
            Debug.Log("UpdateButtonState: parentQuest is null");
            return;
        }

        if (rewardButton == null)
        {
            Debug.Log("UpdateButtonState: Rewardbutton is null");
            return;
        }

        string[] countText1 = parentQuest.candyCountText1.text.Split('/');
        int currentCount1 = int.Parse(countText1[0]);
        int requiredCount1 = int.Parse(countText1[1]);
        bool isCondition1Met = currentCount1 >= requiredCount1;

        // Debug.Log($"UpdateButtonState: currentCount1 = {currentCount1}, requiredCount1 = {requiredCount1}, isCondition1Met = {isCondition1Met}");

        bool isCondition2Met = false;
        Sprite requestCandy2 = parentQuest.requestCandy2.sprite;

        if (requestCandy2 != null)
        {
            string[] countText2 = parentQuest.candyCountText2.text.Split('/');
            int currentCount2 = int.Parse(countText2[0]);
            int requiredCount2 = int.Parse(countText2[1]);
            isCondition2Met = currentCount2 >= requiredCount2;

            // Debug.Log($"UpdateButtonState: currentCount2 = {currentCount2}, requiredCount2 = {requiredCount2}, isCondition2Met = {isCondition2Met}");

            Check2.SetActive(isCondition2Met);
        }
        else
        {
            // requiredCount2가 비활성화 상태일 때
            isCondition2Met = true;
            Check2.SetActive(false); // 이 경우에는 Check2를 비활성화
        }

        if (isCondition1Met)
        {
            Check1.SetActive(true);
        }
        else
        {
            Check1.SetActive(false);
        }

        // 조건 충족 확인
        if (isCondition1Met && isCondition2Met)
        {
            rewardButton.interactable = true;
            Image questImage = parentQuest.GetComponent<Image>();
            if (questImage != null)
            {   
                if (parentQuest.isSpecialQuest) 
                {
                    questImage.color = new Color(0.9f, 0.48f, 1f, 1.0f); // 특별 퀘스트의 경우 다른 색상으로 설정
                }
                else
                {   
                    questImage.color = new Color(0.78f, 0.92f, 0.46f, 1.0f); // 색상을 변경
                }
              
            }
        }
        else
        {
            rewardButton.interactable = false;
            Image questImage = parentQuest.GetComponent<Image>();
            if (questImage != null)
            {
                if (parentQuest.isSpecialQuest) 
                {
                    questImage.color = new Color(1f, 0.8f, 1f, 1.0f); // 특별 퀘스트의 경우 다른 색상으로 설정
                }
                else
                {
                    questImage.color = Color.white; // 원래 색상으로 복구
                }
            }
        }
    }


    Vector3 pulsY = new Vector3(0, 845, 0);
    private void OnRewardButtonClicked()
    {
        if (parentQuest == null)
        {
            return;
        }
        
        // 보상 계산을 위한 초기 설정
        string rewardString = parentQuest.rewardText.text;
        int baseReward = ConvertRewardStringToInt(rewardString);

        // 2배 확률 체크
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        if (randomValue < luckyGoldProbability)
        {
            baseReward *= 2;
        }

        // 실제 증가 비율 계산
        float actualIncreaseRate = 1 + (goldIncreaseRate / 100);

        // 최종 보상 = (기본 보상 또는 2배 보상) * (1 + n%)
        int finalReward = Mathf.FloorToInt(baseReward * actualIncreaseRate);
        RewardMovingManager.instance.RequestMovingCurrency(6, CurrencyType.Gold, finalReward, (transform.parent.transform.localPosition + pulsY));
        // 캔디 회수
        string[] countText1 = parentQuest.candyCountText1.text.Split('/');
        int requiredCount1 = int.Parse(countText1[1]);
        CollectCandy(parentQuest.requestCandy1.sprite, requiredCount1);

        if (parentQuest.requestCandy2.sprite != null)
        {
            string[] countText2 = parentQuest.candyCountText2.text.Split('/');
            int requiredCount2 = int.Parse(countText2[1]);
            CollectCandy(parentQuest.requestCandy2.sprite, requiredCount2);
        }
            
        // 퀘스트 완료 처리
        QuestManager.instance.CompleteQuest(parentQuest);
        parentQuest = null; // 부모 퀘스트 참조를 끊음
        
    }

    private void CollectCandy(Sprite sprite, int requiredCount)
    {   
        
        int level = CandyManager.instance.GetLevelBySprite(sprite);
        int collectedCount = 0;

        foreach (GameObject box in CandyManager.instance.boxes)
        {
            foreach (Transform child in box.transform)
            {
                CandyStatus status = child.GetComponent<CandyStatus>();
                if (status != null && status.level == level && child.gameObject.activeInHierarchy)
                {
                    CandyManager.instance.ReturnToPool(child.gameObject);
                    collectedCount++;
                    

                    if (collectedCount >= requiredCount)
                    {
                        return; // 요구 개수만큼 캔디를 회수했으므로 메서드를 종료합니다.
                    }
                }
            }
        }
    }
    
    private int ConvertRewardStringToInt(string rewardString)
    {
        var match = Regex.Match(rewardString, @"(\d+\.?\d*)([a-zA-Z]+)?");
        string numberPart = match.Groups[1].Value;
        string suffix = match.Groups[2].Value;
        double baseValue = double.Parse(numberPart);

        string[] unit = BigIntegerCtrl_global.bigInteger.GetUnits(); // BigIntegerCtrl_global에서 unit 배열을 가져오는 함수를 작성해야 함.

        int unitIndex = Array.IndexOf(unit, suffix);
        if (unitIndex == -1)
        {
            return (int)baseValue; // Suffix가 없거나 일치하는 것이 없으면 그냥 반환
        }

        double multiplier = Math.Pow(1000, unitIndex);
        int reward = (int)(baseValue * multiplier);

        return reward;
    }

    
    public float GetGoldUp()
    {
        return goldIncreaseRate;
    }

    public void SetGoldUp(float newGoldIncreaseRate)
    {
        newGoldIncreaseRate = Mathf.Round(newGoldIncreaseRate * 10f) / 10f; // 소수 둘째자리에서 반올림
        goldIncreaseRate = Mathf.Min(newGoldIncreaseRate, maxGoldIncreaseRate);
    }
    
    public float GetLuckyGoldUp()
    {
        return luckyGoldProbability;
    }

    public void SetLuckyGoldUp(float newDoubleGoldProbability)
    {
        newDoubleGoldProbability = Mathf.Round(newDoubleGoldProbability * 10f) / 10f;
        luckyGoldProbability = Mathf.Min(newDoubleGoldProbability, maxLuckyGoldProbability);
    }

}