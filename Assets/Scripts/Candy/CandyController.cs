using UnityEngine;

public class CandyController : MonoBehaviour
{
    private bool isDragging;

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
        
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        Debug.Log(hit.collider.gameObject.name);
        if (hit.collider != null)
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            Debug.Log($"Hit layer: {layerName}");

            if (hit.collider.gameObject == this.gameObject && layerName == "Candy")
            {
                CandyStatus status = GetComponent<CandyStatus>();
                Debug.Log($"Lv.{status.level} Candy");
                isDragging = true; // 'Candy' 레이어에 속한 오브젝트만 드래그 가능
            }
        }
    }


    private void OnMouseUp()
    {
        isDragging = false; // 마우스 버튼을 뗄 때 드래그 중단
    }
}