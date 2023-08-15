using UnityEngine;
using System.Collections.Generic;

public class CandyController : MonoBehaviour
{
    RaycastHit2D hit;
    Vector3 startPosition;
    private List<Transform> boxTransforms;

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
                hit.collider.transform.SetParent(null);
            }
        }

        if (Input.GetMouseButton(0) && hit.collider != null && hit.collider.CompareTag("Candy"))
        {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (hit.collider.transform == null) // 이 조건을 추가하여 문제가 있는 경우를 확인
            {
                Debug.LogError("Collider's transform is null");
            }
            hit.collider.transform.position = new Vector3(world.x, world.y, 90);
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (hit.collider != null && hit.collider.CompareTag("Candy"))
            {
                Transform closestBox = FindClosestEmptyBox(hit.collider.transform);

                if (closestBox != null)
                {
                    Transform mergeTarget = GetMergeTarget(hit.collider.transform);

                    if (mergeTarget != null)
                    {
                        MergeCandies(hit.collider.transform, mergeTarget);
                        hit.collider.transform.position = startPosition;
                    }
                    else
                    {
                        float distanceToStart = Vector3.Distance(hit.collider.transform.position, startPosition);
                        float distanceToClosestBox = Vector3.Distance(hit.collider.transform.position, closestBox.position);

                        if (distanceToStart < distanceToClosestBox)
                        {
                            hit.collider.transform.position = startPosition;
                        }
                        else
                        {
                            hit.collider.transform.SetParent(closestBox);
                            hit.collider.transform.position = closestBox.position;
                        }
                    }
                }
                else
                {
                    hit.collider.transform.position = startPosition;
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
                if (dist < 0.5f)
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
        
            // 참조를 먼저 제거한 후에 캔디 파괴
            boxTransforms.Remove(candy1);
            Destroy(candy1.gameObject);

            // 캔디가 병합되어 파괴될 때 CandyManager의 CandyDestroyed 메서드 호출
            CandyManager.instance.CandyDestroyed();
        }
    }
}
