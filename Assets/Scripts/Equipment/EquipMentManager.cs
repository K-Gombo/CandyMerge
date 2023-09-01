using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{  
    public GameObject equipPrefab;
    public TextAsset CsvData { get; set; }
    public List<Equip> equipList = new List<Equip>();
    public Sprite[] equipSprites;
    public MixManager mixManager;
    public Queue<GameObject> equipPool = new Queue<GameObject>();
    public int poolSize = 40;
    public Transform equipMentPoolTransform;
    // EquipmentManager 클래스 내부에 추가
    public Dictionary<string, Sprite> equipNameToSpriteMap = new Dictionary<string, Sprite>();

    
    // 장비 등급을 나타내는 enum
    public enum Rank { F, D, C, B, A, S, SS }
    // 장비 타입을 나타내는 enum
    public enum SlotType { Cook, Cap, Cloth, Shoes }
    
    
    [Serializable]
    public class Equip
    {
        public int equipId;
        public SlotType slotType;
        public string equipName;
        public Sprite equipSprite;
        public int[] skillIds = new int[4];
        public Rank[] skillRanks = new Rank[4];
        public Rank equipRank; // 랜덤으로 할당될 장비 등급
    }

    private void Awake()
    {
        CsvData = Resources.Load<TextAsset>("EquipmentData");

        if (CsvData != null)  // TextAsset이 로딩되었는지 확인
        {
            string csvText = CsvData.text;
            string[] csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 1; i < csvData.Length; i++)
            {
                string[] data = csvData[i].Split(',');
                
                Equip equip = new Equip();
    
                // 각각의 파싱 과정에서 문제가 발생할 수 있으므로 로그를 추가
                try {
                    equip.equipId = int.Parse(data[0]);
                    equip.slotType = (SlotType)Enum.Parse(typeof(SlotType), data[1]);
                    equip.equipName = data[2];
                    for (int j = 0; j < 4; j++)
                    {
                        equip.skillIds[j] = int.Parse(data[3 + j * 2]);
                        equip.skillRanks[j] = (Rank)Enum.Parse(typeof(Rank), data[4 + j * 2]);
                    }
                    
                } catch (Exception e) {
                    
                }
    
                equipList.Add(equip);
                // foreach (var equipment in equipList)
                // {
                //     Debug.Log($"Equip ID: {equipment.equipId}, Equip Name: {equipment.equipName}, SlotType: {equipment.slotType}, {equipment.skillIds[0]},{equipment.skillRanks[0]}");
                // }
              
            }

            AssignRandomRank();
            InitializeEquipSpriteMapping();
            InitializeEquipPool();
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
            // 등급을 랜덤으로 지정하는 로직 (나중에 확률을 고려해서 작성 가능)
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
        obj.SetActive(false);
        equipPool.Enqueue(obj);
    }

    
    
   public void CreateEquipPrefab(Transform parentTransform, float[] rankProbabilities)
{
    Debug.Log("CreateEquipPrefab 호출됨");
    // 랜덤 랭크 할당 로직
    Rank chosenRank = Rank.F;
    if (rankProbabilities != null)
    {
        Debug.Log("랭크 확률 배열이 null이 아닙니다.");
        float randValue = UnityEngine.Random.Range(0, 100);
        Debug.Log($"랜덤 값: {randValue}");
        
        float sum = 0;
        for (int i = 0; i < rankProbabilities.Length; i++)
        {
            sum += rankProbabilities[i];
            Debug.Log($"현재 합계: {sum}");
            if (randValue < sum)
            {
                chosenRank = (Rank)i;
                Debug.Log($"선택된 랭크: {chosenRank}");
                break;
            }
        }
    }
    else
    {
        Debug.Log("랭크 확률 배열이 null입니다.");
    }

    // equipList에서 랜덤으로 Equip 객체를 선택
    int randomIndex = UnityEngine.Random.Range(0, equipList.Count);
    Debug.Log($"랜덤 인덱스: {randomIndex}");

    Equip selectedEquip = equipList[randomIndex];
    Debug.Log($"선택된 Equip: {selectedEquip.equipName}");

    // 장비 프리팹을 풀에서 가져옴
    GameObject newEquip = GetEquipFromPool();
    newEquip.transform.SetParent(parentTransform);

    // 생성된 프리팹에 Equip 정보를 할당
    EquipMentStatus equipComponent = newEquip.GetComponent<EquipMentStatus>();
    if (equipComponent != null)
    {
        Debug.Log("EquipMentStatus 컴포넌트를 찾았습니다.");
        equipComponent.equipId = selectedEquip.equipId;
        equipComponent.slotType = selectedEquip.slotType;
        equipComponent.equipName = selectedEquip.equipName;
        equipComponent.skillIds = selectedEquip.skillIds;
        equipComponent.skillRanks = selectedEquip.skillRanks;
        equipComponent.equipRank = chosenRank;  // 랜덤으로 뽑힌 랭크를 할당합니다.

        // Image 컴포넌트에 sprite를 할당합니다.
        // Image 컴포넌트에 sprite를 할당합니다.
        if (equipNameToSpriteMap.ContainsKey(selectedEquip.equipName))
        {
            Debug.Log("매핑된 이미지 스프라이트를 찾았습니다.");
            equipComponent.imageComponent.sprite = equipNameToSpriteMap[selectedEquip.equipName];  
        }
        else
        {
            Debug.Log("매핑된 이미지 스프라이트를 찾을 수 없습니다.");
        }
    }
    else
    {
        Debug.Log("EquipMentStatus 컴포넌트를 찾을 수 없습니다.");
    }
}

}

