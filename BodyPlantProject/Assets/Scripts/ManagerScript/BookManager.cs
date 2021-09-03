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
    public List<CharacterClass> totalList;
    public GameObject diaryPrefab;
    public GameObject buttonPrefab;
    public List<GameObject> stickerPrefab;
    List<GameObject> diaryList;
    List<GameObject> buttonList;
    List<GameObject> silhouette;
    List<int> passedTime;
    int buttonXgap = 350;
    int buttonYgap = -415;    
    Vector2 buttonStartPoint = new Vector2(-400, -300);
    public GameObject buttonParent;
    public GameObject diaryParent;
    public RectTransform contentRect;
    public List<RectTransform> lovenessMaskList;    //애정도에 마스크 올라갔다 내려갔다 해야되는데 이거임. 차례대로 쓰면 댐.

    Vector2 lovenessZero = new Vector2(0, 19);
    Vector2 lovenessFull = new Vector2(0, 128);
    RaycastHit2D hit;
    GameObject touchedObject;
    public Camera cam;
    private StickerClass touchedStickerClass;
    public GameObject scrollViewObject;
    TutorialMngInBook tutorialMngInBook;



    // Start is called before the first frame update
    void Start()
    {        
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;
        huntCharacterList = saveData.huntCharacterList;
        mineCharacterList = saveData.mineCharacterList;
        fishCharacterList = saveData.fishCharacterList;
        tutorialMngInBook = FindObjectOfType<TutorialMngInBook>();


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
        lovenessMaskList = new List<RectTransform>();

        contentRect.anchoredPosition = new Vector2(0, 0);   
        contentRect.sizeDelta = new Vector2(0, ((characterList.Count / 3) + 1) * (-buttonYgap));
        buttonStartPoint = new Vector2(-350, (contentRect.sizeDelta.y / 2) - 250);
        
        GookBabMukGoSipDa(diaryList, buttonList, totalList);

        StartCoroutine(LovenessCoroutine());

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
            silhouette[i].transform.localPosition = new Vector3(0, 0, 6052f);
            for(int j = 0; j < silhouette[i].transform.childCount; j++)
            {
                silhouette[i].transform.GetChild(j).GetComponent<SpriteRenderer>().sortingOrder = 2;
            }

            diaryList[i].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(StickerBtnFunction);
        }

        for(int i = 0; i < totalList.Count; i++)
        {
            diaryList[i].GetComponent<RectTransform>().SetAsLastSibling();
        }

        for(int i = 0; i < totalList.Count; i++)
        {
            for(int j = 0; j < totalList[i].stickerList.Count; j++)
            {
                GameObject sticker = Instantiate(stickerPrefab[totalList[i].stickerList[j].stickerPrefabIndex], diaryList[i].transform);
                sticker.GetComponent<BoxCollider2D>().enabled = false;
                sticker.transform.GetComponent<RectTransform>().SetAsLastSibling();
                sticker.transform.position = totalList[i].stickerList[j].position;
            }
        }

        Btn.scrollView = scrollViewObject;
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject pointer = EventSystem.current.currentSelectedGameObject;
        for(int i = 0; i < totalList.Count; i++)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                Debug.Log(touchedObject);
                if(totalList[i].loveNess >= 100)
                {
                    if(touchedObject.name == "Loveness" && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        int randomNum = UnityEngine.Random.Range(0, 15);
                        GameObject sticker = Instantiate(stickerPrefab[randomNum]);

                        if(randomNum / 5 == 0)
                        {
                            totalList[i].fishWorkRatio += 0.5f;
                        }
                        else if(randomNum / 5 == 1)
                        {
                            totalList[i].huntWorkRatio += 0.5f;
                        }
                        else if(randomNum / 5 == 2)
                        {
                            totalList[i].mineWorkRatio += 0.5f;
                        }

                        sticker.transform.SetParent(diaryList[i].transform);
                        sticker.GetComponent<BoxCollider2D>().enabled = true;
                        sticker.transform.localScale = new Vector3(100, 100, 1);
                        sticker.transform.position = new Vector3(0, 2.5f, -0.3f);
                        StickerClass stickerClass = new StickerClass();
                        totalList[i].stickerList.Add(stickerClass);
                        stickerClass.position = sticker.transform.position;
                        stickerClass.stickerPrefabIndex = randomNum;
                        stickerClass.characterName = totalList[i].name;
                        stickerClass.stickerObject = sticker;
                        stickerClass.isFirstTimeOfInstantiation = true;

                        //스티커 생성시 붙이기 버튼 SetActive
                        diaryList[i].transform.GetChild(2).gameObject.SetActive(true);
                        if(saveData.tutorialOrder == 6)
                        {
                            diaryList[i].transform.GetChild(2).transform.SetParent(tutorialMngInBook.parentObj.transform);
                        }

                        //sticker.transform.SetParent(diaryList[i].transform);
                        sticker.transform.GetComponent<RectTransform>().SetAsLastSibling();
                        totalList[i].loveNess = 0;

                        gameManager.Save();
                    }
                }

    
                //GameObject touchedStickerObject = EventSystem.current.currentSelectedGameObject;
                if(touchedObject != null)
                {
                    if(touchedObject.CompareTag("Sticker") && Input.GetKey(KeyCode.Mouse0))
                    {
                        Debug.Log(touchedObject);
                        for(int j = 0; j < totalList[i].stickerList.Count; j++)
                        {
                            if(touchedObject == totalList[i].stickerList[j].stickerObject && totalList[i].stickerList[j].isFirstTimeOfInstantiation == true)
                            {
                                if(mousePos.x > 2.2)
                                {
                                    mousePos.x = 2.2f;
                                }
                                else if(mousePos.x < -2.2)
                                {
                                    mousePos.x = -2.2f;
                                }
                                else if(mousePos.y > 3)
                                {
                                    mousePos.y = 3;
                                }
                                else if(mousePos.y < -0.4)
                                {
                                    mousePos.y = -0.4f;
                                }
                                totalList[i].stickerList[j].stickerObject.transform.position = mousePos;
                                totalList[i].stickerList[j].position = mousePos;
                                touchedStickerClass = totalList[i].stickerList[j];
                            }
                        }
                        
                    }
                }
            }           
        }
    }

    IEnumerator LovenessCoroutine()
    {
        float timer = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            gameManager.UpdateLoveness();
            for(int i = 0; i < totalList.Count; i++)
            {
                //diaryList[i].transform.GetChild(1).GetChild(3).GetComponent<Text>().text = characterList[i].loveNess.ToString("N1");
                lovenessMaskList[i].anchoredPosition = Vector2.Lerp(lovenessZero, lovenessFull, totalList[i].loveNess/100.0f);
            }
            
            timer += Time.deltaTime;
            if(timer >= 10)
            {
                timer = 0;
                gameManager.Save();
            }
            
        }
        
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
        scrollViewObject.SetActive(false);
        if(saveData.tutorialOrder == 6)
        {
            tutorialMngInBook.isPlantBtnClicked = true;
        }
    }

    public void StickerBtnFunction()
    {
        touchedStickerClass.isFirstTimeOfInstantiation = false;
        GameObject button = EventSystem.current.currentSelectedGameObject;
        tutorialMngInBook.isStickerAttatched = true;
        gameManager.Save();
        button.SetActive(false);        
    }

    public void GookBabMukGoSipDa(List<GameObject> diaryList, List<GameObject> buttonList, List<CharacterClass> characterList)
    {
        gameManager.UpdateLoveness();
        for(int i = 0; i < characterList.Count; i++)
        { 
            int elapsedTime;
            

            diaryList.Add(Instantiate(diaryPrefab, diaryParent.transform));                   
            buttonList.Add(Instantiate(buttonPrefab, buttonParent.transform));
            lovenessMaskList.Add(diaryList[i].transform.GetChild(1).GetChild(3).GetChild(0).gameObject.GetComponent<RectTransform>());
            lovenessMaskList[i].anchoredPosition = Vector2.Lerp(lovenessZero, lovenessFull, characterList[i].loveNess/100.0f);


            Vector2 buttonPosition = new Vector2((buttonStartPoint.x + (i % 3) * buttonXgap), (buttonStartPoint.y  + (int)(i / 3) * buttonYgap));
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            buttonList[i].GetComponent<Button>().onClick.AddListener(delegate { ButtonFunction(); });




            DateTime date = DateTime.Parse(characterList[i].createdDate);
            Debug.Log(characterList[i].createdDate);
            diaryList[i].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = date.ToString("yyyy년 M월 d일");
            

            if(characterList[i].personality == CharacterClass.Personality.Mongsil)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "몽실몽실";
                diaryList[i].transform.GetChild(1).GetChild(10).gameObject.SetActive(true);
            }
            if(characterList[i].personality == CharacterClass.Personality.Ggumul)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "꾸물꾸물";
                diaryList[i].transform.GetChild(1).GetChild(11).gameObject.SetActive(true);
            }
            if(characterList[i].personality == CharacterClass.Personality.Puksin)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "푹신푹신";
                diaryList[i].transform.GetChild(1).GetChild(12).gameObject.SetActive(true);
            }
            if(characterList[i].personality == CharacterClass.Personality.Jogon)
            {
                diaryList[i].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "조곤조곤";
                diaryList[i].transform.GetChild(1).GetChild(13).gameObject.SetActive(true);
            }
            
            //diaryList[i].transform.GetChild(1).GetChild(3).GetComponent<Text>().text = characterList[i].loveNess.ToString("N1");
            diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<Text>().text = characterList[i].name;
            string characterName = diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<Text>().text;

            //글자수에 따라 간격 조정해주기
            if(characterName.Length == 9)
            {
                diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<LetterSpacing>().spacing = 8;
            }
            if(characterName.Length == 8)
            {
                diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<LetterSpacing>().spacing = 12;
            }
            if(characterName.Length < 8 && characterName.Length >= 4)
            {
                diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<LetterSpacing>().spacing = 20;
            }
            if(characterName.Length <= 3)
            {
                diaryList[i].transform.GetChild(1).GetChild(4).GetComponent<LetterSpacing>().spacing = 30;
            }

            elapsedTime = gameManager.TimeSubtractionToSeconds(characterList[i].createdDate, DateTime.Now.ToString());
            int days = elapsedTime / (60 * 60 * 24);
            diaryList[i].transform.GetChild(1).GetChild(5).GetComponent<Text>().text = days.ToString();  

            //fishing 일한 일수
            diaryList[i].transform.GetChild(1).GetChild(6).GetComponent<Text>().text = (characterList[i].fishTime / 86400).ToString() + "일";
            //mining 일한 일수
            diaryList[i].transform.GetChild(1).GetChild(7).GetComponent<Text>().text = (characterList[i].mineTime / 86400).ToString() + "일";
            //hunting 일한 일수
            diaryList[i].transform.GetChild(1).GetChild(8).GetComponent<Text>().text = (characterList[i].huntTime / 86400).ToString() + "일";

            GameObject parent = new GameObject();
            parent.transform.SetParent(diaryList[i].transform);
            parent.transform.localPosition = new Vector3(0, 300, -1);

            int bodyNumber = 0;
            int armLegNumber = 0;
            int handFootNumber = 0;
            int earNumber = 0;
            int moutNumber = 0;
            int noseNumber = 0;
            int eyeNumber = 0;
            int hairNumber = 0;

            for(int k = 0; k < characterList[i].components.Count; k++)
            {
                ComponentClass component = characterList[i].components[k];
                string path;
                path = "Components/Complete/" + component.name;

                GameObject prefab = Resources.Load<GameObject>(path);
                GameObject inst = Instantiate(prefab, parent.transform);
                inst.transform.localPosition = component.position;
                inst.transform.eulerAngles = component.rotation;
                component.realGameobject = inst;
                inst.transform.position = new Vector3(inst.transform.position.x, inst.transform.position.y, -1);
                
                string name = component.name;

                

                if (name == "body")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.1f - bodyNumber * 0.001f);
                    bodyNumber++;
                }
                if (name == "arm" || name == "leg")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.2f - armLegNumber * 0.001f);
                    armLegNumber++;
                }
                if (name == "hand" || name == "foot")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.3f - handFootNumber * 0.001f);
                    handFootNumber++;
                }
                if(name == "ear")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.4f - earNumber * 0.001f);
                    earNumber++;
                }
                if (name == "mouth")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.5f - moutNumber * 0.001f);
                    moutNumber++;
                }
                if(name == "nose")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.5f - noseNumber * 0.001f);
                    noseNumber++;
                }
                if(name == "eye")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.6f - eyeNumber * 0.001f);
                    eyeNumber++;
                }
                if (name == "hair")
                {
                    inst.transform.localPosition += new Vector3(0, 0, -0.7f - hairNumber * 0.001f);
                    hairNumber++;
                }
            } 

            //북씬 들어갔을때, 생성되는 신체 오브젝트들의 박스 콜라이더를 꺼준다.
            BoxCollider2D[] bcArray;
            bcArray = parent.GetComponentsInChildren<BoxCollider2D>();
            for(int j = 0; j < bcArray.Length; j++)
            {
                bcArray[j].enabled = false;
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
        }
        
    }



    public void HouseSceneLoad()
    {
        if(gameManager.fromPotScene == true)
        {
            gameManager.PotSceneLoad();
        }
        else
        {
            gameManager.HouseSceneLoad();
        }
        
    }    
}
