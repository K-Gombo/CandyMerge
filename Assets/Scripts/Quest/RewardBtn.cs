using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public Quest parentQuest; // 부모 퀘스트를 참조하기 위한 변수

    private Button rewardButton;
    
    
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
    
    private void Start()
    {
        rewardButton = GetComponent<Button>();
        rewardButton.interactable = false;
        rewardButton.onClick.AddListener(OnRewardButtonClicked);
       
    }

    public void UpdateButtonState()
    {   
        if (parentQuest == null)
        {
            
            return; // parentQuest가 null이면 메서드를 종료
        }
        
        if (rewardButton == null)
        {
            
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
        if (parentQuest == null)
        {
            return; // parentQuest가 null이면 메서드를 종료
        }
        rewardButton.onClick.RemoveListener(OnRewardButtonClicked);
       
        // 보상 지급
        string rewardString = parentQuest.rewardText.text;
        int reward = ConvertRewardStringToInt(rewardString);
        CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();
        currencyManager.AddCurrency("Gold", reward);

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

}