using UnityEngine;

public class CandyController : MonoBehaviour
{
    RaycastHit2D hit;
    Vector3 startPosition;

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

        foreach (var box in GameObject.FindGameObjectsWithTag("Box"))
        {
            if (box.transform.childCount == 0) // 빈 박스만 확인
            {
                float distance = Vector3.Distance(candyTransform.position, box.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBox = box.transform;
                }
            }
        }

        return closestBox;
    }
}
