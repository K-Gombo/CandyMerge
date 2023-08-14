using UnityEngine;

public class Candy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CandyStatus status = GetComponent<CandyStatus>();
        status.level = Random.Range(1, 4); // 1부터 3까지의 레벨만 생성되게 변경
        CandyManager.instance.SetAppearance(this.gameObject);
        Debug.Log($"Lv.{status.level} Candy");
    }

   
}