using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//게임 시작 및 씬을 넘나들 때 사용하는 스크립트
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    JsonManager jsonManager;
    public WholeComponents wholeComponents;    //테스트용
    public SaveDataClass saveData;


    void Start()
    {
        jsonManager = new JsonManager();
        wholeComponents = jsonManager.LoadComponents();
        Load();
    }

    
    //세이브데이터 세이브
    void Save()
    {
        jsonManager.SaveJson(saveData);
    }

    //데이터 로드
    void Load()
    {
        saveData = jsonManager.LoadSaveData();
    }
   

    //시간계산기입니다. 예전시간과 현재시간을 넣으면 그 사이의 초가 나와요.
    //초를 알아야 얼마나 지났는지 아니까 그거에 맞게 방치가 되어있다는 걸 깨닫고 화분을 싹틔우던 하겠죠?
    public int TimeSubtractionToSeconds(string pastTime, string latestTime)
    {
        DateTime past = DateTime.Parse(pastTime);
        DateTime latest = DateTime.Parse(latestTime);
        //string to datetime
        if (past == null || latest == null)
        {
            Debug.Log("time is null");
            Application.Quit();
            return 0;
        }

        TimeSpan span = latest - past;
        int seconds = (int)span.TotalSeconds;
        Debug.Log("subtraction is " + seconds);

        return seconds;
    }

    public int TimeSubtractionToSeconds(string pastTime, DateTime latestTime)
    {
        DateTime past = DateTime.Parse(pastTime);
        DateTime latest = latestTime;
        //string to datetime
        if (past == null || latest == null)
        {

            Application.Quit();
            return 0;
        }

        TimeSpan span = latest - past;
        int seconds = (int)span.TotalSeconds;
        //Debug.Log("subtraction is " + seconds);

        return seconds;
    }

    public int TimeSubtractionToSeconds(DateTime pastTime, string latestTime)
    {
        DateTime past = pastTime;
        DateTime latest = DateTime.Parse(latestTime);
        //string to datetime
        if (past == null || latest == null)
        {

            Application.Quit();
            return 0;
        }

        TimeSpan span = latest - past;
        int seconds = (int)span.TotalSeconds;
        //Debug.Log("subtraction is " + seconds);

        return seconds;
    }
}
