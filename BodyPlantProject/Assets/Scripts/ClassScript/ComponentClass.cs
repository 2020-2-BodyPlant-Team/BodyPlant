﻿using System;
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
    //public ComponentDataClass componentData;
    public string name;
    public Vector2 position;           //캐릭터 내에서의 상대적 위치     
    public Vector2 secondPosition;      //팔, 헤어, 다리가 반대로 붙어있을 시 작동

    public string plantedTime;         //심어진 시간
    public bool isSprotued;             //화분에서 전부 자랐는지
    public float percentage;            //성장도
    public Vector3 rotation;            //얼마나 돌아갔는지.
    public Vector3 secondRotation;      //팔, 헤어, 다리가 반대로 붙어있을 시 작동

    public int parentComponentIndex;           //얘의 부모가 뭔지
    public int parentJointIndex;               //얘의 조인트가 뭔지
    public bool secondSwitch;

    public List<int> childIndexList;              //차일드에 붙어있는 인덱스.
    public List<int> childJointList;

    public List<int> childChildIndexList;         //차일드의 차일드에 붙어있는 인덱스
    public List<int> childChildJointList;

    public bool cover;  //뚜껑.
    public bool attached;   //붙어있는지

    public int[] usedElement;     //내가 사용한 보조성분.

    [System.NonSerialized]
    public GameObject realGameobject;   //이 컴포넌트가 가진 실제의 게임오브젝트.
    [System.NonSerialized]
    public GameObject childObject;      //차일드..

    public bool isHarvested;            //수확이 되었는지.

    //테스트용으로 만든거. 디버그 위해서 객체를 만들어야하니까 생성자가 그냥 있습니다.
    public ComponentClass()
    {
        name = "null";
        position = Vector2.zero;
        secondPosition = Vector2.zero;
        plantedTime = DateTime.Now.ToString();
        parentJointIndex = 0;
        parentComponentIndex = -1;
        secondSwitch = false;
        isSprotued = false;
        isHarvested = false;
        rotation = Vector3.zero;
        secondRotation = Vector3.zero;
        childIndexList = new List<int>();
        childJointList = new List<int>();
        childChildIndexList = new List<int>();
        childChildJointList = new List<int>();
        cover = false;
        attached = false;
        usedElement = new int[3];
        for(int i = 0; i < usedElement.Length; i++)
        {
            usedElement[i] = 0;
        }
    }

    public ComponentClass(ComponentClass component)
    {
        name = component.name;            ;
        position = component.position;
        secondPosition = component.secondPosition;
        plantedTime = component.plantedTime;
        parentJointIndex = component.parentJointIndex;
        parentComponentIndex = component.parentComponentIndex;
        secondSwitch = component.secondSwitch;
        isSprotued = component.isSprotued;
        isHarvested = component.isHarvested;
        rotation = component.rotation;
        secondRotation = component.secondRotation;
        childIndexList = new List<int>();
        for(int i = 0; i < component.childIndexList.Count; i++)
        {
            childIndexList.Add(component.childIndexList[i]);
        }
        childJointList = new List<int>();
        for (int i = 0; i < component.childJointList.Count; i++)
        {
            childJointList.Add(component.childJointList[i]);
        }
        childChildIndexList = new List<int>();
        for (int i = 0; i < component.childChildIndexList.Count; i++)
        {
            childChildIndexList.Add(component.childChildIndexList[i]);
        }
        childChildJointList = new List<int>();
        for (int i = 0; i < component.childChildJointList.Count; i++)
        {
            childChildJointList.Add(component.childChildJointList[i]);
        }
        cover = component.cover;
        attached = component.attached;
        usedElement = new int[3];
        for (int i = 0; i < usedElement.Length; i++)
        {
            usedElement[i] = component.usedElement[i];
        }
        realGameobject = component.realGameobject;
        childObject = component.childObject;
        isHarvested = component.isHarvested;
    }
}
