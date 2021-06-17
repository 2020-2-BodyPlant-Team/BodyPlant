using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrologueManager : MonoBehaviour
{
    public GameObject[] littleButtonArray;
    public GameObject[] canvasArray;
    public GameObject[] firstSpriteArray;
    public GameObject[] secondSpriteArray;
    public GameObject[] thirdSpriteArray;
    public GameObject[] fourthSpriteArray;
    Vector3[] originPos;
    Vector3[] originEulerAngles;
    bool[] buttonActive;
    bool magnified =false;

    int nowLittleButtonIndex;
    int nowImageIndex;
    bool buttonClickable;

    
    // Start is called before the first frame update
    void Start()
    {
        nowLittleButtonIndex = 0;
        buttonActive = new bool[4];
        originPos = new Vector3[4];
        originEulerAngles = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            buttonActive[i] = false;
            originPos[i] = littleButtonArray[i].GetComponent<RectTransform>().anchoredPosition;
            originEulerAngles[i] = littleButtonArray[i].transform.localEulerAngles;
        }

        buttonClickable = false;
        StartCoroutine(LittleCircleCoroutine());


    }

    public void OnButtonClick(int index)
    {
        if(index != nowLittleButtonIndex || buttonClickable == false)
        {
            return;
        }

        StartCoroutine(ButtonMoveCoroutine());
    }


    IEnumerator LittleCircleCoroutine()
    {
        yield return new WaitForSeconds(1f);
        float timer = 0;
        Image image = littleButtonArray[nowLittleButtonIndex].transform.GetChild(0).gameObject.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        image.gameObject.SetActive(true);
        while (timer < 1)
        {
            timer += Time.deltaTime/2;
            image.color = new Color(1, 1, 1, timer);
            yield return null;
        }
        buttonActive[nowLittleButtonIndex] = true;
        buttonClickable = true;

        image.color = new Color(1, 1, 1, 1);
    }


    IEnumerator ButtonMoveCoroutine()
    {
        float timer = 0;
        nowImageIndex = 0;
        RectTransform rect = littleButtonArray[nowLittleButtonIndex].GetComponent<RectTransform>();
        for(int i = 0; i < 4; i++)
        {
            if (i == nowLittleButtonIndex)
                continue;
            littleButtonArray[i].SetActive(false);
        }
        littleButtonArray[nowLittleButtonIndex].transform.GetChild(0).gameObject.SetActive(false);
        while (timer < 1)
        {
            timer += Time.deltaTime / 2;
            littleButtonArray[nowLittleButtonIndex].transform.localEulerAngles = Vector3.Lerp(originEulerAngles[nowLittleButtonIndex], Vector3.zero, timer);
            rect.anchoredPosition = Vector3.Lerp(originPos[nowLittleButtonIndex], Vector3.zero, timer);
            littleButtonArray[nowLittleButtonIndex].transform.localScale = new Vector3(1, 1, 1) * (1+timer*2);
            yield return null;
        }
        littleButtonArray[nowLittleButtonIndex].SetActive(false);
        canvasArray[nowLittleButtonIndex].SetActive(true);

        littleButtonArray[nowLittleButtonIndex].transform.localEulerAngles = originEulerAngles[nowLittleButtonIndex];
        rect.anchoredPosition = originPos[nowLittleButtonIndex];
        littleButtonArray[nowLittleButtonIndex].transform.localScale = new Vector3(1, 1, 1);
        magnified = true;

    }

    bool returnCoroutineRunning = false;
    IEnumerator ButtonReturnCoroutine()
    {
        returnCoroutineRunning = true;
        magnified = false;
        float timer = 0;
        nowImageIndex = 1;
        buttonClickable = false;
        RectTransform rect = canvasArray[nowLittleButtonIndex].GetComponent<RectTransform>();



        littleButtonArray[nowLittleButtonIndex].transform.GetChild(0).gameObject.SetActive(false);
        while (timer < 1)
        {
            timer += Time.deltaTime / 2;
            canvasArray[nowLittleButtonIndex].transform.localEulerAngles = Vector3.Lerp( Vector3.zero, originEulerAngles[nowLittleButtonIndex], timer);
            rect.anchoredPosition = Vector3.Lerp( Vector3.zero, originPos[nowLittleButtonIndex], timer);
            canvasArray[nowLittleButtonIndex].transform.localScale = new Vector3(1, 1, 1) * (1- timer * 2.0f/3.0f);
            yield return null;
        }
        canvasArray[nowLittleButtonIndex].SetActive(false);

        canvasArray[nowLittleButtonIndex].transform.localEulerAngles = Vector3.Lerp( Vector3.zero, originEulerAngles[nowLittleButtonIndex], 1);
        rect.anchoredPosition = Vector3.Lerp( Vector3.zero, originPos[nowLittleButtonIndex], 1);
        canvasArray[nowLittleButtonIndex].transform.localScale = new Vector3(1, 1, 1) * 2.0f/3.0f;
        canvasArray[nowLittleButtonIndex].SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            littleButtonArray[i].SetActive(true);
        }
        nowLittleButtonIndex++;
        StartCoroutine(LittleCircleCoroutine());
        returnCoroutineRunning = false;
    }





    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (nowLittleButtonIndex == 0 && magnified)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (timer < 0.5f)
                {
                    return;
                }
                timer = 0;
                if (nowImageIndex >= firstSpriteArray.Length)
                {
                    if (!returnCoroutineRunning)
                    {
                        //littleButtonArray[nowLittleButtonIndex].GetComponent<Image>().sprite = firstSpriteArray[firstSpriteArray.Length - 1].GetComponent<Image>().sprite;
                        StartCoroutine(ButtonReturnCoroutine());
                    }
                        
                    
                }
                else
                {
                    firstSpriteArray[nowImageIndex].SetActive(true);
                    nowImageIndex++;
                }
            }
        }
    }
}
