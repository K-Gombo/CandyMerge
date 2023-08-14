using UnityEngine;

public class Candy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isDragging = false; // 드래그 중인지 확인하는 변수

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CandyStatus status = GetComponent<CandyStatus>();
        status.level = Random.Range(1, 4); // 1부터 3까지의 레벨만 생성되게 변경
        CandyManager.instance.SetAppearance(this.gameObject);
    }

    private void OnMouseDown()
    {
        CandyStatus status = GetComponent<CandyStatus>();
        Debug.Log($"Candy Level: {status.level}");
        isDragging = true; // 마우스 버튼을 누르면 드래그 시작
    }

    private void OnMouseUp()
    {
        isDragging = false; // 마우스 버튼을 뗄 때 드래그 중단
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }
}