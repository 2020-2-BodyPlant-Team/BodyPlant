using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentClass : MonoBehaviour
{
    public string name;         //이름
    public Vector2 position;             //부위의 위치     
    public Sprite unSproutedSprite;            //꽃이 피워나지 않았을 때 스프라이트 이미지
    public Sprite sproutedSprite;      //꽃이 피워났을 떄의 스프라이트 이미지
    public GameObject realGameObject;      //이 컴포넌트가 가진 실제의 게임오브젝트.
    public float sproutTime;         //피워나는데 걸리는 시간
    public SpriteRenderer spriteRenderer;   //스프라이트 렌더러 component. 나중에 realGameObject.GetComponent<SpriteRenderer>()해야함.

    public ComponentClass()         //요건 생성자라는겁니다. 객체를 생성할때 기본값을 지정해주는, 자동실행되는 함수입니다.
    {
        position = Vector2.zero;

        sproutTime = 10;
    }


}
