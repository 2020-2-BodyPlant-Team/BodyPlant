using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터를 이루고 있는 요소 1개를 위한 클래스
/// <summary>
/// 여기서는 로딩할 때 쓰는 요소도 있고, 게임 내에서 쓰는 요소도 있다.
/// 게임 내에서 사용하는 요소는 언제든지 바뀔 수 있고, 저장하는 요소도 있다.
/// 예를들어 이름은 로딩하는 요소이다. 이름은 절대 바뀌지 않으니까.
/// jointPosition또한 관절의 위치이므로 기획자가 정해주는 값, 즉 로딩하는 요소, 바뀌지 않는다
/// 다만 isSprouted,realGameobject는 게임 내에서 언제든지 바뀔 수 있는 요소들이다.
/// 이 중에서도 realGameObject는 저장이 되지 않고, 게임 내에서만 사용하는 데이터이다.
/// 또한 캐릭터 내에서의 상대적 위치인 position또한 언제든지 바뀔 수 있는 값이고, 우리가 저장하고 로딩해서 써야할 값이다.
/// </summary>
[System.Serializable]
public class ComponentClass
{
    public ComponentDataClass componentData;

    public Vector2 position;           //캐릭터 내에서의 상대적 위치     

    public Sprite sprite;               //스프라이트 이미지
    public string plantedTime;         //심어진 시간
    public bool isSprotued;             //화분에서 전부 자랐는지
    public float rotation;            //얼마나 돌아갔는지.


    public GameObject realGameobject;   //이 컴포넌트가 가진 실제의 게임오브젝트.

    public bool isHarvested;            //수확이 되었는지.

    //테스트용으로 만든거. 디버그 위해서 객체를 만들어야하니까 생성자가 그냥 있습니다.
    public ComponentClass()
    {
        componentData = new ComponentDataClass();
        position = Vector2.zero;
        sprite = null;
        plantedTime = DateTime.Now.ToString();
        isSprotued = false;
        isHarvested = false;
        rotation = 90;
    }
}
