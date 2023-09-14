using System.Collections;
using UnityEngine;

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
                ActivateAndMoveBoxes(); // 비활성화 상태일 때만 이 함수를 호출
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

            // 복사본의 RectTransform 설정
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

            // 부모 설정
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
    
    public void ReturnEquipment(EquipmentStatus returnedEquipment)
    {
        if (returnedEquipment.myClone != null)
        {
            // 원본에 대한 참조를 해제
            returnedEquipment.myClone = null;
        }
        returnedEquipment.gameObject.SetActive(false);
    }

}
