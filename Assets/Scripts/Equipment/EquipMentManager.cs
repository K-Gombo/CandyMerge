using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{   
    public GameObject equipPrefab;
    public TextAsset CsvData { get; set; }
    public TextAsset LevelCsvData { get; set; } 
    public List<Equip> equipList = new List<Equip>();
    public Sprite[] equipSprites;
    public Sprite[] rankSprites;
    public Sprite[] slotSprites;
    public GachaManager GachaManager;
    public EquipSkillManager equipSkillManager;
    public EquipmentStatus equipmentStatus;
    public Queue<GameObject> equipPool = new Queue<GameObject>();
    public int poolSize = 40;
    public Transform equipMentPoolTransform;
    public Dictionary<string, Sprite> equipNameToSpriteMap = new Dictionary<string, Sprite>();
    public Dictionary<Rank, Sprite> rankToSpriteMap = new Dictionary<Rank, Sprite>();
    public Dictionary<SlotType, Sprite> slotToSpriteMap = new Dictionary<SlotType, Sprite>();
    public Dictionary<string, EquipLevelData> levelDataMap = new Dictionary<string, EquipLevelData>(); //LevelData 저장할 Dictionary
    
    
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
                    

                }
                catch (Exception e)
                {

                }

                equipList.Add(equip);
                // foreach (var equipment in equipList)
                // {
                //     Debug.Log($"Equip ID: {equipment.equipId}, Equip Name: {equipment.equipName}, SlotType: {equipment.slotType}, {equipment.skillIds[0]},{equipment.skillRanks[0]}");
                // }
            }
            InitializeEquipPool();
            AssignRandomRank();
            InitializeEquipSpriteMapping();
            InitializeSlotSpriteMapping();
            InitializeRankSpriteMapping();
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
                    Debug.LogError("CSV 파싱 에러: " + e.Message);
                }
            
                levelDataMap[levelData.rank] = levelData;
            }

            // 디버깅 코드 추가
            foreach (var pair in levelDataMap)
            {
                Debug.Log($"Key: {pair.Key}, StartLevel: {pair.Value.startLevel}");
            }
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
    
    void InitializeRankSpriteMapping()
    {
        
        Array ranks = Enum.GetValues(typeof(Rank));
        for(int i = 0; i < ranks.Length; i++)
        {
            rankToSpriteMap[(Rank)ranks.GetValue(i)] = rankSprites[i];
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
            
            // 랭크에 따라 배경 스프라이트 설정
            if (rankToSpriteMap.ContainsKey(chosenRank))
            {
                equipComponent.backgroundImageComponent.sprite = rankToSpriteMap[chosenRank];
            }

            // SlotType에 따라 슬롯 스프라이트 설정
            if (slotToSpriteMap.ContainsKey(equipComponent.slotType))
            {
                equipComponent.slotImageComponent.sprite = slotToSpriteMap[equipComponent.slotType];
            }
            
            // 초기 장비 레벨을 설정. 예를 들어, F 등급이면 시작 레벨이 1이 될 수 있습니다.
            if (levelDataMap.ContainsKey(chosenRank.ToString()))
            {
                EquipLevelData levelData = levelDataMap[chosenRank.ToString()];
                equipComponent.equipLevel = levelData.startLevel;  // 장비의 초기 레벨을 설정
                Debug.Log($"장비의 초기 레벨이 {levelData.startLevel}로 설정되었습니다.");
            }
            else
            {
                Debug.LogError($"등급 {chosenRank}에 해당하는 levelData가 없습니다.");
            }

             
        }
    }
    
    // 각 랭크의 최대 레벨을 관리하는 딕셔너리
    public static Dictionary<Rank, int> maxLevelsPerRank = new Dictionary<Rank, int>
    {
        { Rank.F, 1 }, // F는 rankLevel이 1이 최대이므로 바로 다음 등급으로 상승
        { Rank.D, 1 },
        { Rank.C, 2 }, // C는 rankLevel이 2가 최대이므로 2이상일 경우 B등급으로 상승
        { Rank.B, 2 },
        { Rank.A, 3 },
        { Rank.S, 3 },
        { Rank.SS, 4 },

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
    
    
    public void UpdateEquipLevelAndRank()
    {
        if (maxLevelsPerRank.ContainsKey(equipmentStatus.equipRank))
        {
            if (equipmentStatus.equipLevel >= maxLevelsPerRank[equipmentStatus.equipRank])
            {
                equipmentStatus.equipRank = GetNextRank(equipmentStatus.equipRank); // 등급을 다음 등급으로 변경
                equipmentStatus.equipLevel = 1;  // 레벨을 초기화
            }
            else
            {
                equipmentStatus.equipLevel++; // 레벨을 하나 올림
            }
        }
        else
        {
            // 해당 등급에 대한 정보가 없으면 그냥 레벨을 올림
            equipmentStatus.equipLevel++;
        }
    }
    
    public void UpdateEquipLevel()
    {
        if (equipmentStatus.equipLevel < maxLevelsPerRank[equipmentStatus.equipRank])
        {
            equipmentStatus.equipLevel++;
            UpdateEquipLevelAndRank();
        }
        else
        {
            equipmentStatus.equipRank = GetNextRank(equipmentStatus.equipRank);
            equipmentStatus.equipLevel = 1;
        }
        equipmentStatus.UpdateUI();
    }
    
}

