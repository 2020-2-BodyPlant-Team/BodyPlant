using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TutorialMngInPot : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    public GameObject textPanel;
    public GameObject cat;
    public GameObject composeBtn;  
    public GameObject potBtn;
    public GameObject huntNutrient;
    public GameObject fishNutrient;
    public GameObject mineNutrient;
    public GameObject progressBar;
    public GameObject harvestBtn;
    public GameObject harvestCanvas;
    public GameObject backBtn;
    public GameObject parentObj;
    public GameObject flowerPots;
    public GameObject magnifiedUIParent;
    bool nowTexting;
    bool isTextPanelSetActived;
    public bool isHarvestBtnClicked;
    public int numberOfNutrientClicked;
    public bool isBackBtnClicked;
    public Text binText;
    FlowerPotManager flowerPotManager;
    public int textOrder;
    public List<Text> turtorialTexts;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        nowTexting = false;
        isTextPanelSetActived = false;
        isHarvestBtnClicked = false;
        numberOfNutrientClicked = 0;
        isBackBtnClicked = false;
        textOrder = 0;
        flowerPotManager = FindObjectOfType<FlowerPotManager>();

        Debug.Log(saveData.tutorialOrder);
        if(saveData.tutorialOrder != 3)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            isTextPanelSetActived = true;
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

            potBtn.transform.SetParent(parentObj.transform);
    //        potBtn.transform.localPosition = new Vector3(potBtn.transform.localPosition.x,
    //potBtn.transform.localPosition.y, -7f);
            StartCoroutine(FadeInPot(potBtn));
            Debug.Log(flowerPotManager.nowMagnified);

            while(true)
            {
                yield return null;
                if(flowerPotManager.nowMagnified)
                {
                    potBtn.transform.SetParent(flowerPots.transform);
                    FadeInCat();
                    break;
                }
            }
        }
        else if(textOrder == 4)
        {
            huntNutrient.transform.SetParent(parentObj.transform);
            StartCoroutine(FadeInObj(huntNutrient));

            while(true)
            {
                yield return null;
                huntNutrient.transform.GetChild(0).GetComponent<Button>().interactable = true;
                if(numberOfNutrientClicked == 3)
                {
                    huntNutrient.transform.SetParent(magnifiedUIParent.transform);
                    break;
                }
            }
        }
        else if(textOrder == 6)
        {
            FadeOutCat();

            progressBar.transform.SetParent(parentObj.transform);
            potBtn.transform.SetParent(parentObj.transform);

            harvestBtn.transform.SetParent(parentObj.transform);
            harvestCanvas.transform.SetParent(parentObj.transform);
            StartCoroutine(FadeInObj(progressBar));
            StartCoroutine(FadeInObj(progressBar.transform.GetChild(0).gameObject));
            StartCoroutine(FadeInPot(potBtn));

            while(true)
            {
                yield return null;
                if(isHarvestBtnClicked)
                {
                    harvestCanvas.transform.SetParent(magnifiedUIParent.transform);
                    progressBar.transform.SetParent(magnifiedUIParent.transform);
                    potBtn.transform.SetParent(flowerPots.transform);
                    backBtn.transform.SetParent(parentObj.transform);
                    harvestBtn.transform.SetParent(magnifiedUIParent.transform);
                    StartCoroutine(FadeInObj(backBtn));
                    while(true)
                    {
                        if(isBackBtnClicked)
                        {
                            backBtn.transform.SetParent(magnifiedUIParent.transform);
                            FadeInCat();
                            break;
                        }
                        yield return null;
                    }

                    break;
                }
            }
        }
        else if(textOrder == 8)
        {
            composeBtn.transform.SetParent(parentObj.transform);
            StartCoroutine(FadeOutOnlyCat());
        }
        
        //--------------------------------------------------------클릭하고 나서 나오는 행동들-------------------------------------------------------------

        for(int i = 0; i < 8; i++)
            {
                if(textOrder == i)
                {
                    StartCoroutine(LoadTextOneByOne(turtorialTexts[i + 1].text, binText));
                }
            }

        textOrder++;
        
        //---------------------------------------텍스트 순서 하나 올려주고 그 다음 텍스트 따르르르 시작하게 해줘-------------------------------------------
    }

    IEnumerator FadeInObj(GameObject obj)
    {
        int i = 0;
        while (i < 10)
        {
            i += 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<Image>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<Image>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeInPot(GameObject obj)
    {
        int i = 0;
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
        int i = 0;
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

    IEnumerator FadeOutObj(GameObject obj)
    {
        int i = 10;
        while (i > 0)
        {
            i -= 1;
            float f = i / 10.0f;

            Color c = obj.transform.GetComponent<Image>().color;
        
            c.a = f;
        
            obj.transform.GetComponent<Image>().color = c;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeOutPot(GameObject obj)
    {
        int i = 10;
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
        int i = 10;
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
        StartCoroutine(FadeInText(textPanel.transform.GetChild(0).gameObject));
        StartCoroutine(FadeInObj(textPanel.gameObject));
        StartCoroutine(FadeInObj(cat.transform.GetChild(0).gameObject));
        StartCoroutine(FadeInObj(cat.transform.GetChild(1).gameObject));
        StartCoroutine(FadeInObj(cat.transform.GetChild(2).gameObject));
        isTextPanelSetActived = true;
    }

    void FadeInOnlyCat()
    {
        StartCoroutine(FadeInObj(cat.transform.GetChild(0).gameObject));
        StartCoroutine(FadeInObj(cat.transform.GetChild(1).gameObject));
        StartCoroutine(FadeInObj(cat.transform.GetChild(2).gameObject));
    }

    void FadeOutCat()
    {
        Debug.Log("fadeoutcat");
        isTextPanelSetActived = false;
        StartCoroutine(FadeOutText(textPanel.transform.GetChild(0).gameObject));
        StartCoroutine(FadeOutObj(textPanel.gameObject));
        StartCoroutine(FadeOutObj(cat.transform.GetChild(0).gameObject));
        StartCoroutine(FadeOutObj(cat.transform.GetChild(1).gameObject));
        StartCoroutine(FadeOutObj(cat.transform.GetChild(2).gameObject));
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
        
        FadeInOnlyCat();
        StartCoroutine(FadeInObj(composeBtn));
    }
}
