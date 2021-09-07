using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TutorialMngInMine : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    public GameObject textPanel;
    public GameObject cat;
    public GameObject blackPanel;

    bool nowTexting;
    public bool isTextPanelSetActived;
    public Text binText;
    public int textOrder;
    public List<string> turtorialTexts;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        nowTexting = false;
        isTextPanelSetActived = false;
        textOrder = 0;

        if (saveData.mineTutorial)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < turtorialTexts.Count; i++)
            {
                turtorialTexts[i] = turtorialTexts[i].Replace("\\n", "\n");
            }
            StartCoroutine(LoadTextOneByOne(turtorialTexts[textOrder], binText));
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

            if (Input.GetMouseButtonDown(0))
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


        while (true) // 화면을 클릭하면 다음 텍스트가 나옴
        {
            yield return null;
            if (isTextPanelSetActived && Input.GetMouseButtonDown(0))
            {
                break;
            }
        }

        if (textOrder < 2)
        {
            StartCoroutine(LoadTextOneByOne(turtorialTexts[textOrder + 1], binText));
        }
        textOrder++;
        if (textOrder == 3)
        {
            saveData.mineTutorial = true;
            gameManager.Save();
            FadeOutCat();
        }

        //---------------------------------------텍스트 순서 하나 올려주고 그 다음 텍스트 따르르르 시작하게 해줘-------------------------------------------
    }

    public void OnDeerBash()
    {
        OptionManager.singleTon.OptionFade(true);
        FadeInCat();
        StartCoroutine(LoadTextOneByOne(turtorialTexts[textOrder], binText));
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
        if (textOrder == 3)
        {
            gameObject.SetActive(false);
            OptionManager.singleTon.OptionFade(false);
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
        StartCoroutine(FadeInObj(blackPanel.gameObject, 0.4f));
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
        StartCoroutine(FadeOutObj(blackPanel.gameObject));
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
        //composeBtn.transform.SetParent(parentObj.transform);
        //StartCoroutine(FadeInObj(composeBtn, 1f));
    }
}
