using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeManager : MonoBehaviour
{
    public GiftBoxController giftBoxController; // GiftBoxController 참조
    public CandyStatus candyStatus;
    public CandyController candyController;
    public BoxManager boxManager;
    public CurrencyManager currencyManager;
    public RewardButton rewardButton;
    
    public float decreaseFilltime = 0.3f; // 캔디 생성 속도 감소조절 
    public float increaseLuckyCreate = 0.5f;
    public int increaseMaxCandies = 1;
    public int increaseBaseLevel = 1;
    public float increasePassiveAutoCreateSpeed = 0.2f;
    public float increaseGoldUp = 0.5f;
    public float increaseLuckyGold = 0.5f;
    
    public float costMultiplier = 2.0f; //업그레이드 비용 증가 비율

    
    
     
    public int LuckyCreateUpCost = 800; 
    public int CreateSpeedUpCost = 1000;
    public int RemoveLockedCost = 1000;
    public int MaxCandiesUpCost = 1000;
    public int CandyLevelUpCost = 800;
    public int PassiveAutoCreateSpeedUpCost = 900;
    public int GoldUpCost = 1500;
    public int LuckyGoldUpCost = 1500;
    
    // 각 업그레이드의 현재 비용
    public int currentLuckyCreateUpCost;
    public int currentCreateSpeedUpCost;
    public int currentRemoveLockedCost;
    public int currentMaxCandiesUpCost;
    public int currentCandyLevelUpCost;
    public int currentPassiveAutoCreateSpeedUpCost;
    public int currentGoldUpCost;
    public int currentLuckyGoldUpCost;
    
    // 각 업그레이드의 레벨
    public int luckyCreateLevel = 1;
    public int createSpeedLevel = 1;
    public int removeLockedLevel = 1;
    public int maxCandiesLevel = 1;
    public int candyLevel = 1;
    public int passiveAutoCreateSpeedLevel = 1;
    public int goldUpLevel = 1;
    public int luckyGoldLevel = 1;
    
    private void Start()
    {
        // 초기 비용 설정
        currentLuckyCreateUpCost = LuckyCreateUpCost;
        currentCreateSpeedUpCost = CreateSpeedUpCost;
        currentRemoveLockedCost = RemoveLockedCost;
        currentMaxCandiesUpCost = MaxCandiesUpCost;
        currentCandyLevelUpCost = CandyLevelUpCost;
        currentPassiveAutoCreateSpeedUpCost = PassiveAutoCreateSpeedUpCost;
        currentGoldUpCost = GoldUpCost;
        currentLuckyGoldUpCost = LuckyGoldUpCost;
    }
    
    public void UpdateCost(ref int currentCost)
    {
        currentCost = Mathf.FloorToInt(currentCost * costMultiplier);
    }

    
   
    
    public void LuckyCreateUp() // 캔디 2개씩 생성확률업 (스킬1)
    {
        float currentLuckyCreate = giftBoxController.GetLuckyCreate();
        if (currentLuckyCreate >= giftBoxController.maxLuckyCreate)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
        if (currencyManager.SubtractCurrency("Gold", currentLuckyCreateUpCost))
        {
            float newLuckyCreate = Mathf.Min(currentLuckyCreate + increaseLuckyCreate, giftBoxController.maxLuckyCreate);
            giftBoxController.SetLuckyCreate(newLuckyCreate);
            UpdateCost(ref currentLuckyCreateUpCost);
            luckyCreateLevel++;
            Debug.Log($"캔디 확률 업!: {newLuckyCreate}");
            
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    
    public void LuckyCreateUp(int luckyCreateLevel)
    {
        for (int i=0; i<luckyCreateLevel; i++)
        {
            float currentLuckyCreate = giftBoxController.GetLuckyCreate();

            float newLuckyCreate = Mathf.Min(currentLuckyCreate + increaseLuckyCreate, giftBoxController.maxLuckyCreate);
            giftBoxController.SetLuckyCreate(newLuckyCreate);

            Debug.Log($"캔디 확률 업!: {newLuckyCreate}");
        }
    }
    

    public void CreateSpeedUp() //선물상자 제작속도 업 (스킬2)
    {
        float currentFillTime = giftBoxController.GetFillTime();
        if (currentFillTime <= giftBoxController.minimumFillTime)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentCreateSpeedUpCost))
        {
            float newFillTime = Mathf.Max(currentFillTime - decreaseFilltime, giftBoxController.minimumFillTime);
            giftBoxController.SetFillTime(newFillTime);
            UpdateCost(ref currentCreateSpeedUpCost);
            createSpeedLevel++;
            Debug.Log($"생산 쿨타임 감소!:{newFillTime}");
            
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void CreateSpeedUp(int createSpeedLevel)
    {
        for (int i=0; i < createSpeedLevel; i++)
        {
            float currentFillTime = giftBoxController.GetFillTime();

            float newFillTime = Mathf.Max(currentFillTime - decreaseFilltime, giftBoxController.minimumFillTime);
            giftBoxController.SetFillTime(newFillTime);

            Debug.Log($"생산 쿨타임 감소!:{newFillTime}");
        }
    }
    
    public void RemoveLocked() //Locked 오브젝트 해제 보유 캔디 증가 업 (스킬3)
    {
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
                            UpdateCost(ref currentRemoveLockedCost);
                            removeLockedLevel++;
                            Debug.Log($"Locked 오브젝트 제거 완료! 남은 골드: {currencyManager.GetCurrencyAmount("Gold")}");
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

    public void RemoveLocked(int count)
    {
        Transform boxTile = boxManager.boxTile; // BoxManager에서 boxTile 가져오기

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

        int totalCost = currentRemoveLockedCost * count; // 요청한 잠금 해제 개수에 따른 전체 비용

        if (currencyManager.SubtractCurrency("Gold", totalCost))
        {
            for (int i = 0; i < count; i++)
            {
                Destroy(lockedBoxes[i].gameObject);
                CandyManager.instance.LockedTileRemoved();
            }
            Debug.Log($"{count}개의 Locked 오브젝트가 제거되었습니다! 남은 골드: {currencyManager.GetCurrencyAmount("Gold")}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }



    public void MaxCandiesUp() //선물상자가 최대 생성할 수 있는 캔디 수 증가 (스킬4)
    {
        int currentMaxCandies = giftBoxController.GetMaxCandies();
        if (currentMaxCandies >= giftBoxController.realMaxCandies) 
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
        if (currencyManager.SubtractCurrency("Gold", currentMaxCandiesUpCost))
        {
            int newMaxCandies = currentMaxCandies + increaseMaxCandies;
            giftBoxController.SetMaxCandies(newMaxCandies);
            UpdateCost(ref currentMaxCandiesUpCost);
            maxCandiesLevel++;
            Debug.Log($"캔디 생성 증가!:{newMaxCandies}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
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

        int totalCost = currentMaxCandiesUpCost * count; // 요청한 증가량에 따른 전체 비용

        if (currencyManager.SubtractCurrency("Gold", totalCost))
        {
            giftBoxController.SetMaxCandies(currentMaxCandies + totalCandiesToIncrease);
            UpdateCost(ref currentMaxCandiesUpCost);
            Debug.Log($"캔디 생성 증가!:{currentMaxCandies + totalCandiesToIncrease}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    public void CandyLevelUp() //기본 제작 캔디 레벨 업 (스킬5)
    {
        int currentBaseLevel = candyStatus.GetBaseLevel();
        if (currentBaseLevel >= candyStatus.maxBaseLevel) 
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
        if (currencyManager.SubtractCurrency("Gold", currentCandyLevelUpCost))
        {
            int newBaseLevel = currentBaseLevel + increaseBaseLevel;
            candyStatus.SetBaseLevel(newBaseLevel);
            UpdateCost(ref currentCandyLevelUpCost);
            candyLevel++;
            Debug.Log($"캔디 레벨업!:{newBaseLevel}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
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

        int totalCost = currentCandyLevelUpCost * count; // 요청한 증가량에 따른 전체 비용

        if (currencyManager.SubtractCurrency("Gold", totalCost))
        {
            candyStatus.SetBaseLevel(currentBaseLevel + totalLevelsToIncrease);
            Debug.Log($"캔디 레벨업!:{currentBaseLevel + totalLevelsToIncrease}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    public void PassiveAutoCreateSpeedUp() //패시브 자동 제작 속도 업 (스킬7)
    {
        float currentPassiveCreateSpeed = giftBoxController.GetPassiveCreateTry();
        if (currentPassiveCreateSpeed >= giftBoxController.maxPassiveCreateTry) 
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
        if (currencyManager.SubtractCurrency("Gold", currentPassiveAutoCreateSpeedUpCost))
        {
            float newPassiveAutoCreateSpeed = Mathf.Min(currentPassiveCreateSpeed + increasePassiveAutoCreateSpeed, giftBoxController.maxPassiveCreateTry);
            giftBoxController.SetPassiveCreateTry(newPassiveAutoCreateSpeed);
            UpdateCost(ref currentPassiveAutoCreateSpeedUpCost);
            passiveAutoCreateSpeedLevel++;
            Debug.Log($"자동 생성 속도업!:{newPassiveAutoCreateSpeed}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
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

        int totalCost = currentPassiveAutoCreateSpeedUpCost * count; // 요청한 증가량에 따른 전체 비용

        if (currencyManager.SubtractCurrency("Gold", totalCost))
        {
            float newPassiveAutoCreateSpeed = currentPassiveCreateSpeed + totalSpeedIncrease;
            giftBoxController.SetPassiveCreateTry(newPassiveAutoCreateSpeed);
            Debug.Log($"자동 생성 속도업!:{newPassiveAutoCreateSpeed}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    //public void GoldUp()
    //{
    //    float currentGoldUp = rewardButton.GetGoldUp();
    //    if (currentGoldUp >= rewardButton.maxGoldIncreaseRate)
    //    {
    //        Debug.Log("이미 최대로 업그레이드 되었습니다.");
    //        return;
    //    }

    //    if (currencyManager.SubtractCurrency("Gold", currentGoldUpCost))
    //    {
    //        float newGoldUp = Mathf.Min(currentGoldUp + increaseGoldUp, rewardButton.maxGoldIncreaseRate);
    //        rewardButton.SetGoldUp(newGoldUp);
    //        UpdateCost(ref currentGoldUpCost);
    //        goldUpLevel++;
    //        Debug.Log($"추가 골드 획득 업!: {newGoldUp}");
    //    }
    //    else
    //    {
    //        Debug.Log("골드가 부족합니다.");
    //    }
    //}

    //public void GoldUp(int count)
    //{
    //    float currentGoldUp = rewardButton.GetGoldUp();

    //    float totalGoldIncrease = increaseGoldUp * count; // 원하는만큼 골드 획득량 증가
    //    float potentialGoldUp = currentGoldUp + totalGoldIncrease; // 획득량 증가 후 잠재적인 골드 업

    //    if (currentGoldUp >= rewardButton.maxGoldIncreaseRate)
    //    {
    //        Debug.Log("이미 최대로 업그레이드 되었습니다.");
    //        return;
    //    }

    //    if (potentialGoldUp > rewardButton.maxGoldIncreaseRate)
    //    {
    //        Debug.Log($"요청한 증가량({count}번)으로 인해 골드 획득량이 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
    //        totalGoldIncrease = rewardButton.maxGoldIncreaseRate - currentGoldUp;
    //    }

    //    int totalCost = currentGoldUpCost * count; // 요청한 증가량에 따른 전체 비용

    //    if (currencyManager.SubtractCurrency("Gold", totalCost))
    //    {
    //        float newGoldUp = currentGoldUp + totalGoldIncrease;
    //        rewardButton.SetGoldUp(newGoldUp);
    //        Debug.Log($"추가 골드 획득 업!: {newGoldUp}");
    //    }
    //    else
    //    {
    //        Debug.Log("골드가 부족합니다.");
    //    }
    //}

    public void LuckyGoldUp()
    {
        float currentLuckyGoldUp = rewardButton.GetLuckyGoldUp();
        if (currentLuckyGoldUp >= rewardButton.maxLuckyGoldProbability)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }

        if (currencyManager.SubtractCurrency("Gold", currentLuckyGoldUpCost))
        {
            float newLuckyGoldUp = Mathf.Min(currentLuckyGoldUp + increaseLuckyGold, rewardButton.maxLuckyGoldProbability);
            rewardButton.SetLuckyGoldUp(newLuckyGoldUp);
            UpdateCost(ref currentLuckyGoldUpCost);
            luckyGoldLevel++;
            Debug.Log($"골드 2배 확률 업!: {newLuckyGoldUp}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void LuckyGoldUp(int count)
    {
        float currentLuckyGoldUp = rewardButton.GetLuckyGoldUp();

        float totalLuckyGoldIncrease = increaseLuckyGold * count; // 요청한 횟수만큼 확률 증가
        float potentialLuckyGoldUp = currentLuckyGoldUp + totalLuckyGoldIncrease; // 증가 후의 잠재적인 확률

        if (currentLuckyGoldUp >= rewardButton.maxLuckyGoldProbability)
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }

        if (potentialLuckyGoldUp > rewardButton.maxLuckyGoldProbability)
        {
            Debug.Log($"요청한 증가량({count}번)으로 인해 확률이 최대치를 초과합니다. 가능한 최대치까지만 증가됩니다.");
            totalLuckyGoldIncrease = rewardButton.maxLuckyGoldProbability - currentLuckyGoldUp;
        }

        int totalCost = currentLuckyGoldUpCost * count; // 요청한 횟수만큼의 전체 비용 계산

        if (currencyManager.SubtractCurrency("Gold", totalCost))
        {
            float newLuckyGoldUp = currentLuckyGoldUp + totalLuckyGoldIncrease;
            rewardButton.SetLuckyGoldUp(newLuckyGoldUp);
            Debug.Log($"골드 2배 확률 업!: {newLuckyGoldUp}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


}

