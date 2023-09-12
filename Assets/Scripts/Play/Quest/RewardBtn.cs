using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class RewardButton : MonoBehaviour
{
    public Quest parentQuest; // 부모 퀘스트를 참조하기 위한 변수

    private Button rewardButton;
    
    // 골드량 증가 비율
    public float goldIncreaseRate = 0f;
    public float maxGoldIncreaseRate = 30f;

    // 골드 2배 획득 확률 
    public float luckyGoldProbability = 0;
    public float maxLuckyGoldProbability = 40f;
    
    private void Awake()
    {
        if (GetComponent<Button>() == null)
        {
            
            return;
        }
        rewardButton = GetComponent<Button>(); // 버튼 컴포넌트를 rewardButton 변수에 할당
        rewardButton.interactable = false; // 초기에는 비활성화
        rewardButton.onClick.AddListener(OnRewardButtonClicked); // 클릭 이벤트에 메서드 연결
        
    }
    
   

    public void UpdateButtonState()
    {   
        if (parentQuest == null)
        {
            Debug.Log("parentQuest is null");
            return; // parentQuest가 null이면 메서드를 종료
        }
        
        if (rewardButton == null)
        {
            Debug.Log("Rewardbutton is null");
            return;
        }
        // 퀘스트 완료 조건 검사
        string[] countText1 = parentQuest.candyCountText1.text.Split('/');
        int currentCount1 = int.Parse(countText1[0]);
        int requiredCount1 = int.Parse(countText1[1]);

        bool isCondition1Met = currentCount1 >= requiredCount1;

        bool isCondition2Met = true; // 기본적으로 두 번째 조건은 충족된 것으로 간주
        Sprite requestCandy2 = parentQuest.requestCandy2.sprite;
        if (requestCandy2 != null)
        {
            string[] countText2 = parentQuest.candyCountText2.text.Split('/');
            int currentCount2 = int.Parse(countText2[0]);
            int requiredCount2 = int.Parse(countText2[1]);
            isCondition2Met = currentCount2 >= requiredCount2;
        }

        // 조건 충족 확인
        if (isCondition1Met && isCondition2Met)
        {
            rewardButton.interactable = true; // 조건 충족 시 활성화
        }
        else
        {
            rewardButton.interactable = false; // 조건 미충족 시 비활성화
        }
    }

    private void OnRewardButtonClicked()
    {   
        Debug.Log("보상 버튼이 클릭되었습니다."); // 로그 추가
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

        // 최종 보상 지급
        CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();
        currencyManager.AddCurrency("Gold", finalReward);
        
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
        var match = Regex.Match(rewardString, @"(\d+\.?\d*)([a-d]?)");
        string numberPart = match.Groups[1].Value;
        string suffix = match.Groups[2].Value;
        double baseValue = double.Parse(numberPart);

        int reward = 0;
        switch(suffix)
        {
            case "a":
                reward = (int)(baseValue * 1000);
                break;
            case "b":
                reward = (int)(baseValue * 1000000);
                break;
            case "c":
                reward = (int)(baseValue * 1000000000);
                break;
            case "d":
                reward = (int)(baseValue * 1000000000000);
                break;
            default:
                reward = (int)baseValue;
                break;
        }

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