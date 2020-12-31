using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터를 이루고 있는 요소 1개를 위한 클래스
[System.Serializable]
public class ComponentClass
{
    public string name;                 //이름
    public Vector2 position;           //캐릭터 내에서의 상대적 위치
    public List<Vector2> jointPosition; //관절의 위치
    public Sprite sprite;               //스프라이트 이미지
    public string plantedTime;         //심어진 시간
    public bool isSprotued;             //화분에서 전부 자랐는지
    public float rotation;            //얼마나 돌아갔는지.

    //테스트용으로 만든거. 디버그 위해서 객체를 만들어야하니까 생성자가 그냥 있습니다.
    public ComponentClass()
    {
        name = "test";
        position = Vector2.zero;
        jointPosition = new List<Vector2>();
        jointPosition.Add(Vector2.zero);
        jointPosition.Add(Vector2.zero);
        sprite = null;
        plantedTime = DateTime.Now.ToString();
        isSprotued = true;

    }
}
