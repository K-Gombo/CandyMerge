using UnityEngine;

public class CandyStatus : MonoBehaviour
{
    public int level; // 캔디의 레벨을 저장하는 변수

    private void OnDestroy()
    {
        CandyManager.instance.CandyDestroyed(); // 캔디가 파괴될 때마다 CandyManager에 알림
    }
}