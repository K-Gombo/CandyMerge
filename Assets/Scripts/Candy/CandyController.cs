using UnityEngine;
using System.Collections.Generic;
public class CandyController : MonoBehaviour
{
    RaycastHit2D hit;
    Vector3 startPosition;
    private List<Transform> boxTransforms; // 박스 Transform 리스트

    private void Start()
    {
        // 시작할 때 모든 박스를 가져와서 리스트에 저장
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
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (hit.collider != null && hit.collider.CompareTag("Candy"))
            {
                var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit.collider.transform.position = new Vector3(world.x, world.y, 90);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (hit.collider != null && hit.collider.CompareTag("Candy"))
            {
                Transform closestBox = FindClosestEmptyBox(hit.collider.transform);

                if (closestBox != null)
                {
                    hit.collider.transform.SetParent(closestBox);
                    hit.collider.transform.position = closestBox.position; // 박스 위치로 옮김
                }
                else
                {
                    hit.collider.transform.position = startPosition; // 빈 박스가 없으면 원래 위치로 돌림
                }
            }
        }
    }

    private Transform FindClosestEmptyBox(Transform candyTransform)
    {
        Transform closestBox = null;
        float closestDistance = Mathf.Infinity;

        foreach (var boxTransform in boxTransforms) // 미리 저장한 박스 Transform 리스트 사용
        {
            if (boxTransform.childCount == 0) // 빈 박스만 확인
            {
                float distance = Vector3.Distance(candyTransform.position, boxTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBox = boxTransform;
                }
            }
        }

        return closestBox;
    }
}
