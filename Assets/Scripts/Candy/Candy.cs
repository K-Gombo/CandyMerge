using UnityEngine;

public class Candy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public static int baseLevel = 1; // deafault 레벨 (스킬 업그레이드 시 증가)

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CandyStatus status = GetComponent<CandyStatus>();
        status.level = baseLevel; // 기본 레벨로 설정
        CandyManager.instance.SetAppearance(this.gameObject);
        Debug.Log($"Lv.{status.level} Candy");
    }

    // 스킬 업그레이드 메서드
    public static void UpgradeLevel()
    {
        baseLevel++; // 기본 레벨 증가
    }
}