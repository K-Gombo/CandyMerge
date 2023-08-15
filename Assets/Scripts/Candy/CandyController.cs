using UnityEngine;

public class CandyController : MonoBehaviour
{
    private bool isDragging;

    RaycastHit2D hit;

    Vector3 positionVec;

    private void Start()
    {
        // Collider2D 컴포넌트 가져오기 (캔디에 Collider2D가 있어야 함)
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        
        if (collider != null) // Collider2D가 존재하면
        {
            collider.isTrigger = true; // 트리거 활성화
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(worldPoint, transform.forward, Mathf.Infinity);

            Debug.DrawRay(worldPoint, transform.forward, Color.red, Mathf.Infinity);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                positionVec = hit.collider.transform.position;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (hit.collider != null)
            {
                Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition) + "\t" + hit.collider.transform.position);
                
                var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                hit.collider.transform.position = new Vector3(world.x,world.y,90);


            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (hit.collider != null)
            {
                hit.collider.transform.position = positionVec;
            }
        }
        
    }
}