
using System;
using System.Collections;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public GameObject equipMixPanel;
    public Transform[] equipMixBoxes;
    private int currentMixBoxIndex = 0;
    public float moveSpeed = 2.0f;
    public static EquipmentController instance;

    // Update is called once per frame

    void Awake()
    {
        instance = this;
    }

    // 장비 프리팹 클릭 이벤트
    public void OnEquipmentClick(EquipmentStatus clickedEquipment)
    {
        if (equipMixPanel.activeSelf)
        {
            if (currentMixBoxIndex < equipMixBoxes.Length)
            {
                GameObject clonedEquipment = Instantiate(clickedEquipment.gameObject, equipMixBoxes[currentMixBoxIndex]);
                Transform targetBox = equipMixBoxes[currentMixBoxIndex];
                targetBox.gameObject.SetActive(true);
                StartCoroutine(MoveTowards(targetBox, clonedEquipment.transform.position, targetBox.position));
                currentMixBoxIndex++;
            }
            else
            {
                Debug.Log("EquipMixBox가 모두 찼습니다.");
            }
        }
    }

    private IEnumerator MoveTowards(Transform objTransform, Vector3 start, Vector3 target)
    {
        objTransform.position = start;
        while (Vector3.Distance(objTransform.position, target) > 0.05f)
        {
            objTransform.position = Vector3.MoveTowards(objTransform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        objTransform.position = target;
    }
}
