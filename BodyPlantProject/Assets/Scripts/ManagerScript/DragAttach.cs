using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAttach : MonoBehaviour
{
    [SerializeField] bool isDrag = true;
    public ComposeManager composeManager;

    float distance = 10f;
    Vector3 offset;     //마우스를 클릭한 지점에서 스프라이트 지점사이 거리.
    Vector3 mouseStartPoint;    //회전할 때 클릭한 지점 기점으로 돌려야돼서 그렇다.

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = new Vector3(offset.x, offset.y, 0);
        mouseStartPoint = Input.mousePosition;
        //z축 0으로 맞추기.
        //이걸 안하면 마우스가 클릭했을 때 오브젝트가 마우스에 자석처럼 달라붙는다.

        if (composeManager.flipMode)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y-180, transform.eulerAngles.z);
        }
    }

    void OnMouseDrag()    //마우스 드래그로 옮겨서 합체할 수 있도록 했습니다
    {
        if (isDrag)
        {
            if (composeManager.rotationMode)
            {
                //회전모드일 때는 회전을 한다
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                Vector2 deltaVector2 = new Vector2(mouseStartPoint.x - Input.mousePosition.x, mouseStartPoint.y - Input.mousePosition.y);
                Debug.Log(deltaVector2.x + deltaVector2.y);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, deltaVector2.x + deltaVector2.y);
            }
            else if(!composeManager.flipMode)
            {
                //그냥 드래그모드
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
                transform.position = objPosition;
            }

        }
    }

    private void OnMouseExit()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
         * 잠시 지우께요ㅠㅠ
        if (other.transform.tag == "All")
        {
            Debug.Log("Hit");
            transform.parent = other.transform;    //따로 떨어진 신체부위들을 한 오브젝트의 자식으로 들어가게 했습니다
            isDrag = false;                        //합체가 되면 더 이상 드래그가 안 되도록 했습니다
        }*/
    }
}
