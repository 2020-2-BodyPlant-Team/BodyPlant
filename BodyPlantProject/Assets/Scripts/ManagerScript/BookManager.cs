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
    List<CharacterClass> huntCharacterList;
    List<CharacterClass> mineCharacterList;
    List<CharacterClass> fishCharacterList;
    List<CharacterClass> totalList;
    public GameObject diaryPrefab;
    public GameObject buttonPrefab;
    List<GameObject> diaryList;
    List<GameObject> buttonList;
    List<GameObject> silhouette;
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
        huntCharacterList = saveData.huntCharacterList;
        mineCharacterList = saveData.mineCharacterList;
        fishCharacterList = saveData.fishCharacterList;


        totalList = new List<CharacterClass>();
        for(int i = 0; i < characterList.Count; i++)
        {
            totalList.Add(characterList[i]);
        }
        for(int i = 0; i < huntCharacterList.Count; i++)
        {
            totalList.Add(huntCharacterList[i]);
        }
        for(int i = 0; i < mineCharacterList.Count; i++)
        {
            totalList.Add(mineCharacterList[i]);
        }
        for(int i = 0; i < fishCharacterList.Count; i++)
        {
            totalList.Add(fishCharacterList[i]);
        }

        diaryList = new List<GameObject>();
        buttonList = new List<GameObject>();

        contentRect.anchoredPosition = new Vector2(0, 0);   
        contentRect.sizeDelta = new Vector2(0, (characterList.Count / 3) * (-buttonYgap));
        buttonStartPoint = new Vector2(-300, contentRect.sizeDelta.y / 2 - 180);
        
        GookBabMukGoSipDa(diaryList, buttonList, totalList);

        silhouette = new List<GameObject>();
        for(int i = 0; i < totalList.Count; i++)
        {
            GameObject black = Instantiate(totalList[i].realGameobject, buttonList[i].transform);
            silhouette.Add(black);          
            SpriteRenderer[] spriteArray = silhouette[i].GetComponentsInChildren<SpriteRenderer>();
            Debug.Log(spriteArray.Length);
            for(int j = 0; j < spriteArray.Length; j++)
            {
                spriteArray[j].color = Color.black;
            }
            silhouette[i].transform.localScale *= 0.35f;
            silhouette[i].transform.localPosition = new Vector3(0, 0, -0.5f);
        }

        for(int i = 0; i < totalList.Count; i++)
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
        for(int i = 0; i < totalList.Count; i++)
        {
            GameObject button = EventSystem.current.currentSelectedGameObject;
            if(button == buttonList[i])
            {
                diaryList[i].SetActive(true);
            }
        }
    }

    public void GookBabMukGoSipDa(List<GameObject> diaryList, List<GameObject> buttonList, List<CharacterClass> characterList)
    {
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
            parent.transform.localPosition = new Vector3(0, 300, -1);

            int bodyNumber = 0;
            int armLegNumber = 0;
            int handFootNumber = 0;
            int earEyeNumber = 0;   //이목구비
            int hairNumber = 0;

            for(int k = 0; k < characterList[i].components.Count; k++)
            {
                ComponentClass component = characterList[i].components[k];
                string path;
                path = "Components/Complete/" + component.name;

                GameObject prefab = Resources.Load<GameObject>(path);
                GameObject inst = Instantiate(prefab,parent.transform);
                inst.transform.localPosition = component.position;
                inst.transform.eulerAngles = component.rotation;
                component.realGameobject = inst;
                
                string name = component.name;

                

                if (name == "body")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -1 - bodyNumber * 0.01f);
                    bodyNumber++;
                }
                if (name == "arm" || name == "leg")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -2 - armLegNumber * 0.01f);
                    armLegNumber++;
                }
                if (name == "hand" || name == "foot")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -3 - handFootNumber * 0.01f);
                    handFootNumber++;
                }
                if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -4 - earEyeNumber * 0.01f);
                    earEyeNumber++;
                }
                if (name == "hair")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -5 - hairNumber * 0.01f);
                    hairNumber++;
                }
            } 

            // 다이어리에 생성되는 캐릭터의 x, y 위치 중에서 최댓값과 최솟값 초기값 설정 
            float Xmin = characterList[i].components[0].position.x;
            float Xmax = characterList[i].components[0].position.x;
            float Ymin = characterList[i].components[0].position.y;
            float Ymax = characterList[i].components[0].position.y;

            // 최댓값 최솟값 구하기
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

            // 팔, 다리, 머리카락의 경우 세컨드 포지션까지 최대 최소 구하는데 포함해준다.
            for(int j = 0; j < characterList[i].components.Count; j++)
            {
                if(gameManager.FindData(characterList[i].components[j].name).isChild)
                {
                    if(Xmin > characterList[i].components[j].secondPosition.x)
                    {
                        Xmin = characterList[i].components[j].secondPosition.x;
                    }
                    if(Xmax < characterList[i].components[j].secondPosition.x)
                    {
                        Xmax = characterList[i].components[j].secondPosition.x;
                    }
                    if(Ymin > characterList[i].components[j].secondPosition.y)
                    {
                        Ymin = characterList[i].components[j].secondPosition.y;
                    }
                    if(Ymax < characterList[i].components[j].secondPosition.y)
                    {
                        Ymax = characterList[i].components[j].secondPosition.y;
                    }
                }
            }

            float Xgap = Xmax - Xmin;
            float Ygap = Ymax - Ymin;
            float diaryWidth = 4.4f;
            float diaryHeight = 3.4f;

            // 크기 다이어리 페이지에 맞게 조정해주기
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

            float xMinDiaryPos = -2.2f;
            float xMaxDiaryPos = 2.2f;
            float yMinDiaryPos = -0.4f;
            float yMaxDiaryPos = 3f;

            //만약 크기를 조정하고나서 캐릭터가 범위에 벗어나게 생성되었을 때 위치 조정해주기
            //알겠어
            for(int j = 0; j < characterList[i].components.Count; j++)
            {
                GameObject obj = characterList[i].components[j].realGameobject;
                if (obj.transform.position.x < xMinDiaryPos)
                {
                    Vector3 vector = parent.transform.position;
                    vector.x += (xMinDiaryPos - obj.transform.position.x);
                    parent.transform.position = vector;
                }

                if(obj.transform.position.x > xMaxDiaryPos)
                {
                    Vector3 vector = parent.transform.position;
                    vector.x -= (obj.transform.position.x - xMaxDiaryPos);
                    parent.transform.position = vector;
                }

                if(obj.transform.position.y < yMinDiaryPos)
                {
                    Vector3 vector = parent.transform.position;
                    vector.y += (yMinDiaryPos - obj.transform.position.y);
                    parent.transform.position = vector;
                }
                if(obj.transform.position.y > yMaxDiaryPos)
                {
                    Vector3 vector = parent.transform.position;
                    vector.y -= (obj.transform.position.y - yMaxDiaryPos);
                    parent.transform.position = vector;
                }
            }
            // 여기도 마찬가지로 팔, 다리, 머리카락의 세컨드 포지션까지 포함해줘
            for(int j = 0; j < characterList[i].components.Count; j++)
            {
                if(gameManager.FindData(characterList[i].components[j].name).isChild)
                {
                    Transform obj = characterList[i].components[j].realGameobject.transform.GetChild(1);
                    if (obj.position.x < xMinDiaryPos)
                    {
                        Vector3 vector = parent.transform.position;
                        vector.x += (xMinDiaryPos - obj.position.x);
                        parent.transform.position = vector;
                    }

                    if(obj.position.x > xMaxDiaryPos)
                    {
                        Vector3 vector = parent.transform.position;
                        vector.x -= (obj.position.x - xMaxDiaryPos);
                        parent.transform.position = vector;
                    }

                    if(obj.position.y < yMinDiaryPos)
                    {
                        Vector3 vector = parent.transform.position;
                        vector.y += (yMinDiaryPos - obj.position.y);
                        parent.transform.position = vector;
                    }
                    if(obj.position.y > yMaxDiaryPos)
                    {
                        Vector3 vector = parent.transform.position;
                        vector.y -= (obj.position.y - yMaxDiaryPos);
                        parent.transform.position = vector;
                    }
                }
            }

            characterList[i].realGameobject = parent;

            /*for(int k = 0; k < characterList[i].components.Count; k++)
            {
                ComponentClass component = characterList[i].components[k];
                GameObject prefab = Resources.Load<GameObject>("Components/Complete/" + component.name);
                GameObject inst = Instantiate(prefab,parent.transform);
                inst.transform.localPosition = component.position;
                inst.transform.eulerAngles = component.rotation;
            } */
        }
        
    }

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }    
}
