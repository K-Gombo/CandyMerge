using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class EquipmentManager : MonoBehaviour
{   
    public GameObject equipPrefab;
    public static EquipmentManager instance;
    public TextAsset CsvData { get; set; }
    public TextAsset LevelCsvData { get; set; } 
    public List<Equip> equipList = new List<Equip>();
    public Sprite[] equipSprites;
    public Color[] rankColors;
    public Sprite[] slotSprites;
    public CurrencyUI currencyUI; 
    public GachaManager GachaManager;
    public EquipSkillManager equipSkillManager;
    public EquipArrangeManager equipArrangeManager;
    public EquipmentStatus equipmentStatus;
    public Queue<GameObject> equipPool = new Queue<GameObject>();
    public int poolSize = 40;
    public Transform equipMentPoolTransform;
    public Dictionary<string, Sprite> equipNameToSpriteMap = new Dictionary<string, Sprite>();
    public Dictionary<Rank, Color> rankToColorMap = new Dictionary<Rank, Color>();
    public Dictionary<Rank, Color> skillRankToColorMap = new Dictionary<Rank, Color>();
    public Dictionary<SlotType, Sprite> slotToSpriteMap = new Dictionary<SlotType, Sprite>();
    public Dictionary<string, EquipLevelData> levelDataMap = new Dictionary<string, EquipLevelData>(); //LevelData 저장할 Dictionary
    public GameObject mixBtnMixAvailable;
    public GameObject allMixBtnMixAvailable;
    public Dictionary<Rank, Rank> rankUpMap;
    public GameObject[] equipSlotBoxes;
    public GameObject[] equipSlotBoxesImage;
    
    
    
    // 장비 등급을 나타내는 enum
    public enum Rank
    { F,D,C,B,A,S,SS,C1,B1,A1,A2,S1,S2,SS1,SS2,SS3}


    // 장비 타입을 나타내는 enum
    public enum SlotType
    { Cook, Cap, Cloth, Shoes }


    [Serializable]
    public class Equip
    {
        public int equipId;
        public SlotType slotType;
        public string equipName;
        public Sprite equipSprite;
        public int[] skillIds = new int[4];
        public string[] skillNames = new string[4];
        public float[] skillPoints = new float[4];
        public Rank[] skillRanks = new Rank[4];
        public string equipExplain;
        public Rank equipRank; // 랜덤으로 할당될 장비 등급
    }
    
    // EquipLevelData를 저장할 클래스
    [Serializable]
    public class EquipLevelData
    {
        public string rank;
        public int startLevel;
        public int maxLevel;
        public float startGoldGainIncrement;
        public float upgradeGoldGainIncrement;
        public float maxGoldGainIncrement;
        public int upgradeDefaultGoldCost;
    }

    private void Awake()
    {
        instance = this;
        CsvData = Resources.Load<TextAsset>("EquipmentData");
        LoadEquipmentData(); 
        
        LevelCsvData = Resources.Load<TextAsset>("EquipLevelData");
        if(LevelCsvData == null) {
            Debug.LogError("LevelCsvData 로딩 실패");
        } else {
            Debug.Log("LevelCsvData 로딩 성공");
        }
        LoadEquipLevelData();
    }
    
    private void LoadEquipmentData()
    {
        if (CsvData != null)
        {
            string csvText = CsvData.text;
            string[] csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            equipSkillManager = FindObjectOfType<EquipSkillManager>();
            for (int i = 1; i < csvData.Length; i++)
            {
                string[] data = csvData[i].Split(',');

                Equip equip = new Equip();


                try
                {
                    equip.equipId = int.Parse(data[0]);
                    equip.slotType = (SlotType)Enum.Parse(typeof(SlotType), data[1]);
                    equip.equipName = data[2];
                    for (int j = 0; j < 4; j++)
                    {
                        equip.skillIds[j] = int.Parse(data[3 + j * 2]);
                        equip.skillRanks[j] = (Rank)Enum.Parse(typeof(Rank), data[4 + j * 2]);

                        if (equipSkillManager.skillMap.ContainsKey(equip.skillIds[j]))
                        {
                            EquipSkill equipSkill = equipSkillManager.skillMap[equip.skillIds[j]];
                            equip.skillNames[j] = equipSkill.skillName;
                            equip.skillPoints[j] = equipSkill.skillPoint;
                        }
                    }

                    equip.equipExplain = data[11];

                }
                catch (Exception e)
                {
                    Debug.LogError("EquioDataCSV 파싱 에러: " + e.Message);
                }
                equipList.Add(equip);
              
            }
            InitializeEquipPool();
            AssignRandomRank();
            InitializeEquipSpriteMapping();
            InitializeSlotSpriteMapping();
            InitializeRankColorMapping();
            InitializeSkillRankColorMapping();
        }
    }
    
    private void LoadEquipLevelData()
    {
        if (LevelCsvData != null)
        {
            string csvText = LevelCsvData.text;
            string[] csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 1; i < csvData.Length; i++)
            {
                string[] data = csvData[i].Split(',');
                EquipLevelData levelData = new EquipLevelData();
            
                try
                {
                    levelData.rank = data[0];
                    levelData.startLevel = int.Parse(data[1]);
                    levelData.maxLevel = int.Parse(data[2]);
                    levelData.startGoldGainIncrement = float.Parse(data[3]);
                    levelData.upgradeGoldGainIncrement = float.Parse(data[4]);
                    levelData.maxGoldGainIncrement = float.Parse(data[5]);
                    levelData.upgradeDefaultGoldCost = int.Parse(data[6]);
                }
                catch (Exception e)
                {
                    // 에러 처리
                    Debug.LogError("EquipLevelCSV 파싱 에러: " + e.Message);
                }
            
                levelDataMap[levelData.rank] = levelData;
            }

            // // 디버깅 코드 추가
            // foreach (var pair in levelDataMap)
            // {
            //     Debug.Log($"Key: {pair.Key}, StartLevel: {pair.Value.startLevel}");
            // }
        }
        else
        {
            Debug.LogError("LevelCsvData가 null입니다.");
        }
    }



    void InitializeEquipSpriteMapping()
    {
        foreach (Equip equip in equipList)
        {
            if (equip.equipId >= 1 && equip.equipId <= equipSprites.Length)
            {
                equipNameToSpriteMap[equip.equipName] = equipSprites[equip.equipId - 1];
            }
        }
    }
    

    // 각 Equip 객체에 랜덤으로 장비 등급을 할당
    void AssignRandomRank()
    {
        foreach (var equip in equipList)
        {
            // 등급을 랜덤으로 지정
            Array ranks = Enum.GetValues(typeof(Rank));
            equip.equipRank = (Rank)ranks.GetValue(UnityEngine.Random.Range(0, ranks.Length));
        }
    }

    void InitializeEquipPool()
    {

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(equipPrefab, equipMentPoolTransform);
            obj.SetActive(false);
            equipPool.Enqueue(obj);

        }
    }


    public GameObject GetEquipFromPool()
    {
        if (equipPool.Count > 0)
        {
            GameObject obj = equipPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(equipPrefab);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnEquipToPool(GameObject obj)
    {
        obj.transform.SetParent(equipMentPoolTransform);
        obj.transform.localScale = Vector3.one; // 스케일 초기화
        obj.SetActive(false);
        equipPool.Enqueue(obj);
    }
    
    void InitializeRankColorMapping()
    {
        
        Array ranks = Enum.GetValues(typeof(Rank));
        for(int i = 0; i < ranks.Length; i++)
        {
            if (i == rankColors.Length) return;
            rankToColorMap[(Rank)ranks.GetValue(i)] = rankColors[i];
        }
    }
    
    void InitializeSkillRankColorMapping()
    {
        
        Array ranks = Enum.GetValues(typeof(Rank));
        for(int i = 0; i < ranks.Length; i++)
        {
            if (i == rankColors.Length) return;
            skillRankToColorMap[(Rank)ranks.GetValue(i)] = rankColors[i];
        }
    }
    
    void InitializeSlotSpriteMapping()
    {
        // 예를 들어, slotSprites 배열이 Cook, Cap, Cloth, Shoes 순서로 정렬되어 있다고 가정
        Array slotTypes = Enum.GetValues(typeof(SlotType));
        for(int i = 0; i < slotTypes.Length; i++)
        {
            slotToSpriteMap[(SlotType)slotTypes.GetValue(i)] = slotSprites[i];
        }
    }
    
    
    public void CreateEquipPrefab(Transform parentTransform, float[] rankProbabilities)
    {
        
        // 랜덤 랭크 할당 로직
        Rank chosenRank = Rank.F;
        if (rankProbabilities != null)
        {
            float randValue = UnityEngine.Random.Range(0, 100);
            float sum = 0;
            for (int i = 0; i < rankProbabilities.Length; i++)
            {
                sum += rankProbabilities[i];
               
                if (randValue < sum)
                {
                    chosenRank = (Rank)i;
                    break;
                }
            }
        }
        // equipList에서 랜덤으로 Equip 객체 선택
        int randomIndex = UnityEngine.Random.Range(0, equipList.Count);
        Equip selectedEquip = equipList[randomIndex];

        // 장비 프리팹을 풀에서 가져옴
        GameObject newEquip = GetEquipFromPool();
        newEquip.transform.SetParent(parentTransform, false);
        
        // 생성된 프리팹에 Equip 정보를 할당
        EquipmentStatus equipComponent = newEquip.GetComponent<EquipmentStatus>();
        if (equipComponent != null)
        {
            equipComponent.equipId = selectedEquip.equipId;
            equipComponent.slotType = selectedEquip.slotType;
            equipComponent.equipName = selectedEquip.equipName;
            equipComponent.skillIds = selectedEquip.skillIds;
            equipComponent.skillRanks = selectedEquip.skillRanks;
            equipComponent.equipRank = chosenRank;
            equipComponent.equipExplain = selectedEquip.equipExplain;

            for (int i = 0; i < 4; i++)
            {
                if (equipSkillManager.skillMap.ContainsKey(selectedEquip.skillIds[i]))
                {
                    
                    EquipSkill equipSkill = equipSkillManager.skillMap[selectedEquip.skillIds[i]];
                    equipComponent.skillNames[i] = equipSkill.skillName;
                    equipComponent.skillPoints[i] = equipSkill.skillPoint;
                }
            }
            // 장비에 따라 스프라이트 설정
            if (equipNameToSpriteMap.ContainsKey(selectedEquip.equipName))
            {
                equipComponent.imageComponent.sprite = equipNameToSpriteMap[selectedEquip.equipName];
            }
            // 랭크에 따라 배경 컬러 설정
            if (rankToColorMap.ContainsKey(chosenRank))
            {
                equipComponent.backgroundImageComponent.color = rankToColorMap[chosenRank];
                equipComponent.levelCircleComponent.color = rankToColorMap[chosenRank];
                equipComponent.slotBarComponent.color = rankToColorMap[chosenRank];
            }

            // SlotType에 따라 슬롯 스프라이트 설정
            if (slotToSpriteMap.ContainsKey(equipComponent.slotType))
            {
                equipComponent.slotImageComponent.sprite = slotToSpriteMap[equipComponent.slotType];
            }
           
            if (levelDataMap.ContainsKey(chosenRank.ToString()))
            {
                EquipLevelData levelData = levelDataMap[chosenRank.ToString()];
                equipComponent.equipLevel = levelData.startLevel;
                equipComponent.maxEquipLevel = levelData.maxLevel;  // 최대 레벨 설정
                equipComponent.rankLevel = 0;
                equipComponent.goldIncrement = levelData.startGoldGainIncrement;
                equipComponent.upgradeGoldIncrement = levelData.upgradeGoldGainIncrement;
                equipComponent.maxGoldIncrement = levelData.maxGoldGainIncrement;
                equipComponent.upgradeGoldCost = levelData.upgradeDefaultGoldCost;
                
                SetRankLevelSlotActive(equipComponent.rankLevel ,equipComponent.rankLevelSlot);
            }
        }
    }
    
    
    public static Dictionary<Rank, int> maxLevelsPerRank = new Dictionary<Rank, int>
    {
        { Rank.F, 0 },
        { Rank.D, 0 },
        { Rank.C, 0 },
        { Rank.C1, 1 },
        { Rank.B, 0 },
        { Rank.B1, 0 },
        { Rank.A, 0 },
        { Rank.A1, 1 },
        { Rank.A2, 2 },
        { Rank.S, 0 },
        { Rank.S1, 1 },
        { Rank.S2, 2 },
        { Rank.SS, 0 },
        { Rank.SS1, 1 },
        { Rank.SS2, 2 },
        { Rank.SS3, 3 }
    };
    
    public Rank GetNextRank(Rank currentRank)
    {
        Dictionary<Rank, Rank> rankUpMap = new Dictionary<Rank, Rank>
        {
            {Rank.F, Rank.D},
            {Rank.D, Rank.C},
            {Rank.C, Rank.C1},
            {Rank.C1, Rank.B},
            {Rank.B, Rank.B1},
            {Rank.B1, Rank.A},
            {Rank.A, Rank.A1},
            {Rank.A1, Rank.A2},
            {Rank.A2, Rank.S},
            {Rank.S, Rank.S1},
            {Rank.S1, Rank.S2},
            {Rank.S2, Rank.SS},
            {Rank.SS, Rank.SS1},
            {Rank.SS1, Rank.SS2},
            {Rank.SS2, Rank.SS3},
            {Rank.SS3, Rank.SS3}  // 최고 등급이므로 그대로 유지
        };

        if (rankUpMap.ContainsKey(currentRank))
        {
            return rankUpMap[currentRank];
        }

        return currentRank; // 목록에 없는 등급은 그대로 반환
    }
    
    public void UpdateRankLevelOnMerge()
    {
        // EquipmentController의 equipMixBoxes를 가져옵니다.
        Transform[] equipMixBoxes = EquipmentController.instance.equipMixBoxes;
    
        // 모든 EquipMixBoxes가 채워져 있는지 확인
        bool areAllEquipMixBoxesFilled = EquipmentController.instance.AreAllEquipMixBoxesFilled();
    
        // 모든 EquipMixBoxes가 채워져 있다면
        if (areAllEquipMixBoxesFilled)
        {
            EquipmentStatus mainEquipment = equipMixBoxes[0].GetChild(0).GetComponent<EquipmentStatus>().originalEquipment;
        
            int maxRankLevel = maxLevelsPerRank.ContainsKey(mainEquipment.equipRank) ? maxLevelsPerRank[mainEquipment.equipRank] : 0;

            if (mainEquipment.rankLevel >= maxRankLevel)
            {
                Rank nextRank = GetNextRank(mainEquipment.equipRank);
                mainEquipment.rankLevel = nextRank == mainEquipment.equipRank ? 0 : ConvertRankToLevel(nextRank);
                mainEquipment.equipRank = nextRank;
            }
            else
            {
                mainEquipment.rankLevel++;
            }
            
            SetRankLevelSlotActive(mainEquipment.rankLevel, mainEquipment.rankLevelSlot);

            // 새로운 등급의 시작 레벨과 최대 레벨을 설정
            if (levelDataMap.ContainsKey(mainEquipment.equipRank.ToString()))
            {
                EquipLevelData levelData = levelDataMap[mainEquipment.equipRank.ToString()];
                mainEquipment.equipLevel = levelData.startLevel;
            }
            
            // 등급에 따라 색상 업데이트
            if (rankToColorMap.ContainsKey(mainEquipment.equipRank))
            {
                Color newColor = rankToColorMap[mainEquipment.equipRank];
                mainEquipment.backgroundImageComponent.color = newColor;
                mainEquipment.levelCircleComponent.color = newColor;
                mainEquipment.slotBarComponent.color = newColor;
            }
            
            mainEquipment.UpdateLevelUI();
            
            // EquipMixbox[1]과 EquipMixbox[2]의 원본을 풀로 리턴하고 클론 제거
            for (int i = 1; i <= 2; i++)
            {
                GameObject cloneObj = equipMixBoxes[i].GetChild(0).gameObject;
                GameObject originalObj = cloneObj.GetComponent<EquipmentStatus>().originalEquipment.gameObject;

                Destroy(cloneObj);
                ReturnEquipToPool(originalObj);
            }

            // EquipMixbox[0]의 클론 제거
            Destroy(equipMixBoxes[0].GetChild(0).gameObject);
        }
        CheckMixAvailability();
    }
    
    public int ConvertRankToLevel(Rank rank)
    {
        if (rank == Rank.C1 ||rank == Rank.B1 || rank == Rank.A1 || rank == Rank.S1 || rank == Rank.SS1) return 1;
        if (rank == Rank.A2 || rank == Rank.S2 || rank == Rank.SS2) return 2;
        if (rank == Rank.SS3) return 3;
        return 0; // 기본값
    }
    
    // rankLevelSlot을 설정하는 메서드
    public void SetRankLevelSlotActive(int rankLevel, GameObject rankLevelSlot)
    {
        if (rankLevel == 0)
        {
            rankLevelSlot.SetActive(false);
        }
        else
        {   
            rankLevelSlot.SetActive(true);
        }
    }
    
    public Dictionary<Rank, float> startGoldIncrementMap = new Dictionary<Rank, float>
    {
        {Rank.F, 0},
        {Rank.D, 0.4f},
        {Rank.C, 1.48f},
        {Rank.C1, 3.44f},
        {Rank.B, 5.68f},
        {Rank.B1, 9.1f},
        {Rank.A, 12.9f},
        {Rank.A1, 18.18f},
        {Rank.A2, 23.94f},
        {Rank.S, 30.18f},
        {Rank.S1, 38.3f},
        {Rank.S2, 47f},
        {Rank.SS, 56.28f},
        {Rank.SS1, 67.84f},
        {Rank.SS2, 81.88f},
        {Rank.SS3, 98.6f},
    };
    
    public float GetNextGoldIncrement(Rank currentRank)
    {
        Rank nextRank = GetNextRank(currentRank);
        return startGoldIncrementMap.ContainsKey(nextRank) ? startGoldIncrementMap[nextRank] : 0f;
    }
    
    public void CheckMixAvailability()
    {
        // 장비 리스트 업데이트
        equipArrangeManager.UpdateEquipList();

        // 같은 Rank와 equipName을 가진 장비를 카운트하기 위한 딕셔너리
        Dictionary<string, int> equipCountDictionary = new Dictionary<string, int>();
        
        foreach (EquipmentStatus equipment in equipArrangeManager.equipList)
        {
            // Rank와 equipName을 합쳐서 키를 만듦
            string key = equipment.equipRank + "_" + equipment.equipName;

            if (equipCountDictionary.ContainsKey(key))
            {
                equipCountDictionary[key]++;
            }
            else
            {
                equipCountDictionary[key] = 1;
            }
        }

        // 같은 Rank와 equipName을 가진 장비가 3개 이상 있는지 확인
        bool mixAvailable = equipCountDictionary.Any(entry => entry.Value >= 3);

        // mixBtnMixAvailable 버튼 활성화 또는 비활성화
        mixBtnMixAvailable.SetActive(mixAvailable);
        allMixBtnMixAvailable.SetActive(mixAvailable);

        // 각 EquipmentStatus에 대한 mixAvailable 적용
        foreach (EquipmentStatus equipment in equipArrangeManager.equipList)
        {
            string key = equipment.equipRank + "_" + equipment.equipName;
            bool shouldActivate = equipCountDictionary.ContainsKey(key) && equipCountDictionary[key] >= 3;
            equipment.mixAvailable.SetActive(shouldActivate);
        }
    }
    
    
    public void AllMergeEquipments()
    {
        // 같은 Rank와 equipName을 가진 장비를 카운트하기 위한 딕셔너리
        Dictionary<string, List<EquipmentStatus>> equipGroups = new Dictionary<string, List<EquipmentStatus>>();

        foreach (EquipmentStatus equipment in equipArrangeManager.equipList)
        {
          
            string key = equipment.equipRank + "_" + equipment.equipName;

            if (equipGroups.ContainsKey(key))
            {
                equipGroups[key].Add(equipment);
            }
            else
            {
                equipGroups[key] = new List<EquipmentStatus> { equipment };
            }
        }

        // 3개 이상 동일한 조건의 장비가 있으면 합성
        foreach (var group in equipGroups)
        {
            while (group.Value.Count >= 3)
            {
                // 첫 번째 장비를 기본으로 삼아 업그레이드
                EquipmentStatus mainEquipment = group.Value[0];

                // 나머지 로직은 UpdateRankLevelOnMerge 메서드와 유사
                int maxRankLevel = maxLevelsPerRank.ContainsKey(mainEquipment.equipRank) ? maxLevelsPerRank[mainEquipment.equipRank] : 0;

                if (mainEquipment.rankLevel >= maxRankLevel)
                {
                    Rank nextRank = GetNextRank(mainEquipment.equipRank);
                    mainEquipment.rankLevel = nextRank == mainEquipment.equipRank ? 0 : ConvertRankToLevel(nextRank);
                    mainEquipment.equipRank = nextRank;
                }
                else
                {
                    mainEquipment.rankLevel++;
                }

                SetRankLevelSlotActive(mainEquipment.rankLevel, mainEquipment.rankLevelSlot);
                mainEquipment.UpdateLevelUI();

                // 2개의 장비를 풀로 리턴
                for (int i = 1; i <= 2; i++)
                {
                    GameObject originalObj = group.Value[i].gameObject;
                    ReturnEquipToPool(originalObj);
                }

                // 합성한 장비를 리스트에서 제거
                group.Value.RemoveRange(0, 3);
            }
        }
        CheckMixAvailability();
    }

    public void EquipLevelUpgrade(EquipmentStatus equipment)
    {   
        // CurrencyManager에서 현재 유저의 골드 양을 가져온다.
        string UserGold = CurrencyManager.instance.GetCurrencyAmount("Gold");

        BigInteger currentGoldAmount = BigInteger.Parse(UserGold);
        BigInteger costAmount = BigInteger.Parse(equipment.upgradeGoldCost.ToString());

        // 업그레이드 가능한지 검사 (골드는 따로 확인해야 함)
        if (equipment.equipLevel >= equipment.maxEquipLevel)
        {
            Debug.Log("이미 최대 레벨입니다.");
            return;
        }
        // 업그레이드 비용 확인 (골드를 소지하고 있는지)
        if (currentGoldAmount < costAmount) 
        {
            Debug.Log("골드가 부족합니다.");
            
            return;
        }

        // 업그레이드 처리
        equipment.equipLevel++;
        
        // CurrencyManager를 통해 골드를 차감한다.
        CurrencyManager.instance.SubtractCurrency("Gold", equipment.upgradeGoldCost); 

        // 골드 증가량 업데이트
        equipment.goldIncrement += equipment.upgradeGoldIncrement;

        if (equipment.goldIncrement > equipment.maxGoldIncrement)
        {
            equipment.goldIncrement = equipment.maxGoldIncrement;
        }

        // 업그레이드 비용 증가 (2배)
        equipment.upgradeGoldCost *= 2; 
        // 현재 유저 골드 업데이트
        currencyUI.goldText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(CurrencyManager.instance.GetCurrencyAmount("Gold").ToString());
        EquipmentController.instance.EquipStatusUpdate(equipment);
        
        equipment.UpdateLevelUI();
        
        GameObject currentClone = EquipmentController.instance.currentClone;
        
        if (currentClone != null) 
        {
            EquipmentStatus cloneStatus = currentClone.GetComponent<EquipmentStatus>();
            if (cloneStatus != null) 
            {   
                cloneStatus.equipLevel = equipment.equipLevel; 
                cloneStatus.UpdateLevelUI(); 
            }
        }

        
    }

    public void EquipmentSlotEquip(EquipmentStatus equipmentStatus)
    {
        if (equipmentStatus == null) return;

        // 원래의 부모와 크기를 저장
        equipmentStatus.originalParent = equipmentStatus.transform.parent;
        equipmentStatus.originalScale = equipmentStatus.transform.localScale;

        int slotIndex = (int) equipmentStatus.slotType;
        if (slotIndex >= 0 && slotIndex < equipSlotBoxes.Length)
        {
            // 해당 슬롯에 이미 장비가 장착되어 있는지 확인
            if (equipSlotBoxes[slotIndex].transform.childCount > 0)
            {
                // 이미 장착된 장비를 해제
                EquipmentStatus equippedItem = equipSlotBoxes[slotIndex].transform.GetChild(0).GetComponent<EquipmentStatus>();
                if (equippedItem)
                {
                    EquipmentSlotUnequip(equippedItem);
                }
            }

            // 새로운 장비를 장착
            equipmentStatus.transform.SetParent(equipSlotBoxes[slotIndex].transform);

            // 장비 크기를 1.3배로 변경
            equipmentStatus.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

            // 장비를 부모의 정중앙에 위치시킴
            equipmentStatus.transform.localPosition = Vector3.zero;
            
            equipSlotBoxesImage[slotIndex].SetActive(false);

            equipmentStatus.isEquipped = true;
            
            EquipGoldUp(equipmentStatus);
            equipSkillManager.EquipLuckyGoldUp(equipmentStatus);
            equipSkillManager.EquipLuckyCandyLevelUp(equipmentStatus);
            equipSkillManager.EquipLuckyExperienceUp(equipmentStatus);
            equipSkillManager.EquipEquipQuestDiaUp(equipmentStatus);
            equipSkillManager.EquipEquipLuckyDiaQuestUp(equipmentStatus);
        }
        else
        {
            Debug.LogError("Invalid slot index: " + slotIndex);
        }
    }


    
    
    public void EquipmentSlotUnequip(EquipmentStatus equipmentStatus)
    {
        if (equipmentStatus == null || equipmentStatus.originalParent == null) return;

        // 원래의 부모로 장비를 이동
        equipmentStatus.transform.SetParent(equipmentStatus.originalParent);

        // 원래의 크기로 장비를 변경
        equipmentStatus.transform.localScale = equipmentStatus.originalScale;

        // 원래의 위치로 장비를 이동
        equipmentStatus.transform.localPosition = Vector3.zero;
        
        equipSlotBoxesImage[(int)equipmentStatus.slotType].SetActive(true);
        
        equipmentStatus.isEquipped = false;
        
        RewardButton.instance.ResetEquipGoldUp(equipmentStatus.goldIncrement);
        RewardButton.instance.ResetEquipLuckyGoldUp(equipmentStatus);
        CandyController.instance.ResetEquipLuckyCandyLevelUp(equipmentStatus);
        QuestManager.instance.ResetEquipLuckyExperienceUp(equipmentStatus);
        RewardButton.instance.ResetEquipQuestDiaUp(equipmentStatus);
        Quest.instance.ResetEquipLuckyDiaQuestUp(equipmentStatus);


    }
    
    public void EquipGoldUp(EquipmentStatus equipment)
    {
        float currentEquipGoldUp = RewardButton.instance.GetEquipGoldUp();
        
        float newEquipGoldUp = currentEquipGoldUp + equipment.goldIncrement;
        RewardButton.instance.SetEquipGoldUp(newEquipGoldUp);
        Debug.Log($"추가 골드 획득 업!: {newEquipGoldUp}");
    }
    
    
}


