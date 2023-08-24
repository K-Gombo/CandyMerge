using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GiftBoxController giftBoxController; // GiftBoxController 참조
    public float decreaseAmount = 0.5f; // 캔디 생성 속도 감소조절 

    public void CreateSpeedUp()
    {
        float newFillTime = giftBoxController.GetFillTime() - decreaseAmount; // 현재 fillTime 값을 가져와서 n초 감소
        giftBoxController.SetFillTime(newFillTime); // 값을 변경하는 메서드 호출
    }
}