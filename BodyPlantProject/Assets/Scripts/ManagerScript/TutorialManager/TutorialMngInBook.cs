using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TutorialMngInBook : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    BookManager bookManager;
    public GameObject textPanel;
    public GameObject tutorialPanel;
    public GameObject cat;
    public GameObject content;
    public GameObject viewPort;
    public GameObject attatchBtn;
    public GameObject backBtn;
    public GameObject backBtnToHouse;
    public GameObject parentObj;
    public GameObject diaryPageParent;
    bool nowTexting;
    public bool isTextPanelSetActived;
    public bool isBackBtnClicked;
    public bool isBackToHouseBtnClicked;
    public bool isPlantBtnClicked;
    public bool isStickerAttatched;
    public Text binText;
    public int textOrder;
    public List<Text> turtorialTexts;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        bookManager = FindObjectOfType<BookManager>();
        nowTexting = false;
        isTextPanelSetActived = false;
        isBackBtnClicked = false;
        isPlantBtnClicked = false;
        isStickerAttatched = false;
        isBackToHouseBtnClicked = false;
        textOrder = 0;

        //backBtn = diaryPageParent.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;

        Debug.Log("첫 디버그");
        if(saveData.tutorialOrder != 6)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("entered else");
            StartCoroutine(LoadTextOneByOne(turtorialTexts[0].text, binText));  
        }
        
    }

    public IEnumerator LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.1f, bool canClickSkip = true)
    {
        isTextPanelSetActived = true;
        nowTexting = true;
        float miniTimer = 0f; 
        float currentTargetNumber = 0f; 
        int currentNumber = 0; 
        string displayedText = "";
        StringBuilder builder = new StringBuilder(displayedText);
        //backBtn = diaryPageParent.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;  

        while (currentTargetNumber < inputTextString.Length)
        {
            
            while (currentNumber < currentTargetNumber)
            { 
                //displayedText += inputTextString.Substring(currentNumber,1);
                builder.Append(inputTextString.Substring(currentNumber, 1));
                currentNumber++;
            }
            //inputTextUI.text = displayedText;
            inputTextUI.text = builder.ToString();
            yield return null;

            if(Input.GetMouseButtonDown(0))
            {
                break;
            }

            miniTimer += Time.deltaTime;
            currentTargetNumber = miniTimer / eachTime;
        }
        while (currentNumber < inputTextString.Length)
        {
            builder.Append(inputTextString.Substring(currentNumber, 1));
            currentNumber++;
        }
        inputTextUI.text = builder.ToString();
        yield return null;
        nowTexting = false;

        //---------------------------------------------------------텍스트 따르르르 쳐지는 부분--------------------------------------------------------------

        while(true) // 화면을 클릭하면 다음 텍스트가 나옴
        {
            yield return null;
            if(isTextPanelSetActived && Input.GetMouseButtonDown(0))
            {
                Debug.Log("clicked");
                break;
            }
        }

        //---------------------------------------------------텍스트가 다 쳐진 후 클릭을 해야 다음게 나온다.--------------------------------------------------

        if(textOrder == 0)
        {
            FadeOutCat();

            content.transform.SetParent(parentObj.transform);
            StartCoroutine(FadeInObj(content.transform.GetChild(0).GetChild(0).gameObject, 1f));
            StartCoroutine(FadeInObj(content.transform.GetChild(0).GetChild(1).gameObject, 1f));

            List<GameObject> objList = new List<GameObject>();
            Transform bodyPlant = content.transform.GetChild(0).GetChild(2);
            for(int i = 0; i < bodyPlant.childCount; i++)
            {
                if(bodyPlant.GetChild(i).childCount == 0)
                {
                    objList.Add(bodyPlant.GetChild(i).gameObject);
                }
                else
                {
                    if(bodyPlant.GetChild(i).gameObject.name == "hair(Clone)")
                    {
                        objList.Add(bodyPlant.GetChild(i).gameObject);
                        break;
                    }
                    for(int j = 0; j < bodyPlant.GetChild(i).childCount; j++)
                    {
                        objList.Add(bodyPlant.GetChild(i).GetChild(j).gameObject);
                    }
                }  
            }

            for(int i = 0; i < objList.Count; i++)
            {
                StartCoroutine(FadeInSpriteRenderer(objList[i]));
            }

            while(true)
            {
                yield return null;
                if(isPlantBtnClicked)
                {
                    content.transform.SetParent(viewPort.transform);
                    break;
                }
            }

            FadeInCat();
        }

        else if(textOrder == 2)
        {
            FadeOutCat();
            FadeInObj(bookManager.lovenessList[0].gameObject, 1f);
            parentObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-75f, 0f);
            bookManager.lovenessList[0].transform.SetParent(parentObj.transform);
            bookManager.lovenessList[0].transform.GetChild(1).gameObject.SetActive(true);

            while(true)
            {
                yield return null;
                if(!isTextPanelSetActived && Input.GetMouseButton(0))
                {
                    bookManager.lovenessList[0].transform.SetParent(bookManager.diaryList[0].transform.GetChild(1));
                    bookManager.lovenessList[0].transform.SetSiblingIndex(3);
                    bookManager.lovenessList[0].transform.GetChild(1).gameObject.SetActive(false);
                    break;
                }
            }

            FadeInCat();
        }

        else if(textOrder == 7)
        {
            FadeOutCat();
            StartCoroutine(FadeOutObj(tutorialPanel, 0.45f));

            bookManager.lovenessList[0].transform.SetParent(parentObj.transform);
            bookManager.totalList[0].loveNess = 100;

            while(true)
            {
                yield return null;
                if(isStickerAttatched)
                {
                    StartCoroutine(FadeInObj(tutorialPanel, 0.45f));
                    break;
                }
            }

            FadeInCat();
        }

        else if(textOrder == 10)
        {
            StartCoroutine(FadeInObj(backBtn, 1f));
            backBtn.transform.SetParent(parentObj.transform);

            while(true)
            {
                yield return null;
                if(isBackBtnClicked)
                {
                    Debug.Log("here");
                    backBtn.SetActive(false);
                    break;
                }
            }
        }

        else if(textOrder == 11)
        {
            StartCoroutine(FadeInObj(backBtnToHouse, 1f));
            backBtnToHouse.transform.SetParent(parentObj.transform);

            while(isBackToHouseBtnClicked)
            {
                yield return null;
            }
        }
        
        //--------------------------------------------------------클릭하고 나서 나오는 행동들-------------------------------------------------------------

        for(int i = 0; i < 11; i++)
            {
                if(textOrder == i)
                {
                    StartCoroutine(LoadTextOneByOne(turtorialTexts[i + 1].text, binText));
                }
            }

        textOrder++;
        
        //---------------------------------------텍스트 순서 하나 올려주고 그 다음 텍스트 따르르르 시작하게 해줘-------------------------------------------
    }

    IEnumerator FadeInObj(GameObject obj, float limit)
    {
        float i = 0;
        while (i < limit * 10)
        {
            i += 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<Image>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<Image>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeInSpriteRenderer(GameObject obj)
    {
        float i = 0;
        while (i < 10)
        {
            i += 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<SpriteRenderer>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<SpriteRenderer>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeInText(GameObject obj)
    {
        float i = 0;
        while (i < 10)
        {
            i += 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<Text>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<Text>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeOutObj(GameObject obj, float limit)
    {
        float i = limit * 10;
        while (i > 0)
        {
            i -= 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<Image>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<Image>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
        Debug.Log("fadeOut" + obj);
        if(obj == cat.transform.GetChild(2).gameObject)
        {
            isTextPanelSetActived = false;
            textPanel.SetActive(false);
        }
    }

    IEnumerator FadeOutSpriteRenderer(GameObject obj)
    {
        float i = 10;
        while (i > 0)
        {
            i -= 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<SpriteRenderer>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<SpriteRenderer>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeOutText(GameObject obj)
    {
        float i = 10;
        while (i > 0)
        {
            i -= 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<Text>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<Text>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    void FadeInCat()
    {
        textPanel.SetActive(true);
        StartCoroutine(FadeInText(textPanel.transform.GetChild(0).gameObject));
        StartCoroutine(FadeInObj(textPanel.gameObject, 0.4f));
        StartCoroutine(FadeInObj(cat.transform.GetChild(0).gameObject, 1f));
        StartCoroutine(FadeInObj(cat.transform.GetChild(1).gameObject, 1f));
        StartCoroutine(FadeInObj(cat.transform.GetChild(2).gameObject, 1f));
        isTextPanelSetActived = true;
    }

    void FadeInOnlyCat()
    {
        StartCoroutine(FadeInObj(cat.transform.GetChild(0).gameObject, 1f));
        StartCoroutine(FadeInObj(cat.transform.GetChild(1).gameObject, 1f));
        StartCoroutine(FadeInObj(cat.transform.GetChild(2).gameObject, 1f));
    }

    void FadeOutCat()
    {
        //isTextPanelSetActived = false;
        StartCoroutine(FadeOutText(textPanel.transform.GetChild(0).gameObject));
        StartCoroutine(FadeOutObj(textPanel.gameObject, 0.4f));
        StartCoroutine(FadeOutObj(cat.transform.GetChild(0).gameObject, 1f));
        StartCoroutine(FadeOutObj(cat.transform.GetChild(1).gameObject, 1f));
        StartCoroutine(FadeOutObj(cat.transform.GetChild(2).gameObject, 1f));
    }

    IEnumerator FadeOutOnlyCat()
    {
        int i = 10;
        while (i > 0)
        {
            i -= 1;
            float f = i / 10.0f;

            Color c0 = cat.transform.GetChild(0).GetComponent<Image>().color;
            Color c1 = cat.transform.GetChild(1).GetComponent<Image>().color;
            Color c2 = cat.transform.GetChild(2).GetComponent<Image>().color;

            c0.a = f;
            c1.a = f;
            c2.a = f;

            cat.transform.GetChild(0).GetComponent<Image>().color = c0;
            cat.transform.GetChild(1).GetComponent<Image>().color = c1;
            cat.transform.GetChild(2).GetComponent<Image>().color = c2;

            yield return new WaitForSeconds(0.02f);
        }
        
        cat.GetComponent<RectTransform>().localScale = new Vector3(-0.8f, 0.8f, 1f);
        cat.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 710);
        
        FadeInOnlyCat();
    }
}
