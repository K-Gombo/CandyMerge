using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public Quest parentQuest; // 부모 퀘스트를 참조하기 위한 변수

    private Button rewardButton;

    private void Start()
    {
        rewardButton = GetComponent<Button>();
        rewardButton.interactable = false; // 초기에는 비활성화
        rewardButton.onClick.AddListener(OnRewardButtonClicked); // 클릭 이벤트에 메서드 연결
    }

    public void UpdateButtonState()
    {
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
        // 보상 지급
        int reward = int.Parse(parentQuest.rewardText.text);
        CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();
        currencyManager.AddCurrency("Gold", reward);

        // 퀘스트 완료 처리
        Debug.Log("퀘스트 완료! 보상 지급!");
        QuestManager.instance.CompleteQuest(parentQuest);
    }
}