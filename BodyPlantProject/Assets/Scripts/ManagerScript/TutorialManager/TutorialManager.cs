using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TutorialManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    public GameObject textPanel;
    public GameObject cat;
    public GameObject storeBtn;  
    public GameObject potBtn;
    public GameObject bookBtn;
    bool nowTexting;
    public Text binText;
    int textOrder;
    public List<Text> turtorialTexts;
    public List<Text> tutorialTextsInOrder2;
    public List<Text> tutorialTextsInOrder5;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        nowTexting = false;
        textOrder = 0;
        Debug.Log(saveData.tutorialOrder);
        if(saveData.tutorialOrder != 0 && saveData.tutorialOrder != 2 && saveData.tutorialOrder != 5)
        {
            this.gameObject.SetActive(false);
        }

        else
        {
            if(saveData.tutorialOrder == 0)
            {
                StartCoroutine(LoadTextOneByOne(turtorialTexts[0].text, binText));
            }
            else if(saveData.tutorialOrder == 2)
            {
                StartCoroutine(LoadTextOneByOne(tutorialTextsInOrder2[0].text, binText));
            }
            else if(saveData.tutorialOrder == 5)
            {
                StartCoroutine(LoadTextOneByOne(tutorialTextsInOrder5[0].text, binText));
            }
        }
        
    }

    public IEnumerator LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.1f, bool canClickSkip = true)
    {
        nowTexting = true;
        float miniTimer = 0f; 
        float currentTargetNumber = 0f; 
        int currentNumber = 0; 
        string displayedText = "";
        StringBuilder builder = new StringBuilder(displayedText);
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

        if(saveData.tutorialOrder == 0) // 튜토리얼 순서 0 번째일 때
        {
            if(textOrder == 6)
            {
                StartCoroutine(FadeOut());
            }
        }
        else if(saveData.tutorialOrder == 2) // 튜토리얼 순서 2 번째일 때
        {
            if(textOrder == 1)
            {
                StartCoroutine(FadeOut());
            }
        }
        else if(saveData.tutorialOrder == 5)
        {
            if(textOrder == 5)
            {
                StartCoroutine(FadeOut());
            }
        }
        

        while(true)
        {
            yield return null;
            if(Input.GetMouseButtonDown(0))
            {
                break;
            }
        }

        if(saveData.tutorialOrder == 0) // 튜토리얼 순서 0 번째일 때
        {
            for(int i = 0; i < 6; i++)
            {
                if(textOrder == i)
                {
                    StartCoroutine(LoadTextOneByOne(turtorialTexts[i + 1].text, binText));
                }
            }
        }
        else if(saveData.tutorialOrder == 2) // 튜토리얼 순서 2 번째일 때
        {
            for(int i = 0; i < 1; i++)
            {
                if(textOrder == i)
                {
                    Debug.Log(i + 1);
                    StartCoroutine(LoadTextOneByOne(tutorialTextsInOrder2[i + 1].text, binText));
                }
            }
        }
        else if(saveData.tutorialOrder == 5)
        {
            for(int i = 0; i < 5; i++)
            {
                if(textOrder == i)
                {
                    Debug.Log(i + 1);
                    StartCoroutine(LoadTextOneByOne(tutorialTextsInOrder5[i + 1].text, binText));
                }
            }
        }
        
        textOrder++;
    }

    IEnumerator FadeOut()
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

        if(saveData.tutorialOrder == 0) // 튜토리얼 순서 0 번째일 때
        {
            objPositionInOrder0();
        }
        else if(saveData.tutorialOrder == 2) // 튜토리얼 순서 2 번째일 때
        {
            objPositionInOrder2();
        }
        else if(saveData.tutorialOrder == 5)
        {
            objPositionInOrder5();
        }
        
        StartCoroutine(FadeIn());
    }

    void objPositionInOrder0()
    {
        cat.GetComponent<RectTransform>().anchoredPosition = new Vector2(-89, -442);
        storeBtn.transform.SetParent(textPanel.transform);
    }

    void objPositionInOrder2()
    {
        cat.GetComponent<RectTransform>().anchoredPosition = new Vector2(257, -437);
        potBtn.transform.SetParent(textPanel.transform);
    }

    void objPositionInOrder5()
    {
        cat.GetComponent<RectTransform>().anchoredPosition = new Vector2(-86, 746);
        cat.GetComponent<RectTransform>().localScale = new Vector2(-0.8f, 0.8f);
        bookBtn.transform.SetParent(textPanel.transform);
    }

    IEnumerator FadeIn()
    {
        int i = 0;
        while (i < 10)
        {
            i += 1;
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
    }
}
