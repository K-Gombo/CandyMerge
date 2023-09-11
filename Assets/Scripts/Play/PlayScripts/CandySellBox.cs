using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySellBox : MonoBehaviour
{
    public CandyManager candyManager; // CandyManager 참조
    public QuestManager questManager; // QuestManager 참조
    public CurrencyManager currencyManager; // CurrencyManager 참조
    
    // OnTriggerEnter2D를 사용하여 캔디가 들어온 것을 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 태그를 통해 캔디인지 확인
        if (collision.CompareTag("Candy"))
        {
            // 캔디의 레벨을 찾아서 가격을 얻음
            int candyLevel = collision.GetComponent<CandyStatus>().level;
            long sellPrice = questManager.candySellPriceByLevel.ContainsKey(candyLevel) 
                ? questManager.candySellPriceByLevel[candyLevel] 
                : 0;
            
            // 골드를 지급
            currencyManager.AddCurrency("Gold", (int)sellPrice);

            // 캔디를 Pool로 반환
            candyManager.ReturnToPool(collision.gameObject);
        }
    }
    
}