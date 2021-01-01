using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 세이브 데이터 클래스
[System.Serializable]
public class SaveDataClass
{
    public string lastPlayTime;             //마지막 플탐을 알아야 화분을 키운다
    public ComponentClass[] potList;    //화분 3개의 리스트. 화분이 무엇으로 채워져있는지.
    public List<CharacterClass> characterList;  //현재 내가 가지고있는 캐릭터의 리스트
    public List<ComponentClass> owningComponentList;    //현재 내가 수확을 끝마친 부위의 리스트
    public int coin;                            //현재 가지고있는 코인

    public SaveDataClass()
    {
        //테스트용 생성자. 그냥 3개씩 다 만들어준ㄴ겁니다.
        lastPlayTime = DateTime.Now.ToString();
        potList = new ComponentClass[3];
        for(int i = 0; i < 3; i++)
        {
            potList[i] = new ComponentClass();
        }
        characterList = new List<CharacterClass>();
        for (int i = 0; i < 3; i++)
        {
            characterList.Add(new CharacterClass());
        }
        owningComponentList = new List<ComponentClass>();
        for (int i = 0; i < 3; i++)
        {
            owningComponentList.Add(new ComponentClass());
        }

    }
}


