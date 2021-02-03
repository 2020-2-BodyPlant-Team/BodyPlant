using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BookManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<CharacterClass> characterList;
    public GameObject diaryPrefab;
    public GameObject buttonPrefab;
    List<GameObject> diaryList;
    List<GameObject> buttonList;
    List<int> passedTime;
    int buttonXgap = 300;
    int buttonYgap = -415;    
    Vector2 buttonStartPoint = new Vector2(-300, 370);
    public GameObject parent;


    // Start is called before the first frame update
    void Start()
    {        
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;

        diaryList = new List<GameObject>();
        buttonList = new List<GameObject>();

        for(int i = 0; i < characterList.Count; i++)
        { 
            int elapsedTime;

            diaryList.Add(Instantiate(diaryPrefab, parent.transform));                   
            buttonList.Add(Instantiate(buttonPrefab, parent.transform));

            Vector2 buttonPosition = new Vector2((buttonStartPoint.x + (i % 3) * buttonXgap), (buttonStartPoint.y  + (int)(i / 3) * buttonYgap));
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            buttonList[i].GetComponent<Button>().onClick.AddListener(delegate { ButtonFunction(); });

            diaryList[i].transform.GetChild(1).GetComponent<Text>().text = characterList[i].createdDateTime.ToString("yyyy년 M월 d일");
            
            if(characterList[i].personality == CharacterClass.Personality.Mongsil)
            {
                diaryList[i].transform.GetChild(2).GetComponent<Text>().text = "몽실몽실";
            }
            if(characterList[i].personality == CharacterClass.Personality.Dugun)
            {
                diaryList[i].transform.GetChild(2).GetComponent<Text>().text = "두근두근";
            }
            if(characterList[i].personality == CharacterClass.Personality.Ussuk)
            {
                diaryList[i].transform.GetChild(2).GetComponent<Text>().text = "으쓱으쓱";
            }
            if(characterList[i].personality == CharacterClass.Personality.Nunsil)
            {
                diaryList[i].transform.GetChild(2).GetComponent<Text>().text = "는실는실";
            }
            
            diaryList[i].transform.GetChild(3).GetComponent<Text>().text = characterList[i].loveNess.ToString();
            diaryList[i].transform.GetChild(4).GetComponent<Text>().text = characterList[i].name + " 와(과) 지낸지";

            elapsedTime = gameManager.TimeSubtractionToSeconds(characterList[i].createdDate, DateTime.Now.ToString());
            int days = elapsedTime / (60 * 60 * 24);
            diaryList[i].transform.GetChild(5).GetComponent<Text>().text = days.ToString() + " 일이 지났어";              
        }

        for(int i = 0; i < characterList.Count; i++)
        {
            diaryList[i].GetComponent<RectTransform>().SetAsLastSibling();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonFunction()
    {      
        for(int i = 0; i < characterList.Count; i++)
        {
            GameObject button = EventSystem.current.currentSelectedGameObject;
            if(button == buttonList[i])
            {
                diaryList[i].SetActive(true);
            }
        }
    }

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }
    
}
