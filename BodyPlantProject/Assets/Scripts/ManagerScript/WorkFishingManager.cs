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
    RectTransform rt;

    public GameObject panjung;
    public float a = 0; //Coloring 코루틴에서 while 돌리려고 만든 변수입니다
    //float b = 0; 
    public bool touchforfish = false;
    bool ifSunggong = false;
    public CharacterMover characterMover;
    public Text fishElementText;

    public GameObject[] boatObjectArray;
    public GiveCoin coinManager;

    public WorkCharacterManager workCharacterManager;

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

        for(int i = 0; i < boatObjectArray.Length; i++)
        {
            boatObjectArray[i].SetActive(false);
        }

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        fishingBar.SetActive(false);
        iconAnimator = icon.GetComponent<Animator>();
        cor = Coloring();
        StartCoroutine(cor);

        for (int i = 0; i < characterList.Count; i++)
        {
            boatObjectArray[i].SetActive(true);
            GameObject characterObject;
            characterMover.SpawnCharacter(characterList[i],i);
            characterObject = characterList[i].realGameobject;
            characterObject.transform.position = new Vector3(boatObjectArray[i].transform.position.x, boatObjectArray[i].transform.position.y,0);
            characterObject.transform.localScale = boatObjectArray[i].transform.localScale;
            boatObjectArray[i].transform.SetParent(characterObject.transform);
        }

        coinManager.SetCharacterList(characterList, 1);
        fishElementText.text = saveData.fishElement.ToString();

        workCharacterManager.SetCharacterList(characterList);

    }

    IEnumerator Coloring()
    {
        icon.transform.localPosition = new Vector2(-300, icon.transform.localPosition.y);
        touchforfish = false;
        panjung.GetComponent<Image>().color = new Color(215 / 255f, 57 / 255f, 57 / 255f);
        WaitForSeconds loopTime = new WaitForSeconds(UnityEngine.Random.Range(1.0f, 4.0f));
        if (ifSunggong)
        {
            yield return loopTime;
            ifSunggong = false;
        }
        fishingBar.SetActive(true);
        animSpeed = UnityEngine.Random.Range(1.5f, 4f);
        iconAnimator.SetFloat("fishingSpeed", animSpeed);
        //iconAnimator.SetTrigger("isFishing");
        iconAnimator.SetBool("isFish", true);
        WaitForSeconds wait = new WaitForSeconds(0.02f);
        while (posX <= 351)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(new Color(215 / 255f, 57 / 255f, 57 / 255f), new Color(72 / 255f, 163 / 255f, 62 / 225f), a);
            a = (posX + 300) / 485;
            yield return wait;
            if (posX >= 345)
            {
                panjung.GetComponent<Image>().color = Color.black;
                yield return wait;
                break;
            }
        }
        iconAnimator.SetBool("isFish", false);
        fishingBar.SetActive(false);
        yield return loopTime;
        cor = Coloring();
        StartCoroutine(cor);
    }
    
    void Update()
    {
        characterMover.FishingUpdate();

        if(touchforfish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //보조성분 획득할 곳
                saveData.fishElement++;
                gameManager.Save();
                fishElementText.text = saveData.fishElement.ToString();
                Debug.Log("보조성분 획득");
                touchforfish = false;
                StopCoroutine(cor);
                fishingBar.SetActive(false);
                ifSunggong = true;
                cor = Coloring();
                StartCoroutine(cor);
            }
        }
    }
}
