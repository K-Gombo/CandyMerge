using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public GameObject equipMixPanel;
    public Transform[] equipMixBoxes;
    public static EquipmentController instance;
    public GameObject mixLockedPanel;

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

        if (equipMixBoxes[0].childCount == 0)
        {
            targetParent = equipMixBoxes[0];
            mixLockedPanel.SetActive(true); // mixLockedPanel 활성화
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

            equipMixBoxes[1].gameObject.SetActive(false);
            equipMixBoxes[2].gameObject.SetActive(false);
            mixLockedPanel.SetActive(false); // mixLockedPanel 비활성화
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
