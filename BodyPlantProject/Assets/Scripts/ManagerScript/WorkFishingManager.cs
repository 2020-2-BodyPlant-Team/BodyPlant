using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkFishingManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    List<CharacterClass> characterList;

    public GameObject fishingBar;
    public GameObject icon;
    public GameObject bringButton;
    public GameObject coinButton;
    public float wholeWorkingTime;
    float timeCoinRatio = 0.0283f;
    float maxCoin = 100;
    public float nowCoin;
    DateTime startTime;

    Animator iconAnimator;
    public float posX;
    public float animSpeed;
    float loopTime;

    public GameObject panjung;
    float a = 0;
    float b = 0; //Coloring 코루틴에서 while 돌리려고 만든 변수입니다
    public bool touchforfish = false;
    public CharacterMover characterMover;

    public GameObject[] boatObjectArray;
    public Text coinText;

    IEnumerator cor;    //Coloring 코루틴 일시정지용

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }

    public void BringBtnOnClick()
    {
        gameManager.SecretRoomSceneLoad();
    }

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.fishCharacterList;
        if (characterList.Count > 3)
        {
            Application.Quit(); //몰라 꺼버려 ㅋㅋ
        }
        if(characterList.Count == 3 || saveData.characterList.Count ==0)
        {
            bringButton.SetActive(false);
        }

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;

        iconAnimator = icon.GetComponent<Animator>();

        cor = Coloring();
        StartCoroutine(cor);    //근데 이거 스타트에 있으면 바로 시작해야 되는거 아닌가요 왜 안되지ㅠㅠㅠㅠ
        coinButton.SetActive(false);
        
        for(int i = 0; i < characterList.Count; i++)
        {
            GameObject characterObject;
            characterMover.SpawnCharacter(characterList[i],i);
            characterObject = characterList[i].realGameobject;
            characterObject.transform.position = new Vector3(boatObjectArray[i].transform.position.x, boatObjectArray[i].transform.position.y,0);
            characterObject.transform.localScale = boatObjectArray[i].transform.localScale;
            boatObjectArray[i].transform.SetParent(characterObject.transform);

            float workedTime = (float)gameManager.TimeSubtractionToSeconds(characterList[i].lastEarnedTime, DateTime.Now.ToString());
            workedTime *= characterList[i].fishWorkRatio;
            wholeWorkingTime += workedTime;
        }
        startTime = DateTime.Now;
        nowCoin = wholeWorkingTime * timeCoinRatio;
        if(nowCoin >= 50)
        {
            coinButton.SetActive(true);
        }
        if(nowCoin > maxCoin)
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
                coinButton.SetActive(true);
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
        coinButton.SetActive(false);
        gameManager.Save();
        coinText.text = saveData.coin.ToString();
    }

    IEnumerator Coloring()
    {
        fishingBar.SetActive(true);
        animSpeed = UnityEngine.Random.Range(1.5f, 4f);
        iconAnimator.SetFloat("fishingSpeed", animSpeed);
        iconAnimator.SetTrigger("isFishing");
        while (posX <= -52)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(Color.red, Color.yellow, a);
            a = (posX + 300) / 248; //(posX + 300) / 300
            yield return new WaitForSeconds(0.02f);
        }
        while (posX > -52)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.green, b);
            b = (posX + 52) / 247;  //posX / 300
            //if (panjung.GetComponent<Image>().color == Color.green)
            if(posX > 195)
            {
                touchforfish = true;
            }
            if(posX > 346)
            {
                panjung.GetComponent<Image>().color = Color.black;
                touchforfish = false;
                iconAnimator.SetFloat("fishingSpeed", 0);
                yield return new WaitForSeconds(0.2f);
                iconAnimator.SetFloat("fishingSpeed", 1);   //의도:판정바 검정색 된 거 좀 보여주고 가라
            }
            yield return new WaitForSeconds(0.02f);
        }
        fishingBar.SetActive(false);
        touchforfish = false;
        loopTime = UnityEngine.Random.Range(1.0f, 4.0f);
        yield return new WaitForSeconds(loopTime);
        cor = Coloring();
        StartCoroutine(cor);
    }

    void Update()
    {
        characterMover.FishingUpdate();

        if(touchforfish == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //보조성분 추가할 곳
                Debug.Log("보조성분 획득");
                touchforfish = false;
                StopCoroutine(cor);
                fishingBar.SetActive(false);
                cor = Coloring();
                StartCoroutine(cor);    //의도:낚시 성공하면 코루틴 바로 끝내고 새로 시작해라
            }
        }
    }
}
