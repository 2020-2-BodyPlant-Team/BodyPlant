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
        componentList = new List<ComponentClass>();
        componentList.Add(new ComponentClass("정상훈"));
        componentList.Add(new ComponentClass("김예현"));
        componentList.Add(new ComponentClass("안우진"));
        componentList.Add(new ComponentClass("문유진"));
        componentList.Add(new ComponentClass("이하영"));
        for(int i = 0; i < 5; i++)
        {
            Debug.Log(componentList[i].name);
        }

    }

    private void Update()
    {

    }
}
