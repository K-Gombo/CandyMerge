using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public TextAsset CsvData { get; set; }
    public CandyController candyController; // CandyController에 대한 참조
    public EquipmentManager equipmentManager;
    public GameObject equipPrefab; // 생성할 장비의 프리팹
    public Transform equipSpawnLocation; // 장비가 생성될 위치
    public CandyManager candyManager;
    public GachaUIManager GachaUIManager; // GachaUIManager에 대한 참조 추가
    
    public Animator chestGachaAnimator;
    public GameObject mask;
    private GameObject equipmentClone; // 장비 복사본 저장
    private GameObject lastCreatedEquip;
    public Button autoGachaBtn;
    public Button equipKeyCountBtn;
    public GameObject equipKeyPanel;
    public Text equipGachaKeyCountText;
    public Text equipKeyGachaAvailavleText;
    
    public bool isAnimationInProgress = false;
    
    
    // 범위와 확률을 저장하는 딕셔너리
    Dictionary<string, Dictionary<string, float>> rankProbabilities = new Dictionary<string, Dictionary<string, float>>();

    private void Awake()
    {
        CsvData = Resources.Load<TextAsset>("EquipRank");
        var csvText = CsvData.text;
        var csvData = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // 헤더 읽기
        string[] headers = csvData[0].Split(',');

        for (int i = 1; i < csvData.Length; i++)
        {
            var line = csvData[i];
            var data = line.Split(',');
            var rangeString = data[0];
            Dictionary<string, float> rankInfo = new Dictionary<string, float>();

            for (int j = 1; j < headers.Length; j++)
            {
                rankInfo[headers[j]] = float.Parse(data[j]);
            }

            rankProbabilities[rangeString] = rankInfo;
        }
    }
    
    void Start()
    {
        // 이벤트 구독
        equipmentManager.OnEquipCreated += EquipCreated;
        autoGachaBtn.onClick.AddListener(() => {
            int candyCount = 3;
           
            candyController.MoveHighestLevelCandiesToMixBox(candyCount);
           
        });
        equipKeyCountBtn.onClick.AddListener(OnEquipKeyCountBtnClick);
    }
    
    private void Update()
    {
        // keyCount 값에 따라 버튼의 활성화 상태를 변경
        if (GiftBoxController.instance.keyCount < 5)
        {
            equipKeyCountBtn.interactable = false;
        }
        else
        {
            equipKeyCountBtn.interactable = true;
        }
    }
    
    public void OnEquipKeyCountBtnClick()
    {
        equipKeyPanel.SetActive(true);
        EquipGachaTextUpdate();
    }

    public void EquipGachaTextUpdate()
    {
        equipGachaKeyCountText.text = GiftBoxController.instance.keyCount.ToString();
        equipKeyGachaAvailavleText.text = (GiftBoxController.instance.keyCount / 5).ToString();
    }
    
    void EquipCreated(GameObject newEquip)
    {
        lastCreatedEquip = newEquip;
    }
    
    
    // MixBox에 캔디가 있는지 확인하고 있다면 원래 Box로 이동
    public bool CheckCandiesExistInMixBox()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox"); // 모든 MixBox를 찾음
        bool candyExistsInAnyMixBox = false;

        foreach (GameObject mixBox in mixBoxes)
        {
            Transform[] allChildren = mixBox.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("Candy"))
                {
                    candyExistsInAnyMixBox = true;
                    break;
                }
            }

            if (candyExistsInAnyMixBox)
            {
                break;
            }
        }

        return candyExistsInAnyMixBox;
    }

    // 문자열 형태의 레벨 범위를 List<int> 형태로 파싱
    public List<int> ParseRange(string range)
    {
        List<int> parsedLevels = new List<int>();
        string[] levels = range.Split('-');

        if (levels.Length == 2)
        {
            int start;
            int end;

            if (int.TryParse(levels[0], out start) && int.TryParse(levels[1], out end))
            {
                for (int i = start; i <= end; i++)
                {
                    parsedLevels.Add(i);
                }
            }
        }

        return parsedLevels;
    }

    // 전체 레벨에 따라 해당하는 레벨 범위를 찾기
    public string FindRange(int totalLevel)
    {
        foreach (var range in rankProbabilities.Keys)
        {
            List<int> levels = ParseRange(range);
            if (levels.Count > 0 && totalLevel >= levels[0] && totalLevel <= levels[levels.Count - 1])
            {
                return range;
            }
        }

        // 적절한 범위가 없으면 null 반환
        return null;
    }

    // 특정 범위와 등급에 해당하는 확률을 가져오기
    public float GetProbability(string range, string rank)
    {
        if (rankProbabilities.ContainsKey(range))
        {
            if (rankProbabilities[range].ContainsKey(rank))
            {
                return rankProbabilities[range][rank];
            }
        }

        return 0; // 없는 경우
    }

    // 총 레벨에 따른 등급 확률을 반환
    public float[] GetRankProbabilities(int totalLevel)
    {
        // totalLevel에 따라 적절한 range를 찾습니다.
        string range = FindRange(totalLevel);

        // 등급(F, D, C, B, A, S, SS)에 따른 확률을 배열에 저장
        float[] probabilities = new float[7];
        probabilities[0] = GetProbability(range, "F");
        probabilities[1] = GetProbability(range, "D");
        probabilities[2] = GetProbability(range, "C");
        probabilities[3] = GetProbability(range, "B");
        probabilities[4] = GetProbability(range, "A");
        probabilities[5] = GetProbability(range, "S");
        probabilities[6] = GetProbability(range, "SS");

        return probabilities;
    }
    
    // 캔디가 mixMox에 3개가 있는지 확인
    public bool CheckCandiesCount()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox");
        int totalCandyCount = 0;

        foreach (GameObject mixBox in mixBoxes)
        {
            totalCandyCount += mixBox.transform.childCount;
        }

        return totalCandyCount >= 3; // 캔디가 3개 이상이면 true를 반환
    }

    //장비 갓차
    public void EquipGacha()
    {
        if (CheckCandiesCount())
        {
            isAnimationInProgress = true;
            // PlayTrigger가 시작되기 전에 클론 생성
            int totalLevel = GachaUIManager.totalLevelSum; // GachaUIManager에서 캔디 총합 레벨을 가져옴
            float[] rankProbabilities = GetRankProbabilities(totalLevel);
            equipmentManager.CreateEquipPrefab(equipSpawnLocation, rankProbabilities);
            
            if (lastCreatedEquip == null)
            {
                return;
            }
            GameObject clone = Instantiate(lastCreatedEquip, mask.transform.position, Quaternion.identity, mask.transform);
    
            // 클론의 크기를 원본의 1.2배로 설정
            clone.transform.localScale = lastCreatedEquip.transform.localScale * 1.2f;
            

            // 애니메이션 트리거 설정
            chestGachaAnimator.SetTrigger("PlayTrigger");

            // 애니메이션이 끝난 후에 할 일
            StartCoroutine(WaitForAnimationToEnd(() => 
            {
                // 여기서 MixBox에 있는 모든 캔디를 반환
                ReturnAllCandiesInMixBox();

                // 애니메이션 종료
                chestGachaAnimator.SetTrigger("ExitTrigger");
                isAnimationInProgress = false; 
            }));
        }
    }
    
    private IEnumerator WaitForAnimationToEnd(Action callback)
    {
        yield return new WaitForSeconds(2.0f);
        callback();
    }

    // MixBox에 있는 모든 캔디를 반환하는 함수
    private void ReturnAllCandiesInMixBox()
    {
        GameObject[] mixBoxes = GameObject.FindGameObjectsWithTag("MixBox");
        foreach (GameObject mixBox in mixBoxes)
        {
            Transform[] allChildren = mixBox.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("Candy"))
                {
                    
                    candyManager.ReturnToPool(child.gameObject);
                }
            }
        }

    }
}
    


