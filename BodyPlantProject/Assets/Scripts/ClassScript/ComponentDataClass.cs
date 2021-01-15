using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이걸 따로 만든 이유는, 자꾸 값만 복사하고 싶은데 realGameObject가 말썽을 부렸다
//그래서 hard하게 값이 정해져있는 요소들만 data로 빼왔다.
[System.Serializable]
public class ComponentDataClass
{
    public string name;                 //이름                              
    public bool isJoint;            //관절이 돌아가는 건지 (팔, 다리인지)
    public List<Vector2> attachPosition;    //붙이는곳의 위치.
    public int price;                   //얼마에 사는지
    public float sproutSeconds;         //피워나는데 몇초 걸리는지
    public Sprite[] componentSpriteArray;      //스프라이트
    public Vector2 sproutingPosition;   //다 자라났을 떄의 위치.

    public ComponentDataClass()
    {
        name = "null";
        attachPosition = new List<Vector2>();
        attachPosition.Add(Vector2.zero);
        price = 10;
        sproutSeconds = 10;
        sproutingPosition = new Vector2(0, 2);
    }
}
