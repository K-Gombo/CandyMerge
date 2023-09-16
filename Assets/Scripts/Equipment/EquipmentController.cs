using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public GameObject equipMixPanel;
    public Transform[] equipMixBoxes;
    public static EquipmentController instance;
    public GameObject mixLockedPanel;
    public GameObject equipMixBtn;
    public EquipArrangeManager equipArrangeManager;
    public GameObject backBtn;
    public Transform equipMixResultBox;
    public EquipmentManager equipmentManager;public GameObject equipNameExplain;
    public Text equipNameExplainText;
    public GameObject equipRankExplain;
    public Text equipRankExplainText;
    public GameObject equipArrangeBtnGroup;
    public GameObject equipExplain;
    

    void Awake()
    {
        instance = this;

        // backBtn에 이벤트 연결
        Button backBtnComponent = backBtn.GetComponent<Button>();
        backBtnComponent.onClick.AddListener(OnbackBtnClick);
    }

    public void OnEquipmentClick(EquipmentStatus clickedEquipment)
{
    if (!clickedEquipment.isOriginal)
    {
        return;
    }

    if (clickedEquipment.myClone != null)
    {
        return;
    }

    if (equipMixPanel.activeSelf)
    {
        GameObject clone = null;
        Transform targetParent = null;
        equipArrangeBtnGroup.SetActive(false);

        if (equipMixBoxes[0].childCount == 0)
        {
            targetParent = equipMixBoxes[0];
            mixLockedPanel.SetActive(true); 
            equipMixBtn.SetActive(true); 
            if (!equipMixBoxes[1].gameObject.activeSelf && !equipMixBoxes[2].gameObject.activeSelf)
            {
                ActivateAndMoveBoxes();
            }
        }
        else if (equipMixBoxes[0].childCount > 0 && equipMixBoxes[1].gameObject.activeSelf && equipMixBoxes[2].gameObject.activeSelf)
        {
            if (equipMixBoxes[1].childCount == 0)
            {
                targetParent = equipMixBoxes[1];
            }
            else if (equipMixBoxes[2].childCount == 0)
            {
                targetParent = equipMixBoxes[2];
            }
        }

        if (targetParent != null)
        {
            clone = Instantiate(clickedEquipment.gameObject);
            EquipmentStatus cloneStatus = clone.GetComponent<EquipmentStatus>();
            cloneStatus.isOriginal = false;
            cloneStatus.originalEquipment = clickedEquipment;

            Button originalButton = clickedEquipment.GetComponent<Button>();
            originalButton.onClick.RemoveAllListeners();
            originalButton.onClick.AddListener(() => OnEquipmentClick(clickedEquipment));

            Button cloneButton = clone.GetComponent<Button>();
            cloneButton.onClick.AddListener(() => OnCloneClick(cloneStatus));

            RectTransform cloneRect = clone.GetComponent<RectTransform>();
            RectTransform parentRect = targetParent.GetComponent<RectTransform>();

            cloneRect.anchorMin = parentRect.anchorMin;
            cloneRect.anchorMax = parentRect.anchorMax;
            cloneRect.anchoredPosition3D = Vector3.zero;
            cloneRect.sizeDelta = parentRect.sizeDelta;
            cloneRect.localScale = new Vector3(
                clickedEquipment.transform.localScale.x * 1.2f,
                clickedEquipment.transform.localScale.y * 1.2f,
                clickedEquipment.transform.localScale.z * 1.2f
            );

            clone.transform.SetParent(targetParent, false);
            clone.transform.localPosition = Vector3.zero;

            clickedEquipment.myClone = clone;
            
            // FilterByRankAndName 메서드 호출
            equipArrangeManager.FilterByRankAndName(clickedEquipment.equipRank, clickedEquipment.equipName , clickedEquipment.rankLevel);
            // 선택한 장비의 rankLevel이 1 높은 클론을 equipMixResultBox에 생성
            if (equipMixResultBox.childCount == 0)  // 자식 오브젝트가 없을 때만 클론 생성
            {   
                GameObject previewClone = Instantiate(clickedEquipment.gameObject);
                EquipmentStatus previewCloneStatus = previewClone.GetComponent<EquipmentStatus>();
                previewCloneStatus.isOriginal = false;

                // 여기에 scale을 1.3배로 설정
                previewClone.transform.localScale = new Vector3(
                    clickedEquipment.transform.localScale.x * 1.3f,
                    clickedEquipment.transform.localScale.y * 1.3f,
                    clickedEquipment.transform.localScale.z * 1.3f
                );

                // 새로운 rankLevel과 equipRank을 계산
                // 새로운 rankLevel과 equipRank을 계산
                int maxRankLevel = EquipmentManager.maxLevelsPerRank.ContainsKey(clickedEquipment.equipRank) ? EquipmentManager.maxLevelsPerRank[clickedEquipment.equipRank] : 0;
                int newRankLevel;
                if (clickedEquipment.rankLevel >= maxRankLevel)
                {
                    EquipmentManager.Rank nextRank = equipmentManager.GetNextRank(clickedEquipment.equipRank);
                    newRankLevel = equipmentManager.ConvertRankToLevel(nextRank);
                    previewCloneStatus.equipRank = nextRank;
                }
                else
                {
                    newRankLevel = clickedEquipment.rankLevel + 1;
                }
                previewCloneStatus.rankLevel = newRankLevel;

                // 색상 업데이트
                if (equipmentManager.rankToColorMap.ContainsKey(previewCloneStatus.equipRank))
                {
                    Color newColor = equipmentManager.rankToColorMap[previewCloneStatus.equipRank];
                    previewCloneStatus.backgroundImageComponent.color = newColor;
                    previewCloneStatus.levelCircleComponent.color = newColor;
                    previewCloneStatus.slotBarComponent.color = newColor;
                }

                // UI 업데이트
                previewCloneStatus.UpdateLevelUI();

                // 여기에 SetRankLevelSlotActive 메서드 호출을 추가
                equipmentManager.SetRankLevelSlotActive(newRankLevel, previewCloneStatus.rankLevelSlot);

                previewClone.transform.SetParent(equipMixResultBox, false);
                previewClone.transform.localPosition = Vector3.zero;
            }
            else
            {   
                GameObject existingPreviewClone = equipMixResultBox.GetChild(0).gameObject;
                if (!existingPreviewClone.activeSelf)
                {
                    existingPreviewClone.SetActive(true);  // 비활성화된 상태라면 활성화
                }
            }
            
            // 원본에 있는 특정 오브젝트 활성화
            clickedEquipment.touchLock1.SetActive(true);
            if (clickedEquipment.rankLevel != 0)
            {
                clickedEquipment.touchLock2.SetActive(true);
            }
            clickedEquipment.check.SetActive(true);
        }
        equipExplain.SetActive(false);
        equipNameExplain.SetActive(true);
        equipRankExplain.SetActive(true);
        EquipExplainUpdate(clickedEquipment);
    }
    
}


    private void ActivateAndMoveBoxes()
    {
        for (int i = 1; i <= 2; i++)
        {
            Transform targetMixBox = equipMixBoxes[i];
            Vector3 originalLocalPosition = targetMixBox.localPosition;
            Vector3 newLocalPosition = originalLocalPosition + new Vector3(900f, 0, 0);

            targetMixBox.localPosition = newLocalPosition;
            targetMixBox.gameObject.SetActive(true);

            StartCoroutine(MoveToOriginalPosition(targetMixBox, newLocalPosition, originalLocalPosition));
        }
    }

    IEnumerator MoveToOriginalPosition(Transform targetTransform, Vector3 startLocalPos, Vector3 endLocalPos)
    {
        float journeyTime = 0.2f;
        float startTime = Time.time;

        while (Time.time < startTime + journeyTime)
        {
            float fracComplete = (Time.time - startTime) / journeyTime;
            targetTransform.localPosition = Vector3.Lerp(startLocalPos, endLocalPos, fracComplete);
            yield return null;
        }

        targetTransform.localPosition = endLocalPos;
    }

    public void OnCloneClick(EquipmentStatus clickedClone)
    {
        if (clickedClone.isOriginal)
        {
            return;
        }

        if (clickedClone.myClone != null)
        {
            clickedClone.myClone.GetComponent<EquipmentStatus>().myClone = null;
        }

        if (clickedClone.transform.parent == equipMixBoxes[0])
        {
            // equipMixBoxes[1]에 있는 클론들의 원본 객체들을 비활성화
            foreach (Transform child in equipMixBoxes[1])
            {
                EquipmentStatus original = child.GetComponent<EquipmentStatus>().originalEquipment;
                if (original != null)
                {
                    original.touchLock1.SetActive(false);
                    original.touchLock2.SetActive(false);
                    original.check.SetActive(false);
                }
                Destroy(child.gameObject);
            }

            // equipMixBoxes[2]에 있는 클론들의 원본 객체들을 비활성화
            foreach (Transform child in equipMixBoxes[2])
            {
                EquipmentStatus original = child.GetComponent<EquipmentStatus>().originalEquipment;
                if (original != null)
                {
                    original.touchLock1.SetActive(false);
                    original.touchLock2.SetActive(false);
                    original.check.SetActive(false);
                }
                Destroy(child.gameObject);
            }
            
            // equipMixResultBox에 있는 previewClone을 삭제
            if (equipMixResultBox.childCount > 0)
            {
                Destroy(equipMixResultBox.GetChild(0).gameObject);
            }

            equipMixBoxes[1].gameObject.SetActive(false);
            equipMixBoxes[2].gameObject.SetActive(false);
            mixLockedPanel.SetActive(false); // mixLockedPanel 비활성화
            equipMixBtn.SetActive(false); 
            equipArrangeManager.SortByRank();
            equipArrangeBtnGroup.SetActive(true);
            equipExplain.SetActive(true);
            equipNameExplain.SetActive(false);
            equipRankExplain.SetActive(false);
        }

        // 원본의 특정 오브젝트 비활성화
        if (clickedClone.originalEquipment != null)
        {
            clickedClone.originalEquipment.touchLock1.SetActive(false);
            clickedClone.originalEquipment.touchLock2.SetActive(false);
            clickedClone.originalEquipment.check.SetActive(false);
        }
        Destroy(clickedClone.gameObject);
    }
    
    public bool AreAllEquipMixBoxesFilled()
    {
        for (int i = 0; i < 3; i++)
        {
            if (equipMixBoxes[i].childCount == 0)
            {
                return false;
            }
        }
        return true;
    }
    
    
    private void DestroyAllClonesAndDeactivate(Transform equipMixBox)
    {
        foreach (Transform child in equipMixBox)
        {
            EquipmentStatus clickedClone = child.GetComponent<EquipmentStatus>();
            if (clickedClone != null && clickedClone.originalEquipment != null)
            {
                clickedClone.originalEquipment.touchLock1.SetActive(false);
                clickedClone.originalEquipment.touchLock2.SetActive(false);
                clickedClone.originalEquipment.check.SetActive(false);
            }

            Destroy(child.gameObject);
        }
        equipMixBoxes[1].gameObject.SetActive(false);
        equipMixBoxes[2].gameObject.SetActive(false);
        equipExplain.SetActive(true);
        equipNameExplain.SetActive(false);
        equipRankExplain.SetActive(false);
    }
    
    public void OnbackBtnClick()
    {
        for (int i = 0; i < 3; i++)
        {
            DestroyAllClonesAndDeactivate(equipMixBoxes[i]);
        }

        mixLockedPanel.SetActive(false); 
        equipMixBtn.SetActive(false);
        if (equipMixResultBox.childCount > 0)
        {
            Destroy(equipMixResultBox.GetChild(0).gameObject);
        }
        equipArrangeManager.SortByRank();
    }
    
    public void EquipMixAfter(Transform equipMixBox)
    {   
        foreach (Transform child in equipMixBox)
        {
            EquipmentStatus clickedClone = child.GetComponent<EquipmentStatus>();
            if (clickedClone != null && clickedClone.originalEquipment != null)
            {
                clickedClone.originalEquipment.touchLock1.SetActive(false);
                clickedClone.originalEquipment.touchLock2.SetActive(false);
                clickedClone.originalEquipment.check.SetActive(false);
            }
        }
        if (equipMixResultBox.childCount > 0)
        {
            Destroy(equipMixResultBox.GetChild(0).gameObject);
        }
        equipMixBoxes[1].gameObject.SetActive(false);
        equipMixBoxes[2].gameObject.SetActive(false);
        equipExplain.SetActive(true);
        equipNameExplain.SetActive(false);
        equipRankExplain.SetActive(false);
    }


    public void EquipExplainUpdate(EquipmentStatus clickedEquipment)
    {
        equipNameExplainText.text = clickedEquipment.equipName;

        // 현재 장비 등급과 다음 장비 등급을 얻습니다.
        EquipmentManager.Rank currentRank = clickedEquipment.equipRank;
        EquipmentManager.Rank nextRank = equipmentManager.GetNextRank(currentRank);

        // 등급을 문자열로 변환합니다.
        string currentRankStr = currentRank.ToString();
        string nextRankStr = nextRank.ToString();
    
        if (currentRankStr.EndsWith("1") || currentRankStr.EndsWith("2") || currentRankStr.EndsWith("3"))
        {
            equipRankExplainText.text = $"랭크업 : {currentRankStr} -> {currentRankStr.Substring(0, currentRankStr.Length - 1)}+1";
        }
        else if (nextRankStr.EndsWith("1") || nextRankStr.EndsWith("2") || nextRankStr.EndsWith("3"))
        {
            equipRankExplainText.text = $"랭크업 : {currentRankStr} -> {nextRankStr.Substring(0, nextRankStr.Length - 1)}+1";
        }
        else
        {
            equipRankExplainText.text = $"랭크업 : {currentRankStr} -> {nextRankStr}";
        }
    }
    
    


}
