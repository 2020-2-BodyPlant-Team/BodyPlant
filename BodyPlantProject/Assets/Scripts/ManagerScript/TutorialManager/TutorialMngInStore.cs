using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TutorialMngInStore : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    public GameObject textPanel;
    public GameObject cat;
    public GameObject armSeedBtn;  
    public GameObject btnParent;
    public GameObject backBtn;
    bool nowTexting;
    public Text binText;
    public int textOrder;
    public List<string> turtorialTexts;
    public bool isSeedBtnClicked;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        nowTexting = false;
        isSeedBtnClicked = false;
        textOrder = 0;
        

        if(saveData.tutorialOrder != 1)
        {
            this.gameObject.SetActive(false);
        }

        else
        {
            for (int i = 0; i < turtorialTexts.Count; i++)
            {
                turtorialTexts[i] = turtorialTexts[i].Replace("\\n", "\n");
            }
            cat.transform.SetParent(textPanel.transform);
            StartCoroutine(LoadTextOneByOne(turtorialTexts[0], binText));  
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
            armSeedBtn.transform.SetParent(textPanel.transform);
            StartCoroutine(FadeInBtn(armSeedBtn));
        }

        if(textOrder == 9)
        {
            backBtn.transform.SetParent(textPanel.transform);
            armSeedBtn.transform.SetParent(btnParent.transform);
            StartCoroutine(FadeInBtn(backBtn));
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
                StartCoroutine(LoadTextOneByOne(turtorialTexts[i + 1], binText));
            }
        }

        while(textOrder >= 6 && textOrder < 9)
        {
            yield return null;
            if(isSeedBtnClicked)
            {
                StartCoroutine(LoadTextOneByOne(turtorialTexts[textOrder + 1], binText));
                break;
            }
        }
        
        
        
        textOrder++;
        
    }
    
    IEnumerator FadeOutSeedBtn()
    {
        int i = 10;
        while (i > 0)
        {
            i -= 1;
            float f = i / 10.0f;

            Color c = armSeedBtn.GetComponent<Image>().color;
            Color c0 = armSeedBtn.transform.GetChild(0).GetComponent<Image>().color;
            Color c1 = armSeedBtn.transform.GetChild(1).GetComponent<Image>().color;
            Color c3 = armSeedBtn.transform.GetChild(3).GetComponent<Image>().color;

            c.a = f;
            c0.a = f;
            c1.a = f;
            c3.a = f;

            armSeedBtn.GetComponent<Image>().color = c;
            armSeedBtn.transform.GetChild(0).GetComponent<Image>().color = c0;
            armSeedBtn.transform.GetChild(1).GetComponent<Image>().color = c1;
            armSeedBtn.transform.GetChild(3).GetComponent<Image>().color = c3;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeInBtn(GameObject button)
    {
        int i = 0;
        while (i < 10)
        {
            i += 1;
            float f = i / 10.0f;

            Color c = button.GetComponent<Image>().color;

            c.a = f;

            button.GetComponent<Image>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }
}

