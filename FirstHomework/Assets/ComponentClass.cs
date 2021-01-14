using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ComponentClass
{
    public static int stage;
    public string name;         //이름
    public Vector2 position;             //부위의 위치     
    public GameObject realGameobject;      //이 컴포넌트가 가진 실제의 게임오브젝트.
    public float sproutTime;         //피워나는데 걸리는 시간
    public SpriteRenderer spriteRenderer;   //스프라이트 렌더러 component. 나중에 realGameObject.GetComponent<SpriteRenderer>()해야함.
    public float timer;

    public static void PrintStage()
    {
        Debug.Log(stage);
    }

    public void PrintName()
    {
        Debug.Log(name);
    }

    public ComponentClass()         //요건 생성자라는겁니다. 객체를 생성할때 기본값을 지정해주는, 자동실행되는 함수입니다.
    {
        name = "정상훈";
    }

    public ComponentClass(string myname)
    {
        name = myname;
    }



}
