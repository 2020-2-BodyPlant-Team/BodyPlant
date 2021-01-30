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
        Mongsil,Dugun,Ussuk,Nunsil
            //몽실몽실, 두근두근, 으쓱으쓱, 는실는실
    }
    public string name;                     //이름;
    public Personality personality;         //성격
    public string createdDate;              //만들어진 날짜
    public int[] workedTime;                //int[3]배열. 낚시 사냥 광질 얼마나 했는지.
    public float loveNess;                  //애정도
    public List<ComponentClass> components; //어떤 부위가 들러붙어있는지

    public CharacterClass()
    {
        name = "null";

        createdDate = DateTime.Now.ToString();
        personality = (Personality)UnityEngine.Random.Range(0,3);
        workedTime = new int[3];
        for(int i = 0; i < 3; i++)
        {
            workedTime[i] = 0;
        }
        loveNess = 0;
    }

    
}
