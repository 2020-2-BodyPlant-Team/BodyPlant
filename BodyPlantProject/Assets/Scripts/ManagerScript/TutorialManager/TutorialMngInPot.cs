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
    public GameObject potBtnAnother;
    public GameObject potBtnTheOther;
    public GameObject huntNutrient;
    public GameObject progressBar;
    public GameObject harvestBtn;
    public GameObject harvestCanvas;
    public GameObject backBtn;
    public GameObject parentObj;
    public GameObject flowerPots;
    public GameObject magnifiedUIParent;
    bool nowTexting;
    public bool isTextPanelSetActived;
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
            potBtn.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            potBtnAnother.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            potBtnTheOther.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
            potBtn.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    //        potBtn.transform.localPosition = new Vector3(potBtn.transform.localPosition.x,
    //potBtn.transform.localPosition.y, -7f);
            Transform potBtnTransform = potBtn.GetComponent<Transform>();
            potBtnTransform.position = new Vector3(potBtnTransform.position.x, potBtnTransform.position.y, potBtnTransform.position.z * -0.5f);
            StartCoroutine(FadeInPot(potBtn));
            Debug.Log(flowerPotManager.nowMagnified);

            while(true)
            {
                yield return null;
                if(flowerPotManager.nowMagnified)
                {
                    potBtnTransform.position = new Vector3(potBtnTransform.position.x, potBtnTransform.position.y, potBtnTransform.position.z / -0.5f);
                    potBtn.transform.SetParent(flowerPots.transform);
                    FadeInCat();
                    break;
                }
            }
        }
        else if(textOrder == 4)
        {
            huntNutrient.transform.SetParent(parentObj.transform);
            StartCoroutine(FadeInObj(huntNutrient, 1f));

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
            Transform potBtnTransform = potBtn.GetComponent<Transform>();
            potBtnTransform.position = new Vector3(potBtnTransform.position.x, potBtnTransform.position.y, potBtnTransform.position.z * -0.5f);

            harvestBtn.transform.SetParent(parentObj.transform);
            RectTransform harvestTransform = harvestBtn.GetComponent<RectTransform>();
            Debug.Log("adfas");
            harvestTransform.localPosition = new Vector3(harvestTransform.localPosition.x, harvestTransform.localPosition.y, -479f);
            harvestCanvas.transform.SetParent(parentObj.transform);
            StartCoroutine(FadeInObj(progressBar, 1f));
            StartCoroutine(FadeInObj(progressBar.transform.GetChild(0).gameObject, 1f));
            StartCoroutine(FadeInPot(potBtn));

            while(true)
            {
                yield return null;
                if(isHarvestBtnClicked)
                {
                    harvestCanvas.transform.SetParent(magnifiedUIParent.transform);
                    progressBar.transform.SetParent(magnifiedUIParent.transform);

                    potBtnTransform.position = new Vector3(potBtnTransform.position.x, potBtnTransform.position.y, potBtnTransform.position.z / -0.5f);
                    potBtn.transform.SetParent(flowerPots.transform);
                    backBtn.transform.SetParent(parentObj.transform);
                    harvestBtn.transform.SetParent(magnifiedUIParent.transform);
                    StartCoroutine(FadeInObj(backBtn, 1f));
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
        else if(textOrder == 7)
        {
            //composeBtn.transform.SetParent(parentObj.transform);
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

    IEnumerator FadeInPot(GameObject obj)
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

    IEnumerator FadeOutObj(GameObject obj)
    {
        float i = 10;
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
        
        cat.GetComponent<RectTransform>().localScale = new Vector3(-0.8f, 0.8f, 1f);
        cat.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 710);
        
        FadeInOnlyCat();
        composeBtn.transform.SetParent(parentObj.transform);
        StartCoroutine(FadeInObj(composeBtn, 1f));
    }
}
