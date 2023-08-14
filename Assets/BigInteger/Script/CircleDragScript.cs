using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleDragScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public GameObject ParentPanel;
    public GameObject FakePanel;
    public Vector2 TestVec2;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    // 드래그 시작
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //일단 페이크패널에 보냈다가 다시 ParentPanel 하위에 넣으면 UI가 가장 위로 올라간다
        //gameObject.transform.parent = FakePanel.transform; 
        //gameObject.transform.parent = ParentPanel.transform;
    }
    // 드래그 중일때
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = Input.mousePosition;
        this.transform.position = currentPos;
    }
    // 드래그 끝났을때
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //gameObject.transform.localPosition = TestVec2;
    }

    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Type_0")
        {
            Debug.Log("Type_0");
        }
    }*/

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Type_0"))
        {
         Debug.Log("Type_0");
        }
    }
}
