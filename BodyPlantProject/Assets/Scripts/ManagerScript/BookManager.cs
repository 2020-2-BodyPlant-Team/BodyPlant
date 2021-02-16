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
    Vector2 buttonStartPoint = new Vector2(-300, -100);
    public GameObject buttonParent;
    public GameObject diaryParent;
    public RectTransform contentRect;


    // Start is called before the first frame update
    void Start()
    {        
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;

        diaryList = new List<GameObject>();
        buttonList = new List<GameObject>();

        contentRect.anchoredPosition = new Vector2(0, 0);   
        contentRect.sizeDelta = new Vector2(0, (characterList.Count / 3) * (-buttonYgap));
        buttonStartPoint = new Vector2(-300, contentRect.sizeDelta.y / 2 - 180);

        

        for(int i = 0; i < characterList.Count; i++)
        { 
            int elapsedTime;
            

            diaryList.Add(Instantiate(diaryPrefab, diaryParent.transform));                   
            buttonList.Add(Instantiate(buttonPrefab, buttonParent.transform));

            Vector2 buttonPosition = new Vector2((buttonStartPoint.x + (i % 3) * buttonXgap), (buttonStartPoint.y  + (int)(i / 3) * buttonYgap));
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            buttonList[i].GetComponent<Button>().onClick.AddListener(delegate { ButtonFunction(); });


            DateTime date = DateTime.Parse(characterList[i].createdDate);
            Debug.Log(characterList[i].createdDate);
            diaryList[i].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = date.ToString("yyyy년 M월 d일");
            

            if(characterList[i].personality == CharacterClass.Personality.Mongsil)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "몽실몽실";
            }
            if(characterList[i].personality == CharacterClass.Personality.Dugun)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "두근두근";
            }
            if(characterList[i].personality == CharacterClass.Personality.Ussuk)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "으쓱으쓱";
            }
            if(characterList[i].personality == CharacterClass.Personality.Nunsil)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "는실는실";
            }
            
            diaryList[i].transform.GetChild(1).GetChild(3).GetComponent<Text>().text = characterList[i].loveNess.ToString();
            diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<Text>().text = characterList[i].name;

            elapsedTime = gameManager.TimeSubtractionToSeconds(characterList[i].createdDate, DateTime.Now.ToString());
            int days = elapsedTime / (60 * 60 * 24);
            diaryList[i].transform.GetChild(1).GetChild(5).GetComponent<Text>().text = days.ToString();  

            GameObject parent = new GameObject();
            parent.transform.SetParent(diaryList[i].transform);
            parent.transform.localPosition = new Vector3(0, 400, -1);
            //parent.transform.localScale = new Vector3(100, 100, 100);

            for(int k = 0; k < characterList[i].components.Count; k++)
            {
                ComponentClass component = characterList[i].components[k];
                GameObject prefab = Resources.Load<GameObject>("Components/Complete/" + component.name);
                GameObject inst = Instantiate(prefab,parent.transform);
                inst.transform.localPosition = component.position;
                inst.transform.eulerAngles = component.rotation;
            } 

            float Xmin = characterList[i].components[0].position.x;
            float Xmax = characterList[i].components[0].position.x;
            float Ymin = characterList[i].components[0].position.y;
            float Ymax = characterList[i].components[0].position.y;

            for(int j = 1; j < characterList[i].components.Count; j++)
            {
                if(Xmin > characterList[i].components[j].position.x)
                {
                    Xmin = characterList[i].components[j].position.x;
                }
                if(Xmax < characterList[i].components[j].position.x)
                {
                    Xmax = characterList[i].components[j].position.x;
                }
                if(Ymin > characterList[i].components[j].position.y)
                {
                    Ymin = characterList[i].components[j].position.y;
                }
                if(Ymax < characterList[i].components[j].position.y)
                {
                    Ymax = characterList[i].components[j].position.y;
                }
            }

            float Xgap = Xmax - Xmin;
            float Ygap = Ymax - Ymin;
            float diaryWidth = 2;
            float diaryHeight = 1.5f;

            if(Xgap > diaryWidth && Ygap < diaryHeight)
            {
                float ratio = diaryWidth / Xgap;
                parent.transform.localScale *= ratio;
            }
            if(Xgap < diaryWidth && Ygap > diaryHeight)
            {
                float ratio = diaryHeight / Ygap;
                parent.transform.localScale *= ratio;
            }
            if(Xgap > diaryWidth && Ygap > diaryHeight)
            {
                float Xratio = diaryWidth / Xgap;
                float Yratio = diaryHeight / Ygap;
                if(Xratio >= Yratio)
                {
                    parent.transform.localScale *= Xratio;
                }
                if(Xratio < Yratio)
                {
                    parent.transform.localScale *= Yratio;
                }
            }

            /*for(int k = 0; k < characterList[i].components.Count; k++)
            {
                ComponentClass component = characterList[i].components[k];
                GameObject prefab = Resources.Load<GameObject>("Components/Complete/" + component.name);
                GameObject inst = Instantiate(prefab,parent.transform);
                inst.transform.localPosition = component.position;
                inst.transform.eulerAngles = component.rotation;
            } */
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
