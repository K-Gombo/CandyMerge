using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public GameObject equipMixPanel;
    public Transform[] equipMixBoxes;
    public static EquipmentController instance;
    public GameObject mixLockedPanel;
    public GameObject equipMixBtn;
    public GameObject equipAllMixBtn;
    public EquipArrangeManager equipArrangeManager;
    public LanguageUIManager languageUIManager;
    public CurrencyUI currencyUI;
    public GameObject backBtn;
    public GameObject plus;
    public Transform equipMixResultBox;
    public EquipmentManager equipmentManager;
    public GameObject equipNameExplain;
    public Text equipNameExplainText;
    public GameObject equipRankExplain;
    public Text equipRankExplainText;
    public GameObject equipGoldIncrementExpain;
    public Text equipGoldIncrementExplainText;
    public GameObject equipArrangeBtnGroup;
    public GameObject equipExplain;
    public GameObject equipPanel;
    public GameObject equipStatusPanel;
    public Text equipNameStatusText;
    public Image equipRankStatus;
    public Text equipRankStatusText;
    public Text equipGoldIncrementText;
    public Text equipLevelStatusText;
    public Text equipExplainText;
    public Text[] equipSkillText;
    public Image[] equipSkillImages;
    public Image[] equipSkillLockImages;
    public GameObject[] equipSkillLock;
    public GameObject StatusEquipImage;
    public Text equipStatusGoldText;
    [HideInInspector]public EquipmentStatus clickedEquipForUpgrade;
    [HideInInspector]public GameObject currentClone;
    public Button equipmentEquipBtn;
    public Text equipmentEquipBtnText;
    

    void Awake()
    {
        instance = this;

        // backBtn에 이벤트 연결
        Button backBtnComponent = backBtn.GetComponent<Button>();
        backBtnComponent.onClick.AddListener(OnbackBtnClick);
        
        equipmentEquipBtn.onClick.AddListener(OnEquipmentEquipBtnClick);
        
    }

    public void OnEquipmentClick(EquipmentStatus clickedEquipment)
    {
        if (equipMixPanel.activeSelf)
        {
            EquipMixPanelEquipClick(clickedEquipment);
            return;
        }

        if (equipPanel.activeSelf)
        {
            EquipPanelEquipClick(clickedEquipment);
            clickedEquipForUpgrade = clickedEquipment;

            // 클릭한 장비의 장착 상태에 따라 버튼 텍스트를 변경
            languageUIManager.UpdateEquipBtnText(clickedEquipForUpgrade.isEquipped);  // 이 부분 추가
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
        if (equipMixPanel.activeSelf)
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

                // equipMixResultBox에 있는 previewClone 삭제
                if (equipMixResultBox.childCount > 0)
                {
                    Destroy(equipMixResultBox.GetChild(0).gameObject);
                }

                equipMixBoxes[1].gameObject.SetActive(false);
                equipMixBoxes[2].gameObject.SetActive(false);
                plus.SetActive(false);
                mixLockedPanel.SetActive(false); 
                equipMixBtn.SetActive(false);
                equipAllMixBtn.SetActive(true);
                equipArrangeManager.SortByRank();
                equipArrangeBtnGroup.SetActive(true);
                equipExplain.SetActive(true);
                equipNameExplain.SetActive(false);
                equipRankExplain.SetActive(false);
                equipGoldIncrementExpain.SetActive(false);
                equipmentManager.CheckMixAvailability();
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
        plus.SetActive(false);
        equipAllMixBtn.SetActive(true);
        equipExplain.SetActive(true);
        equipNameExplain.SetActive(false);
        equipRankExplain.SetActive(false);
        equipGoldIncrementExpain.SetActive(false);
    }
    
    public void OnbackBtnClick()
    {
        for (int i = 0; i < 3; i++)
        {
            DestroyAllClonesAndDeactivate(equipMixBoxes[i]);
        }

        mixLockedPanel.SetActive(false); 
        equipMixBtn.SetActive(false);
        equipAllMixBtn.SetActive(false);
        if (equipMixResultBox.childCount > 0)
        {
            Destroy(equipMixResultBox.GetChild(0).gameObject);
        }
        equipArrangeManager.SortByRank();
        equipmentManager.CheckMixAvailability();
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
        plus.SetActive(false);
        equipExplain.SetActive(true);
        equipNameExplain.SetActive(false);
        equipRankExplain.SetActive(false);
        equipAllMixBtn.SetActive(true);
        equipGoldIncrementExpain.SetActive(false);
        equipmentManager.CheckMixAvailability();
        equipArrangeBtnGroup.SetActive(true);
    }


    public void EquipExplainUpdate(EquipmentStatus clickedEquipment)
    {
        equipNameExplainText.text = clickedEquipment.equipName;

        EquipmentManager.Rank currentRank = clickedEquipment.equipRank;
        EquipmentManager.Rank nextRank = equipmentManager.GetNextRank(currentRank);

        string currentRankStr = currentRank.ToString();
        string nextRankStr = nextRank.ToString();

        string modifiedCurrentRankStr = Regex.Replace(currentRankStr, @"(\d+)$", m => "+" + m.Groups[1].Value);
        string modifiedNextRankStr = Regex.Replace(nextRankStr, @"(\d+)$", m => "+" + m.Groups[1].Value);

        equipRankExplainText.text = $"Rank : {modifiedCurrentRankStr} <b><color=#00CC00>->  {modifiedNextRankStr}</color></b>";

        float nextGoldIncrement = equipmentManager.GetNextGoldIncrement(clickedEquipment.equipRank);
        LanguageUIManager.Language currentLanguage = languageUIManager.GetCurrentLanguage();
        equipGoldIncrementExplainText.text = languageUIManager.GetGoldIncrementText(clickedEquipment.goldIncrement, nextGoldIncrement, currentLanguage);
    }

    
    public void EquipMixPanelEquipClick(EquipmentStatus clickedEquipment)
    {
        equipArrangeManager.UpdateEquipList();
        if (!clickedEquipment.isOriginal)
        {
            return;
        }

        if (clickedEquipment.myClone != null)
        {
            return;
        }

        if (equipMixPanel.activeSelf)
        {   clickedEquipment.mixAvailable.SetActive(false);
            GameObject clone = null;
            Transform targetParent = null;
            equipArrangeBtnGroup.SetActive(false);
            plus.SetActive(true);

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
                
                equipArrangeManager.FilterByRankAndName(clickedEquipment.equipRank, clickedEquipment.equipName , clickedEquipment.rankLevel);
                
                foreach (EquipmentStatus equipment in equipArrangeManager.equipList)
                {
                    if (equipment.equipRank == clickedEquipment.equipRank && 
                        equipment.equipName == clickedEquipment.equipName && 
                        equipment.rankLevel == clickedEquipment.rankLevel)
                    {
                        equipment.mixAvailable.SetActive(false);
                    }
                }
                
                if (equipMixResultBox.childCount == 0)  
                {   
                    GameObject previewClone = Instantiate(clickedEquipment.gameObject);
                    EquipmentStatus previewCloneStatus = previewClone.GetComponent<EquipmentStatus>();
                    previewCloneStatus.isOriginal = false;
                    
                    previewClone.transform.localScale = new Vector3(
                        clickedEquipment.transform.localScale.x * 1.3f,
                        clickedEquipment.transform.localScale.y * 1.3f,
                        clickedEquipment.transform.localScale.z * 1.3f
                    );
                    
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
                    
                    equipmentManager.SetRankLevelSlotActive(newRankLevel, previewCloneStatus.rankLevelSlot);

                    previewClone.transform.SetParent(equipMixResultBox, false);
                    previewClone.transform.localPosition = Vector3.zero;
                }
                else
                {   
                    GameObject existingPreviewClone = equipMixResultBox.GetChild(0).gameObject;
                    if (!existingPreviewClone.activeSelf)
                    {
                        existingPreviewClone.SetActive(true);
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
            equipAllMixBtn.SetActive(false);
            equipGoldIncrementExpain.SetActive(true);
            EquipExplainUpdate(clickedEquipment);
        }
    }

    public void EquipPanelEquipClick(EquipmentStatus clickedEquipment)
    {
        equipStatusPanel.SetActive(true);
        EquipStatusUpdate(clickedEquipment);

        // 클론 생성
        clickedEquipment.mixAvailable.SetActive(false);
        GameObject clone = Instantiate(clickedEquipment.gameObject);
        EquipmentStatus cloneStatus = clone.GetComponent<EquipmentStatus>();
        cloneStatus.isOriginal = false;
        cloneStatus.originalEquipment = clickedEquipment;
        Destroy(clone.GetComponent<Button>());
        RectTransform cloneRect = clone.GetComponent<RectTransform>();
        cloneRect.anchorMin = new Vector2(0.5f, 0.5f);
        cloneRect.anchorMax = new Vector2(0.5f, 0.5f);
        cloneRect.anchoredPosition3D = Vector3.zero;
        cloneRect.sizeDelta = new Vector2(100, 100);
        Vector3 originalScale = clickedEquipment.transform.localScale;
        if (clickedEquipment.isEquipped)
        {
            cloneRect.localScale = clickedEquipment.originalScale * 1.2f;
        }
        else
        {
            cloneRect.localScale = originalScale * 1.2f;
        }
        
        
        clone.transform.SetParent(StatusEquipImage.transform, false);
        clone.transform.localPosition = Vector3.zero;
        
        currentClone = clone;
    }
    
    public void EquipStatusUpdate(EquipmentStatus clickedEquipment)
    {
        equipNameStatusText.text = clickedEquipment.equipName;

        EquipmentManager.Rank statusRank = clickedEquipment.equipRank;
        string statusRankStr = statusRank.ToString();
        string changeStatusRankStr = Regex.Replace(statusRankStr, @"(\d+)$", m => "+" + m.Groups[1].Value);
        equipRankStatusText.text = $"{changeStatusRankStr}";

        string cleanedStatusRankStr = Regex.Replace(statusRankStr, @"\d", "");
        if (Enum.TryParse(cleanedStatusRankStr, out EquipmentManager.Rank cleanedStatusRank))
        {
            if (equipmentManager.rankToColorMap.ContainsKey(cleanedStatusRank))
            {
                equipRankStatus.color = equipmentManager.rankToColorMap[cleanedStatusRank];
            }
        }
        equipGoldIncrementText.text = $"{clickedEquipment.goldIncrement}%";
        equipLevelStatusText.text = $"Level {clickedEquipment.equipLevel}/{clickedEquipment.maxEquipLevel}";
        equipExplainText.text = $"{clickedEquipment.equipExplain}";

        for (int i = 0; i < clickedEquipment.skillNames.Length; i++)
        {
            equipSkillText[i].text = $"{clickedEquipment.skillNames[i]}  <b>{clickedEquipment.skillPoints[i]}%</b>";
        }

        List<EquipmentManager.Rank> rankOrderList = new List<EquipmentManager.Rank>(EquipmentManager.maxLevelsPerRank.Keys);
        int equipmentRankOrder = rankOrderList.IndexOf(clickedEquipment.equipRank);

        for (int i = 0; i < equipSkillLock.Length; i++)
        {
            if (i < clickedEquipment.skillRanks.Length)
            {
                EquipmentManager.Rank skillRank = clickedEquipment.skillRanks[i];
                int skillRankOrder = rankOrderList.IndexOf(skillRank);

                if (skillRankOrder > equipmentRankOrder)
                {
                    equipSkillLock[i].SetActive(true);
                    clickedEquipment.skillUnlocked[i] = false;  // 스킬 잠김
                }
                else
                {
                    equipSkillLock[i].SetActive(false);
                    clickedEquipment.skillUnlocked[i] = true;  // 스킬 해금
                }

                if (equipmentManager.rankToColorMap.ContainsKey(skillRank))
                {
                    equipSkillImages[i].color = equipmentManager.rankToColorMap[skillRank];
                    equipSkillLockImages[i].color = equipmentManager.rankToColorMap[skillRank];
                }
            }
            else
            {
                equipSkillLock[i].SetActive(false);
                clickedEquipment.skillUnlocked[i] = false;  // 스킬 잠김
            }
        }

        string formattedGold = currencyUI.goldText.text;
        string formattedUpgradeGoldCost = BigIntegerCtrl_global.bigInteger.ChangeMoney(clickedEquipment.upgradeGoldCost.ToString());
        equipStatusGoldText.text = $"{formattedGold}/{formattedUpgradeGoldCost}";
    }

    
    public void OnEquipmentEquipBtnClick()
    {
        if (clickedEquipForUpgrade == null) return;

        if (clickedEquipForUpgrade.isEquipped)
        {
            // 이미 장착되어 있다면 해제
            equipmentManager.EquipmentSlotUnequip(clickedEquipForUpgrade, false);
        }
        else
        {
            // 장착되어 있지 않다면 장착
            equipmentManager.EquipmentSlotEquip(clickedEquipForUpgrade, false);
        }
        
        // 생성된 클론을 제거
        if (currentClone != null)
        {
            Destroy(currentClone);
           currentClone = null; // 클론 제거 후 null로 설정
        }
        equipStatusPanel.SetActive(false);
    }
    

}
