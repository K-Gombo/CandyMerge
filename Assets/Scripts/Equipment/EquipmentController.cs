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
    public EquipmentManager equipmentManager;
    public GameObject equipArrangeBtnGroup;

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
                Debug.Log("previewClone 생성됨: " + previewClone.activeSelf);
                
                // 여기에 scale을 1.3배로 설정
                previewClone.transform.localScale = new Vector3(
                    clickedEquipment.transform.localScale.x * 1.3f,
                    clickedEquipment.transform.localScale.y * 1.3f,
                    clickedEquipment.transform.localScale.z * 1.3f
                );

                // rankLevel을 1만큼 증가
                previewCloneStatus.rankLevel = clickedEquipment.rankLevel + 1;

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

                Debug.Log("UI 업데이트 후 previewClone 상태: " + previewClone.activeSelf);

                previewClone.transform.SetParent(equipMixResultBox, false);
                previewClone.transform.localPosition = Vector3.zero;
                Debug.Log("부모 설정 후 previewClone 상태: " + previewClone.activeSelf);
            }
            else
            {   
                GameObject existingPreviewClone = equipMixResultBox.GetChild(0).gameObject;
                if (!existingPreviewClone.activeSelf)
                {
                    existingPreviewClone.SetActive(true);  // 비활성화된 상태라면 활성화
                }
                Debug.Log("previewClone 이미 존재: " + equipMixResultBox.GetChild(0).gameObject.activeSelf);
            }
            
            // 원본에 있는 특정 오브젝트 활성화
            clickedEquipment.touchLock1.SetActive(true);
            clickedEquipment.touchLock2.SetActive(true);
            clickedEquipment.check.SetActive(true);
        }
        
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
    }
    
}
