using UnityEngine;
using System.Collections.Generic;

public class CandyController : MonoBehaviour
{
    RaycastHit2D hit;
    Vector3 startPosition;
    private Transform originalParent; // 원래 있던 박스를 저장하기 위한 변수
    private List<Transform> boxTransforms;
    private int originalSortingOrder;

    private void Start()
    {
        boxTransforms = new List<Transform>();
        foreach (var box in GameObject.FindGameObjectsWithTag("Box"))
        {
            boxTransforms.Add(box.transform);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(worldPoint, transform.forward, Mathf.Infinity);
            if (hit.collider != null && hit.collider.CompareTag("Candy"))
            {
                startPosition = hit.collider.transform.position;
                originalParent = hit.collider.transform.parent; // 원래 부모 박스 저장
                hit.collider.transform.SetParent(null);
                originalSortingOrder = hit.collider.GetComponent<SpriteRenderer>().sortingOrder;
                hit.collider.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }

        if (Input.GetMouseButton(0) && hit.collider != null && hit.collider.CompareTag("Candy"))
        {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (hit.collider.transform == null)
            {
                Debug.LogError("Collider's transform is null");
            }

            hit.collider.transform.position = new Vector3(world.x, world.y, 90);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (hit.collider != null && hit.collider.CompareTag("Candy"))
            {
                hit.collider.GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrder;
                Transform mergeTarget = GetMergeTarget(hit.collider.transform);
                Transform closestBox = FindClosestEmptyBox(hit.collider.transform);
                float thresholdDistance = 0.5f;

                if (mergeTarget != null && hit.collider.GetComponent<CandyStatus>().level ==
                    mergeTarget.GetComponent<CandyStatus>().level)
                {
                    Debug.Log("여기다!");
                    MergeCandies(hit.collider.transform, mergeTarget);
                    hit.collider.transform.position = startPosition;
                }
                else
                {
                    float distanceToBox = closestBox != null
                        ? Vector3.Distance(hit.collider.transform.position, closestBox.position)
                        : Mathf.Infinity;

                    if (mergeTarget == null && distanceToBox < thresholdDistance)
                    {
                        hit.collider.transform.SetParent(closestBox);
                        hit.collider.transform.position = closestBox.position;
                    }
                    else
                    {
                        ReturnToOriginalBox(hit.collider.transform); // 원래 박스로 되돌리는 메서드 호출
                    }
                }
            }
        }

    }

    private Transform FindClosestEmptyBox(Transform candy)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = candy.position;

        foreach (Transform t in boxTransforms)
        {
            if (t.childCount == 0)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }

        return tMin;
    }

    private Transform GetMergeTarget(Transform candy)
    {
        foreach (Transform t in boxTransforms)
        {
            if (t.childCount > 0 && t.GetChild(0).name == candy.name)
            {
                float dist = Vector3.Distance(t.position, candy.position);
                if (dist < 0.2f)
                {
                    return t.GetChild(0);
                }
            }
        }

        return null;
    }

    private void MergeCandies(Transform candy1, Transform candy2)
    {
        CandyStatus candyStatus1 = candy1.GetComponent<CandyStatus>();
        CandyStatus candyStatus2 = candy2.GetComponent<CandyStatus>();

        if (candyStatus1.level == candyStatus2.level)
        {
            candyStatus2.level++;
            CandyManager.instance.SetAppearance(candy2.gameObject);
            candy2.position = candy2.parent.position;

            boxTransforms.Remove(candy1);
            Destroy(candy1.gameObject);

            CandyManager.instance.CandyDestroyed();
        }
        else
        {
            candy1.position = startPosition;
        }
    }

    private void ReturnToOriginalBox(Transform candy)
    {
        candy.transform.SetParent(originalParent); // 원래 부모 박스로 설정
        Debug.LogWarning(originalParent);
        candy.transform.position = startPosition;
    }

   
    
}