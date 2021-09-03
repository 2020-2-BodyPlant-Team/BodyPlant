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
    public List<CharacterClass> characterList;  //현재 비밀공간에 있는 캐릭터 리스트

    public List<CharacterClass> huntCharacterList;
    public List<CharacterClass> mineCharacterList;
    public List<CharacterClass> fishCharacterList;
    //0 hunt 1 mine 2 fish

    public List<ComponentClass> owningComponentList;    //현재 내가 수확을 끝마친 부위의 리스트
    public int coin;                            //현재 가지고있는 코인
    public List<string> boughtNameList;         //내가 산거 이름리스트
    public List<string> boughtDateList;         //내가 산거 데이트 리스트

    public int huntElement;
    public int mineElement;
    public int fishElement;                //보조성분. 

    public bool trainSelled;
    public bool chairSelled;

    public int tutorialOrder;
    public bool fishTutorial;
    public bool mineTutorial;

    //public int[] beanRemainingArray;  //잔량이었는데 이거 안한대. 이거때매 기획 바꿀뻔해서 살짝 화날뻔.

    public SaveDataClass()
    {
        //테스트용 생성자. 그냥 3개씩 다 만들어준ㄴ겁니다.
        lastPlayTime = DateTime.Now.ToString();
        potList = new ComponentClass[3];
        boughtNameList = new List<string>();
        boughtDateList = new List<string>();
        huntCharacterList = new List<CharacterClass>();
        mineCharacterList = new List<CharacterClass>();
        fishCharacterList = new List<CharacterClass>();


        characterList = new List<CharacterClass>();

        owningComponentList = new List<ComponentClass>();
        ComponentClass component = new ComponentClass();
        component.name = "body";
        owningComponentList.Add(component);
        component = new ComponentClass();
        component.name = "hand";
        owningComponentList.Add(component);
        fishElement = 100;
        mineElement = 100;
        huntElement = 100;
        coin = 5000;
        trainSelled = false;
        chairSelled = false;
        fishTutorial = false;
        mineTutorial = false;

        tutorialOrder = 0;
    }
}


