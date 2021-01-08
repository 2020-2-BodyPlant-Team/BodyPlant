using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAttach : MonoBehaviour
{
    [SerializeField] bool isDrag = true;

    float distance = 10f;

    void OnMouseDrag()    //마우스 드래그로 옮겨서 합체할 수 있도록 했습니다
    {
        if (isDrag)
        {
            Debug.Log("Drag");
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "All")
        {
            Debug.Log("Hit");
            transform.parent = other.transform;    //따로 떨어진 신체부위들을 한 오브젝트의 자식으로 들어가게 했습니다
            isDrag = false;                        //합체가 되면 더 이상 드래그가 안 되도록 했습니다
        }
    }
}
