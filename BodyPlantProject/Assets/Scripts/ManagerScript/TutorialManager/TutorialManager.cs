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
    bool nowTexting;
    public Text binText;
    int textOrder;
    public List<Text> turtorialTexts;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        nowTexting = false;
        textOrder = 0;

        if(saveData.tutorialOrder != 0)
        {
            this.gameObject.SetActive(false);
        }

        else
        {
            StartCoroutine(LoadTextOneByOne(turtorialTexts[0].text, binText));
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

        if(textOrder == 6)
        {
            StartCoroutine(FadeOut());
        }

        while(true)
        {
            yield return null;
            if(Input.GetMouseButtonDown(0))
            {
                break;
            }
        }

        for(int i = 0; i < 6; i++)
        {
            if(textOrder == i)
            {
                
                StartCoroutine(LoadTextOneByOne(turtorialTexts[i + 1].text, binText));
                
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
        cat.GetComponent<RectTransform>().anchoredPosition = new Vector2(-89, -442);
        storeBtn.transform.SetParent(textPanel.transform);
        StartCoroutine(FadeIn());
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
