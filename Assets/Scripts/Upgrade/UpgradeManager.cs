using System;
using System.Collections.Generic;
using UnityEngine;
using Keiwando.BigInteger;
using UnityEngine.VFX;
 
public class UpgradeManager : MonoBehaviour
{
    public GiftBoxController giftBoxController; // GiftBoxController 참조
    public CandyStatus candyStatus;
    public CandyController candyController;
    public BoxManager boxManager;
    public CurrencyManager currencyManager;
    public OfflineRewardManager offlineRewardManager;
    
   //몇 개의 Locked 오브젝트가 제거되었는지 저장할 변수
    public int actualRemovedLockedCount = 0;

    
    public float increaseLuckyCreate = 0.05f;
    public float decreaseFilltime = 0.01f; // 캔디 생성 속도 감소조절
    public int increaseMaxCandies = 1;
    public int increaseBaseLevel = 1;
    public float increasePassiveAutoCreateSpeed = 0.01f;
    public float increaseGoldUp = 0.02f;
    public float increaseLuckyGold = 0.02f;
    public float increaseOffLineRewardBonus = 0.03f;
    
     
    private int luckyCreateUpCost = 1600;
    private int createSpeedUpCost = 3000;
    private int removeLockedCost = 500;
    private int maxCandiesUpCost = 500;
    private int candyLevelUpCost = 400;
    private int passiveAutoCreateSpeedUpCost = 900;
    private int goldUpCost = 1500;
    private int luckyGoldUpCost = 1000;
    private int offLineRewardBonusCost = 2000;
    
    // 각 업그레이드의 현재 비용
    public BigInteger currentLuckyCreateUpCost;
    public BigInteger currentCreateSpeedUpCost;
    public BigInteger currentRemoveLockedCost;
    public BigInteger currentMaxCandiesUpCost;
    public BigInteger currentCandyLevelUpCost;
    public BigInteger currentPassiveAutoCreateSpeedUpCost;
    public BigInteger currentGoldUpCost;
    public BigInteger currentLuckyGoldUpCost;
    public BigInteger currentOffLineRewardBonusCost;
    
    // 각 업그레이드의 레벨
    public int luckyCreateLevel ;
    public int createSpeedLevel ;
    public int removeLockedLevel ;
    public int maxCandiesLevel;
    public int candyLevel ;
    public int passiveAutoCreateSpeedLevel ;
    public int goldUpLevel ;
    public int luckyGoldLevel ;
    public int offLineRewardBonusLevel;
    
    //각 업그레이드의 맥스레벨
    public int maxLuckyCreateUpgradeLevel = 400;
    public int maxCreateSpeedUpgradeLevel = 250;
    public int maxRemoveLockedUpgradeLevel = 33;
    public int maxCandiesUpgradeLevel = 30;
    public int maxCandyLevelUpgradeLevel = 57;
    public int maxPassiveAutoCreateSpeedUpgradeLevel = 1000;
    public int maxGoldUpUpgradeLevel = 1000;
    public int maxLuckyGoldUpgradeLevel = 1500;
    public int maxOffLineRewardBonusLevel = 1000;
    

    
    private void Awake()
    {
        // 초기 비용 설정
        currentLuckyCreateUpCost = luckyCreateUpCost;
        currentCreateSpeedUpCost = createSpeedUpCost;
        currentRemoveLockedCost = removeLockedCost;
        currentMaxCandiesUpCost = maxCandiesUpCost;
        currentCandyLevelUpCost = candyLevelUpCost;
        currentPassiveAutoCreateSpeedUpCost = passiveAutoCreateSpeedUpCost;
        currentGoldUpCost = goldUpCost;
        currentLuckyGoldUpCost = luckyGoldUpCost;
        currentOffLineRewardBonusCost = offLineRewardBonusCost;

    }
    
    public void LuckyCreateUp() //스킬1 
    {
        if (luckyCreateLevel >= maxLuckyCreateUpgradeLevel)
        {
            Debug.Log("이미 최대 레벨에 도달했습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentLuckyCreateUpCost))
        {
            float currentLuckyCreate = giftBoxController.GetLuckyCreate();
            float newLuckyCreate = currentLuckyCreate + increaseLuckyCreate;
            giftBoxController.SetLuckyCreate(newLuckyCreate);
            luckyCreateLevel++;

            // 비용을 1.4배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(14);  // 1.4 * 10
            BigInteger newCost = (currentLuckyCreateUpCost * multiplier) / 10;  // 1.4배
            currentLuckyCreateUpCost = newCost;

            Debug.Log($"캔디 확률 업!: {newLuckyCreate}, 새로운 비용: {currentLuckyCreateUpCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    
    public void CreateSpeedUp() //스킬 2
    {
        if (createSpeedLevel >= maxCreateSpeedUpgradeLevel)
        {
            Debug.Log("이미 최대 레벨에 도달했습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentCreateSpeedUpCost))
        {
            float currentFillTime = giftBoxController.GetFillTime();
            float newFillTime = currentFillTime - decreaseFilltime;
            giftBoxController.SetFillTime(newFillTime);
            createSpeedLevel++;

            // 비용을 1.4배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(14);  // 1.4 * 10
            BigInteger newCost = (currentCreateSpeedUpCost * multiplier) / 10;  // 1.4배
            currentCreateSpeedUpCost = newCost;

            Debug.Log($"생산 쿨타임 감소! 현재 레벨: {createSpeedLevel}, 새로운 쿨타임: {newFillTime}, 새로운 비용: {currentCreateSpeedUpCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    
    
   public void RemoveLocked() // Locked 오브젝트 해제 보유 캔디 증가 업 (스킬3)
{
    // 최대 업그레이드 레벨에 도달했는지 확인
    if (removeLockedLevel >= maxRemoveLockedUpgradeLevel)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    Transform boxTile = boxManager.boxTile; // BoxManager에서 boxTile 가져오기
    bool isLockedExist = false;

    // 먼저 Locked 오브젝트가 있는지 없는지 확인
    for (int i = 0; i < boxTile.childCount; i++)
    {
        Transform box = boxTile.GetChild(i);
        if (box.CompareTag("Box"))
        {
            for (int j = 0; j < box.childCount; j++)
            {
                Transform child = box.GetChild(j);
                if (child.CompareTag("Locked"))
                {
                    isLockedExist = true;
                    break;
                }
            }
        }

        if (isLockedExist)
        {
            break;
        }
    }

    // Locked 오브젝트가 없다면 최대 업그레이드 상태
    if (!isLockedExist)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    // 골드가 충분한지 확인
    if (currencyManager.SubtractCurrency("Gold", currentRemoveLockedCost))
    {
        for (int i = 0; i < boxTile.childCount; i++)
        {
            Transform box = boxTile.GetChild(i);
            if (box.CompareTag("Box"))
            {
                for (int j = 0; j < box.childCount; j++)
                {
                    Transform child = box.GetChild(j);
                    if (child.CompareTag("Locked"))
                    {
                        Destroy(child.gameObject);
                        CandyManager.instance.LockedTileRemoved();
                        removeLockedLevel++;
                        
                        // 실제로 몇 개의 Locked 오브젝트가 제거되었는지 저장
                        actualRemovedLockedCount = removeLockedLevel;
                        
                        // 비용을 1.5배로 증가시키는 부분
                        BigInteger multiplier = new BigInteger(30);  // 1.5 * 10  1.5배설정
                        BigInteger newCost = (currentRemoveLockedCost * multiplier) / 10;  
                        currentRemoveLockedCost = newCost;

                        Debug.Log($"Locked 오브젝트 제거 완료! 남은 골드: {currencyManager.GetCurrencyAmount("Gold")}, 새로운 비용: {currentRemoveLockedCost}");
                        return;
                    }
                }
            }
        }
    }
    else
    {
        Debug.Log("골드가 부족합니다.");
    }
}
    
   public void MaxCandiesUp() //선물상자가 최대 생성할 수 있는 캔디 수 증가 (스킬4)
   {
       if (maxCandiesLevel >= maxCandiesUpgradeLevel)
       {
           Debug.Log("이미 최대 레벨까지 업그레이드 되었습니다.");
           return;
       }

       int currentMaxCandies = giftBoxController.GetMaxCandies();
    
       if (currencyManager.SubtractCurrency("Gold", currentMaxCandiesUpCost))
       {
           int newMaxCandies = currentMaxCandies + increaseMaxCandies;
           giftBoxController.SetMaxCandies(newMaxCandies);
           maxCandiesLevel++;

           // 비용을 1.5배로 증가시킵니다.
           BigInteger multiplier = new BigInteger(30);  // 
           BigInteger newCost = (currentMaxCandiesUpCost * multiplier) / 10;  // 
           currentMaxCandiesUpCost = newCost;

           Debug.Log($"캔디 생성 증가!:{newMaxCandies}, 새로운 비용: {currentMaxCandiesUpCost}");
       }
       else
       {
           Debug.Log("골드가 부족합니다.");
       }
   }

    

    public void CandyLevelUp() //기본 제작 캔디 레벨 업 (스킬5)
    {
        int currentBaseLevel = candyStatus.GetBaseLevel();
        if (candyLevel >= maxCandyLevelUpgradeLevel)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
    
        if (currencyManager.SubtractCurrency("Gold", currentCandyLevelUpCost))
        {
            int newBaseLevel = currentBaseLevel + increaseBaseLevel;
            candyStatus.SetBaseLevel(newBaseLevel);
            candyLevel++;

            // 비용을 1.5배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(45);  // 
            BigInteger newCost = (currentCandyLevelUpCost * multiplier) / 10;  // 4.5배
            currentCandyLevelUpCost = newCost;

            Debug.Log($"캔디 레벨업!:{newBaseLevel}, 새로운 비용: {currentCandyLevelUpCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }



    

    public void PassiveAutoCreateSpeedUp() //패시브 자동 제작 속도 업 (스킬7)
    {
        float currentPassiveCreateSpeed = giftBoxController.GetPassiveCreateTry();
        if (passiveAutoCreateSpeedLevel >= maxPassiveAutoCreateSpeedUpgradeLevel)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
    
        if (currencyManager.SubtractCurrency("Gold", currentPassiveAutoCreateSpeedUpCost))
        {
            float newPassiveAutoCreateSpeed = currentPassiveCreateSpeed + increasePassiveAutoCreateSpeed;
            giftBoxController.SetPassiveCreateTry(newPassiveAutoCreateSpeed);
            passiveAutoCreateSpeedLevel++;

            // 비용을 1.3배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(115);  // 1.3 * 10
            BigInteger newCost = (currentPassiveAutoCreateSpeedUpCost * multiplier) / 100;  // 1.3배
            currentPassiveAutoCreateSpeedUpCost = newCost;

            Debug.Log($"자동 생성 속도업!:{newPassiveAutoCreateSpeed}, 새로운 비용: {currentPassiveAutoCreateSpeedUpCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }



    


    public void GoldUp()
    {
        float currentGoldUp = RewardButton.instance.GetGoldUp();
        if (goldUpLevel >= maxGoldUpUpgradeLevel)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentGoldUpCost))
        {
            float newGoldUp = currentGoldUp + increaseGoldUp;
            RewardButton.instance.SetGoldUp(newGoldUp);
            goldUpLevel++;

            // 비용을 1.3배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(115);  // 1.3 * 10
            BigInteger newCost = (currentGoldUpCost * multiplier) / 100;  // 1.3배
            currentGoldUpCost = newCost;

            Debug.Log($"추가 골드 획득 업!: {newGoldUp}, 새로운 비용: {currentGoldUpCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    

    public void LuckyGoldUp()
    {
        float currentLuckyGoldUp = RewardButton.instance.GetLuckyGoldUp();
        if (luckyGoldLevel >= maxLuckyGoldUpgradeLevel)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentLuckyGoldUpCost))
        {
            float newLuckyGoldUp = currentLuckyGoldUp + increaseLuckyGold;
            RewardButton.instance.SetLuckyGoldUp(newLuckyGoldUp);
            luckyGoldLevel++;

            // 비용을 1.2배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(114);  // 1.2 * 10
            BigInteger newCost = (currentLuckyGoldUpCost * multiplier) / 100;  // 1.2배
            currentLuckyGoldUpCost = newCost;

            Debug.Log($"골드 2배 확률 업!: {newLuckyGoldUp}, 새로운 비용: {currentLuckyGoldUpCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    
    
    public void OffLineRewardBonusUp() // 스킬10
    {
        if (offLineRewardBonusLevel >= maxOffLineRewardBonusLevel)
        {
            Debug.Log("이미 최대 레벨에 도달했습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentOffLineRewardBonusCost))
        {
            float currentOffLineRewardBonus = offlineRewardManager.GetOffLineRewardBonusUp();
            float newOffLineRewardBonus = currentOffLineRewardBonus + increaseOffLineRewardBonus;
            offlineRewardManager.SetOffLineRewardBonusUp(newOffLineRewardBonus);
            offLineRewardBonusLevel++;

            // 비용을 1.3배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(115);  // 1.3 * 10
            BigInteger newCost = (currentOffLineRewardBonusCost * multiplier) / 100;  // 1.3배
            currentOffLineRewardBonusCost = newCost;

            Debug.Log($"오프라인 리워드 보너스 업!: {newOffLineRewardBonus}, 새로운 비용: {currentOffLineRewardBonusCost}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    
public void LuckyCreateUp(int luckyCreateLevel)
{
    for (int i = 0; i < luckyCreateLevel; i++)
    {
        float currentLuckyCreate = giftBoxController.GetLuckyCreate();

            // 비용을 1.4배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(14);  // 1.4 * 10
            BigInteger newCost = (currentLuckyCreateUpCost * multiplier) / 10;  // 1.4배
            currentLuckyCreateUpCost = newCost;


            float newLuckyCreate = Mathf.Min(currentLuckyCreate + increaseLuckyCreate, giftBoxController.maxLuckyCreate);
        giftBoxController.SetLuckyCreate(newLuckyCreate);

        Debug.Log($"캔디 확률 업!: {newLuckyCreate}");
    }

}

public void CreateSpeedUp(int createSpeedLevel)
{
    for (int i = 0; i < createSpeedLevel; i++)
    {
        float currentFillTime = giftBoxController.GetFillTime();

            // 비용을 1.4배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(14);  // 1.4 * 10
            BigInteger newCost = (currentCreateSpeedUpCost * multiplier) / 10;  // 1.4배
            currentCreateSpeedUpCost = newCost;

            float newFillTime = Mathf.Max(currentFillTime - decreaseFilltime, giftBoxController.minimumFillTime);
        giftBoxController.SetFillTime(newFillTime);

        Debug.Log($"생산 쿨타임 감소!:{newFillTime}");
    }
}

public void RemoveLocked(int count)
{
    Transform boxTile = boxManager.boxTile; // BoxManager에서 boxTile 가져오기
    Debug.Log("나 먼저 불렸다 : " + boxTile.childCount);

    int lockedCount = 0; // 현재 잠금 상자 개수를 저장
    List<Transform> lockedBoxes = new List<Transform>(); // 잠금 상자 리스트

    // 현재 잠금 상자 개수와 해당 상자들을 lockedBoxes 리스트에 추가
    for (int i = 0; i < boxTile.childCount; i++)
    {
        Transform box = boxTile.GetChild(i);
        if (box.CompareTag("Box"))
        {
            for (int j = 0; j < box.childCount; j++)
            {

                    Transform child = box.GetChild(j);
                if (child.CompareTag("Locked"))
                {
                    lockedBoxes.Add(child);
                    lockedCount++;
                }
            }
        }
    }

    if (lockedCount == 0)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    if (count > lockedCount)
    {
        Debug.Log($"요청한 잠금 해제 개수({count}개)는 현재 잠금 상자 개수({lockedCount}개)보다 많습니다. {lockedCount}개만 해제됩니다.");
        count = lockedCount;
    }

    for (int i=0; i<count; i++)
        {
            // 비용을 1.5배로 증가시키는 부분
            BigInteger multiplier = new BigInteger(30);  // 1.5 * 10  1.5배설정
            BigInteger newCost = (currentRemoveLockedCost * multiplier) / 10;
            currentRemoveLockedCost = newCost;
        }

    BigInteger totalCost = currentRemoveLockedCost * count; // 요청한 잠금 해제 개수에 따른 전체 비용


    for (int i = 0; i < count; i++)
    {
        Destroy(lockedBoxes[i].gameObject);
        CandyManager.instance.LockedTileRemoved();
    }
    Debug.Log($"{count}개의 Locked 오브젝트가 제거되었습니다! 남은 골드: {currencyManager.GetCurrencyAmount("Gold")}");

}
    
public void MaxCandiesUp(int count)
{
    int currentMaxCandies = giftBoxController.GetMaxCandies();

    int totalCandiesToIncrease = increaseMaxCandies * count; // 원하는만큼 증가시킬 캔디 수
    int potentialMaxCandies = currentMaxCandies + totalCandiesToIncrease; // 캔디 증가 후 잠재적인 최대 캔디 수

    if (currentMaxCandies >= giftBoxController.realMaxCandies)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    if (potentialMaxCandies > giftBoxController.realMaxCandies)
    {
        Debug.Log($"요청한 증가량({count}번)으로 인해 최대 캔디 수가 실제 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
        totalCandiesToIncrease = giftBoxController.realMaxCandies - currentMaxCandies;
    }

    for (int i=0; i<count; i++)
        {
            // 비용을 1.5배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(30);  // 1.5 * 10
            BigInteger newCost = (currentMaxCandiesUpCost * multiplier) / 10;  // 1.5배
            currentMaxCandiesUpCost = newCost;
        }

    BigInteger totalCost = currentMaxCandiesUpCost * count; // 요청한 증가량에 따른 전체 비용


    giftBoxController.SetMaxCandies(currentMaxCandies + totalCandiesToIncrease);
    Debug.Log($"캔디 생성 증가!:{currentMaxCandies + totalCandiesToIncrease}");
}


public void CandyLevelUp(int count)
{
    int currentBaseLevel = candyStatus.GetBaseLevel();

    int totalLevelsToIncrease = increaseBaseLevel * count; // 원하는만큼 증가시킬 레벨
    int potentialBaseLevel = currentBaseLevel + totalLevelsToIncrease; // 레벨 증가 후 잠재적인 캔디 레벨

    if (currentBaseLevel >= candyStatus.maxBaseLevel)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    if (potentialBaseLevel > candyStatus.maxBaseLevel)
    {
        Debug.Log($"요청한 증가량({count}번)으로 인해 캔디 레벨이 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
        totalLevelsToIncrease = candyStatus.maxBaseLevel - currentBaseLevel;
    }

    for (int i=0; i<count; i++)
        {
            // 비용을 1.5배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(45);  // 1.5 * 10
            BigInteger newCost = (currentCandyLevelUpCost * multiplier) / 10;  // 1.5배
            currentCandyLevelUpCost = newCost;
        }

    BigInteger totalCost = currentCandyLevelUpCost * count; // 요청한 증가량에 따른 전체 비용

    candyStatus.SetBaseLevel(currentBaseLevel + totalLevelsToIncrease);
    Debug.Log($"캔디 레벨업!:{currentBaseLevel + totalLevelsToIncrease}");

}


public void PassiveAutoCreateSpeedUp(int count)
{
    float currentPassiveCreateSpeed = giftBoxController.GetPassiveCreateTry();

    float totalSpeedIncrease = increasePassiveAutoCreateSpeed * count; // 원하는만큼 속도 증가
    float potentialPassiveCreateSpeed = currentPassiveCreateSpeed + totalSpeedIncrease; // 속도 증가 후 잠재적인 패시브 생성 속도

    if (currentPassiveCreateSpeed >= giftBoxController.maxPassiveCreateTry)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    if (potentialPassiveCreateSpeed > giftBoxController.maxPassiveCreateTry)
    {
        Debug.Log($"요청한 증가량({count}번)으로 인해 패시브 자동 생성 속도가 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
        totalSpeedIncrease = giftBoxController.maxPassiveCreateTry - currentPassiveCreateSpeed;
    }

    for (int i=0; i<count; i++)
        {
            // 비용을 1.3배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(115);  // 1.3 * 10
            BigInteger newCost = (currentPassiveAutoCreateSpeedUpCost * multiplier) / 100;  // 1.3배
            currentPassiveAutoCreateSpeedUpCost = newCost;
        }

    BigInteger totalCost = currentPassiveAutoCreateSpeedUpCost * count; // 요청한 증가량에 따른 전체 비용


    float newPassiveAutoCreateSpeed = currentPassiveCreateSpeed + totalSpeedIncrease;
    giftBoxController.SetPassiveCreateTry(newPassiveAutoCreateSpeed);
    Debug.Log($"자동 생성 속도업!:{newPassiveAutoCreateSpeed}");

}

public void GoldUp(int count)
{
    float currentGoldUp = RewardButton.instance.GetGoldUp();

    float totalGoldIncrease = increaseGoldUp * count; // 원하는만큼 골드 획득량 증가
    float potentialGoldUp = currentGoldUp + totalGoldIncrease; // 획득량 증가 후 잠재적인 골드 업

    if (currentGoldUp >= RewardButton.instance.maxGoldIncreaseRate)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    if (potentialGoldUp > RewardButton.instance.maxGoldIncreaseRate)
    {
        Debug.Log($"요청한 증가량({count}번)으로 인해 골드 획득량이 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
        totalGoldIncrease = RewardButton.instance.maxGoldIncreaseRate - currentGoldUp;
    }

    for (int i =0; i<count; i++)
        {
            // 비용을 1.3배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(115);  // 1.3 * 10
            BigInteger newCost = (currentGoldUpCost * multiplier) / 100;  // 1.3배
            currentGoldUpCost = newCost;
        }

    BigInteger totalCost = currentGoldUpCost * count; // 요청한 증가량에 따른 전체 비용


    float newGoldUp = currentGoldUp + totalGoldIncrease;
    RewardButton.instance.SetGoldUp(newGoldUp);
    Debug.Log($"추가 골드 획득 업!: {newGoldUp}");

}


public void LuckyGoldUp(int count)
{
    float currentLuckyGoldUp = RewardButton.instance.GetLuckyGoldUp();

    float totalLuckyGoldIncrease = increaseLuckyGold * count; // 요청한 횟수만큼 확률 증가
    float potentialLuckyGoldUp = currentLuckyGoldUp + totalLuckyGoldIncrease; // 증가 후의 잠재적인 확률

    if (currentLuckyGoldUp >= RewardButton.instance.maxLuckyGoldProbability)
    {
        Debug.Log("이미 최대로 업그레이드 되었습니다.");
        return;
    }

    if (potentialLuckyGoldUp > RewardButton.instance.maxLuckyGoldProbability)
    {
        Debug.Log($"요청한 증가량({count}번)으로 인해 확률이 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
        totalLuckyGoldIncrease = RewardButton.instance.maxLuckyGoldProbability - currentLuckyGoldUp;
    }

    for (int i=0; i< count; i++)
        {
            // 비용을 1.2배로 증가시킵니다.
            BigInteger multiplier = new BigInteger(114);  // 1.2 * 10
            BigInteger newCost = (currentLuckyGoldUpCost * multiplier) / 100;  // 1.2배
            currentLuckyGoldUpCost = newCost;
        }

    BigInteger totalCost = currentLuckyGoldUpCost * count; // 요청한 횟수만큼의 전체 비용 계산

    float newLuckyGoldUp = currentLuckyGoldUp + totalLuckyGoldIncrease;
    RewardButton.instance.SetLuckyGoldUp(newLuckyGoldUp);
    Debug.Log($"골드 2배 확률 업!: {newLuckyGoldUp}");

}

    public void OffLineRewardBonusUp(int count) // 스킬10
    {
        if (offLineRewardBonusLevel >= maxOffLineRewardBonusLevel)
        {
            Debug.Log("이미 최대 레벨에 도달했습니다.");
            return;
        }

        float currentOffLineRewardBonus = offlineRewardManager.GetOffLineRewardBonusUp();
        float totalOffLineRewardBonusIncrease = increaseOffLineRewardBonus * count; // 요청한 횟수만큼 증가
        float newOffLineRewardBonus = currentOffLineRewardBonus + totalOffLineRewardBonusIncrease;

        // 비용과 레벨을 증가시킨다.
        for (int i = 0; i < count; i++)
        {
            if (offLineRewardBonusLevel >= maxOffLineRewardBonusLevel)
            {
                Debug.Log($"최대 레벨 {maxOffLineRewardBonusLevel}에 도달했습니다.");
                break;
            }
            // 비용을 1.3배로 증가시킵니다.

            BigInteger multiplier = new BigInteger(115); // 1.3 * 10
            currentOffLineRewardBonusCost = (currentOffLineRewardBonusCost * multiplier) / 100; // 1.3배
        }

        offlineRewardManager.SetOffLineRewardBonusUp(newOffLineRewardBonus);
        Debug.Log($"오프라인 리워드 보너스 업!: {newOffLineRewardBonus}, 새로운 비용: {currentOffLineRewardBonusCost}");
    }


}

