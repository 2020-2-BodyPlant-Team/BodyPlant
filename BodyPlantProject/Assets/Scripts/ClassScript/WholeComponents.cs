using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전체 부위들을 포함하고 있는 클래스. 나중에 json파일의 형태로 여기로 데이터를 불러오게 될 것임.
[System.Serializable]
public class WholeComponents
{
    public List<ComponentDataClass> componentList;
    
    //이거는 기획자가 만들어준 json파일을 받아오는 역할입니다.

    //이거도 테스트용 생성자
    public WholeComponents()
    {
        componentList = new List<ComponentDataClass>();
        for(int i = 0; i < 10; i++)
        {
            componentList.Add(new ComponentDataClass());
        }
    }
}
