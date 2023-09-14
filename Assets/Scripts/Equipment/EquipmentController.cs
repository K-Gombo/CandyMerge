using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public GameObject equipMixPanel;
    public Transform[] equipMixBoxes;
    public static EquipmentController instance;

    void Awake()
    {
        instance = this;
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

            if (equipMixBoxes[0].childCount == 0 && !equipMixBoxes[1].gameObject.activeSelf && !equipMixBoxes[2].gameObject.activeSelf)
            {
                targetParent = equipMixBoxes[0];
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
                // 복사본을 생성
                clone = Instantiate(clickedEquipment.gameObject);
                EquipmentStatus cloneStatus = clone.GetComponent<EquipmentStatus>();
                cloneStatus.isOriginal = false;

                // 원본 클릭 이벤트 제거
                Button originalButton = clickedEquipment.GetComponent<Button>();
                originalButton.onClick.RemoveAllListeners();

                // 원본 클릭 이벤트 재설정
                originalButton.onClick.AddListener(() => OnEquipmentClick(clickedEquipment));

                // 새로운 클릭 이벤트 추가
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
            // EquipMixBox2, 3에 있는 모든 클론을 제거
            foreach (Transform child in equipMixBoxes[1])
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in equipMixBoxes[2])
            {
                Destroy(child.gameObject);
            }

            // EquipMixBox2, 3를 비활성화
            equipMixBoxes[1].gameObject.SetActive(false);
            equipMixBoxes[2].gameObject.SetActive(false);
        }

        // 클릭된 클론 제거
        Destroy(clickedClone.gameObject);
    }

}
