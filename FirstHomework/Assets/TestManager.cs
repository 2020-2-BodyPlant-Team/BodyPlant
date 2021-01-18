using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    GameObject something; //null
    //이미 저장되어있는 특정 숫자
    List<ComponentClass> componentList;
    //리스트 안에 있는 componentClass객체의 이름을 바꾸고 출력하라.

    //언제 출력할지.
    private void Start()
    {
        ComponentClass myComponent;
        myComponent = new ComponentClass();
        myComponent.memberVoid();
    }

    private void Update()
    {

    }
}
