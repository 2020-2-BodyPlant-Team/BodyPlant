using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터의 클래스
[System.Serializable]
public class CharacterClass
{
    public enum Personality     //성격 enum. 굳이 없어도 되는데 편하니까 만드는거에요. 걍 int로 만들고 1 2 3 4 해도됩니다.
    {
        Mongsil,Ggumul,Puksin,Jogon
            //몽실몽실, 두근두근, 는실는실, 으쓱으쓱
            //차례대로 눈치껏 만들어라 ㅋㅋ
    }
    public string name;                     //이름;
    public Personality personality;         //성격
    public string createdDate;              //만들어진 날짜
    public DateTime createdDateTime;        //년월일 쓰기 위해서 만듦
    public string lastEarnedTime;         //최근에 수금한 날짜.
    public string workEndTime;              //가장 마지막 일에서 나온 날짜.
    public string loveStartTime;            //애정도가 차오르는 그거.

    public int huntTime;
    public int fishTime;
    public int mineTime;
    public int loveTime;                    //애정도가 올라간 시간.
    public float huntWorkRatio;
    public float fishWorkRatio;
    public float mineWorkRatio;
    public float loveNess;                  //애정도
    public float xGap;
    public float yGap;
    public List<ComponentClass> components; //어떤 부위가 들러붙어있는지
    public GameObject realGameobject;

    public CharacterClass()
    {
        name = "null";

        createdDate = DateTime.Now.ToString();
        createdDateTime = DateTime.Now;
        lastEarnedTime = DateTime.Now.ToString();
        //personality = (Personality)UnityEngine.Random.Range(0,3);
        huntTime = 0;
        fishTime = 0;
        mineTime = 0;
        huntWorkRatio = 1;
        fishWorkRatio = 1;
        mineWorkRatio = 1;
        loveStartTime = DateTime.Now.ToString();
        loveTime = 0;
        loveNess = 0;
    }

    
}
