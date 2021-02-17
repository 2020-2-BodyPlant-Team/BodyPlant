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

    public GameObject canvas;
    public GameObject icon;
    public GameObject bringButton;
    public GameObject coinButton;
    public float wholeWorkingTime;
    float timeCoinRatio = 0.0283f;
    public float nowCoin;
    DateTime startTime;

    Animator iconAnimator;
    public float posX;
    public float animSpeed;
    float loopTime;

    public GameObject panjung;
    float a = 0;
    public float b = 0;
    public CharacterMover characterMover;

    public GameObject[] boatObjectArray;
    public Text coinText;


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
        canvas.SetActive(false);
        
        StartCoroutine("Coloring");
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
        canvas.SetActive(true);
        animSpeed = UnityEngine.Random.Range(1f, 5f);
        iconAnimator.SetFloat("fishingSpeed", animSpeed);
        iconAnimator.SetTrigger("isFishing");
        while (posX <= 0)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(Color.red, Color.yellow, a);
            a = (posX + 300) / 300;
            yield return new WaitForSeconds(0.03f);
        }
        while (posX >= 0)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.green, b);
            b = posX / 300;
            yield return new WaitForSeconds(0.03f);
            if (b >= 1)
            {
                if (Input.GetMouseButton(0))
                {
                    //보조성분 추가할 곳
                    Debug.Log("보조성분 획득");
                }
            }
            //판정 바가 검정색으로 변하는 것 아직 구현x
        }
        canvas.SetActive(false);
        loopTime = UnityEngine.Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(loopTime);
        StartCoroutine("Coloring");
    }

    void Update()
    {
        characterMover.FishingUpdate();


    
    }
}
