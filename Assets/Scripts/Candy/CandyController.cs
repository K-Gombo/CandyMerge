using UnityEngine;

public class CandyController : MonoBehaviour
{
    private bool isDragging;

    RaycastHit2D hit;

    Vector3 positionVec;

    [SerializeField] GameObject temp;

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
            Vector2 wolrdPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(wolrdPoint, transform.forward, Mathf.Infinity);

            Debug.DrawRay(wolrdPoint, transform.forward, Color.red, Mathf.Infinity);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                positionVec = hit.collider.transform.position;
            }
        }

        if (Input.GetMouseButton(0))
        {
            temp.transform.localPosition = Input.mousePosition;
            if (hit.collider != null)
            {
                Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition) + "\t" + hit.collider.transform.position);
                //Debug.Log(Camera.main.WorldToScreenPoint(Input.mousePosition));
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
        //if (isDragging)
        //{
        //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        //    transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        //}



    }
}