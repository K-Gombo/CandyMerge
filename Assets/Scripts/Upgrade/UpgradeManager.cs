using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeManager : MonoBehaviour
{
    public GiftBoxController giftBoxController; // GiftBoxController 참조
    public CandyStatus candyStatus;
    public CandyController candyController;
    public BoxManager boxManager;
    
    public float decreaseFilltime = 0.5f; // 캔디 생성 속도 감소조절 
    public float increaseLuckyCreate = 0.5f;
    public int increaseMaxCandies = 1;
    public int increaseBaseLevel = 1;
    public float increasePassiveAutoMergeSpeed = 0.2f;
    public float increasePassiveAutoCreateSpeed = 0.2f;
    
    public float costMultiplier = 2.0f; //업그레이드 비용 증가 비율
    
    public CurrencyManager currencyManager; 
    public int LuckyCreateUpCost = 800; 
    public int CreateSpeedUpCost = 1000;
    public int RemoveLockedCost = 1000;
    public int MaxCandiesUpCost = 1000;
    public int CandyLevelUpCost = 800;
    public int PassiveAutoMergeSpeedUpCost = 900;
    public int PassiveAutoCreateSpeedUpCost = 900;
    
    // 각 업그레이드의 현재 비용
    public int currentLuckyCreateUpCost;
    public int currentCreateSpeedUpCost;
    public int currentRemoveLockedCost;
    public int currentMaxCandiesUpCost;
    public int currentCandyLevelUpCost;
    public int currentPassiveAutoMergeSpeedUpCost;
    public int currentPassiveAutoCreateSpeedUpCost;
    private void Start()
    {
        // 초기 비용 설정
        currentLuckyCreateUpCost = LuckyCreateUpCost;
        currentCreateSpeedUpCost = CreateSpeedUpCost;
        currentRemoveLockedCost = RemoveLockedCost;
        currentMaxCandiesUpCost = MaxCandiesUpCost;
        currentCandyLevelUpCost = CandyLevelUpCost;
        currentPassiveAutoMergeSpeedUpCost = PassiveAutoMergeSpeedUpCost;
        currentPassiveAutoCreateSpeedUpCost = PassiveAutoCreateSpeedUpCost;
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
            Debug.Log($"캔디 확률 업!: {newLuckyCreate}");
            
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
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
            Debug.Log($"생산 쿨타임 감소!:{newFillTime}");
            
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
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
            Debug.Log($"캔디 생성 증가!:{newMaxCandies}");
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
            Debug.Log($"캔디 레벨업!:{newBaseLevel}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void PassiveAutoMergeSpeedUp() //패시브 자동 머지 속도 업 (스킬6)
    {
        float currentPassiveWating = candyController.GetPassiveWating();
        if (currentPassiveWating >= candyController.maxPassiveWating) 
        {
            Debug.Log("이미 최대로 업그레이드 되었습니다.");
            return;
        }
        if (currencyManager.SubtractCurrency("Gold", currentPassiveAutoMergeSpeedUpCost))
        {
            float newPassiveAutoMergeSpeed = Mathf.Min(currentPassiveWating + increasePassiveAutoMergeSpeed, candyController.maxPassiveWating);
            candyController.SetPassiveWating(newPassiveAutoMergeSpeed);
            UpdateCost(ref currentPassiveAutoMergeSpeedUpCost);
            Debug.Log($"자동 머지 속도업!:{newPassiveAutoMergeSpeed}");
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
            Debug.Log($"자동 생성 속도업!:{newPassiveAutoCreateSpeed}");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    
    
    
}

