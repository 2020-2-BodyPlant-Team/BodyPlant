using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GiveCoin : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<CharacterClass> characterList;

    /*
    DateTime now;
    DateTime endedtime;

    float bonus; //누적된 코인 변수
    int sec;
    string endedString;
    string nowString;

    public GameObject bonusPack;
    public GameObject UICanvas;
    */

    public GameObject[] coinButtonArray;
    public float wholeWorkingTime;
    float timeCoinRatio = 0.0283f;
    float maxCoin = 300;
    public float nowCoin;
    DateTime startTime;
    public Text coinText;

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;

        //now = DateTime.Now;
        //endedtime = Convert.ToDateTime(saveData.lastPlayTime);  //saveData 속에 마지막 플탐 받아오기
        //DateTime endedtime = new DateTime(2021, 02, 01, 12, 17, 00);

        //nowString = now.ToString();
        //endedString = endedtime.ToString();
        //InvokeRepeating("GiveBonus",0, 30); //의도: 30초에 한 번씩 누적된 코인 체크 
        //문제 발생: 다른 씬 다녀오면 초기화돼서 코인이 중복 지급됨  cor = Coloring();
        //StartCoroutine(cor);    //근데 이거 스타트에 있으면 바로 시작해야 되는거 아닌가요 왜 안되지ㅠㅠㅠㅠ
     
    }

    //0 hunt 1 fish 2 mine
    public void SetCharacterList(List<CharacterClass> list, int nowWork)
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        if (list == null)
        {
            return;
        }

        characterList = list;
        foreach (GameObject obj in coinButtonArray)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < characterList.Count; i++)
        {
            float workedTime = (float)gameManager.TimeSubtractionToSeconds(characterList[i].lastEarnedTime, DateTime.Now.ToString());
            if(nowWork == 0)
            {
                workedTime *= characterList[i].huntWorkRatio;
            }
            else if (nowWork == 1)
            {
                workedTime *= characterList[i].fishWorkRatio;
            }
            else
            {
                workedTime *= characterList[i].mineWorkRatio;
            }

            wholeWorkingTime += workedTime;
        }

        startTime = DateTime.Now;
        nowCoin = wholeWorkingTime * timeCoinRatio;
        if (nowCoin >= 50)
        {
            bool exist = false;
            for (int i = 0; i < coinButtonArray.Length; i++)
            {
                exist = coinButtonArray[i].activeSelf;
                if(exist == true)
                {
                    break;
                }
            }
            if (exist == false)
            {
                Debug.Log("스타트");
                coinButtonArray[UnityEngine.Random.Range(0, coinButtonArray.Length)].SetActive(true);
            }
        }
        if (nowCoin > maxCoin)
        {
            nowCoin = maxCoin;
        }
        StartCoroutine(CoinEarnCoroutine());
        coinText.text = saveData.coin.ToString();

    }

    IEnumerator CoinEarnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < characterList.Count; i++)
            {
                float timePerSecond = characterList[i].fishWorkRatio;
                characterList[i].workEndTime = DateTime.Now.ToString();
                wholeWorkingTime += timePerSecond;
                characterList[i].fishTime++;
            }
            nowCoin = wholeWorkingTime * timeCoinRatio;
            if (nowCoin >= 50)
            {
                bool exist = false;
                for(int i = 0; i< coinButtonArray.Length; i++)
                {
                    exist = coinButtonArray[i].activeSelf;
                    if (exist == true)
                    {
                        break;
                    }
                }
                if(exist == false)
                {
                    Debug.Log("코루틴");
                    coinButtonArray[UnityEngine.Random.Range(0, coinButtonArray.Length)].SetActive(true);
                }
               
            }
            if (nowCoin > maxCoin)
            {
                nowCoin = maxCoin;
            }
        }
    }

    public void CoinEarn()
    {
        saveData.coin += (int)nowCoin;
        wholeWorkingTime = 0;
        nowCoin = 0;
        for (int i = 0; i < characterList.Count; i++)
        {
            startTime = DateTime.Now;
            characterList[i].lastEarnedTime = DateTime.Now.ToString();
        }
        foreach(GameObject obj in coinButtonArray)
        {
            obj.SetActive(false);
        }
        gameManager.Save();
        coinText.text = saveData.coin.ToString();
    }


    /*
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
       
    }

    public void ClickBonus()
    {
        bonusPack.SetActive(false);
        //코인 변수에다 보너스 추가하기
        endedtime = DateTime.Now;
        endedString = endedtime.ToString(); //클릭 당시 시간을 새로운 누적 기준점으로 넣기
        //Debug.Log("클릭했음");
    }*/
}
