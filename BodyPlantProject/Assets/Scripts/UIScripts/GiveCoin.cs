using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GiveCoin : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;

    DateTime now;
    DateTime endedtime;

    float bonus; //누적된 코인 변수
    int sec;
    string endedString;
    string nowString;

    public GameObject bonusPack;
    public GameObject UICanvas;

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;

        now = DateTime.Now;
        endedtime = Convert.ToDateTime(saveData.lastPlayTime);  //saveData 속에 마지막 플탐 받아오기
        //DateTime endedtime = new DateTime(2021, 02, 01, 12, 17, 00);

        nowString = now.ToString();
        endedString = endedtime.ToString();
        InvokeRepeating("GiveBonus",0, 30); //의도: 30초에 한 번씩 누적된 코인 체크 
        //문제 발생: 다른 씬 다녀오면 초기화돼서 코인이 중복 지급됨
    }

    void GiveBonus()
    {
        bonusPack.SetActive(false);
        now = DateTime.Now;
        nowString = now.ToString(); //체크 당시 시간 실시간으로 넣기
        sec = gameManager.TimeSubtractionToSeconds(endedString, nowString); //게임매니저 속 타이머 사용
        //Debug.Log(sec);
        bonus = sec / 60 * 1.67f;   //1분당 1.67코인 누적

        if (bonus > 30)
        {
            bonusPack.SetActive(true);
            Vector3 pos;
            pos = UICanvas.transform.position;
            pos.x = UnityEngine.Random.Range(180f, 900f);
            pos.y = bonusPack.transform.position.y;
            bonusPack.transform.position = pos; //누적 코인 얻는 버튼 위치 이동
        }
        if (bonus >= 100)
        {
            bonus = 100;    //누적 코인이 100개 넘으면 == 1시간 이상이 지나면 누적량 100으로 고정
        }
        
        /*
        Debug.Log((int)bonus);
        Debug.Log(now);
        Debug.Log(endedtime);   //확인용 로그 세 줄 나중에 날려도 됨
        */
    }

    public void ClickBonus()
    {
        bonusPack.SetActive(false);
        //코인 변수에다 보너스 추가하기
        endedtime = DateTime.Now;
        endedString = endedtime.ToString(); //클릭 당시 시간을 새로운 누적 기준점으로 넣기
        //Debug.Log("클릭했음");
    }
}
