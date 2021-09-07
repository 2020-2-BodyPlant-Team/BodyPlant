using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PrologueManager : MonoBehaviour
{
    SoundManager soundManager;
    GameManager gameManager;
    SaveDataClass saveData;
    public GameObject[] littleButtonArray;
    public GameObject[] canvasArray;
    public GameObject[] firstSpriteArray;
    public GameObject[] secondSpriteArray;
    public GameObject[] thirdSpriteArray;
    public GameObject[] fourthSpriteArray;
    public RectTransform[] spriteParentRectArray;
    public GameObject[,] spriteArray;
    public GameObject[] returnedObject;
    public RectTransform wholeParent;
    public Text fourthText;
    public Text textString;
    public int[] arrayLength;
    Vector3[] originPos;
    Vector3[] originEulerAngles;
    bool[] buttonActive;
    bool magnified =false;
    
    [SerializeField ]
    int nowLittleButtonIndex;
    [SerializeField]
    int nowImageIndex;
    bool buttonClickable;
    bool afterCameraOrigin;

    bool isInBook = true;
    [SerializeField]
    GameObject inbookCanvas;
    [SerializeField]
    GameObject outBookCanvas;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        soundManager = SoundManager.inst;
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

        spriteArray = new GameObject[4, 5];
        for (int i = 0; i < firstSpriteArray.Length; i++)
        {
            spriteArray[0, i] = firstSpriteArray[i];
        }
        for (int i = 0; i < secondSpriteArray.Length; i++)
        {
            spriteArray[1, i] = secondSpriteArray[i];
        }
        for (int i = 0; i < thirdSpriteArray.Length; i++)
        {
            spriteArray[2, i] = thirdSpriteArray[i];
        }
        for (int i = 0; i < fourthSpriteArray.Length; i++)
        {
            spriteArray[3, i] = fourthSpriteArray[i];
        }

        arrayLength = new int[4];
        arrayLength[0] = firstSpriteArray.Length;
        arrayLength[1] = secondSpriteArray.Length;
        arrayLength[2] = thirdSpriteArray.Length;
        arrayLength[3] = fourthSpriteArray.Length;


        buttonClickable = false;



    }

    bool fourthButtonFirstClicked = false;
    public void OnButtonClick(int index)
    {
        if(index != nowLittleButtonIndex || buttonClickable == false)
        {
            return;
        }
        if(index == 3)
        {
            if(fourthButtonFirstClicked == false)
            {
                fourthButtonFirstClicked = true;
                littleButtonArray[3].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(ButtonMoveCoroutine());
            }
        }
        else
        {
            StartCoroutine(ButtonMoveCoroutine());
        }
        
    }

    IEnumerator BookCameraWork()
    {
        float timer = 0;
        timer = 0;
        Vector3 origin = new Vector3(1080, 0, 0);
        Vector3 goal = Vector3.zero;
        while (timer < 1)
        {
            timer += Time.deltaTime * 0.6f;
            wholeParent.anchoredPosition = Vector2.Lerp(origin, goal, timer);
            yield return null;
        }
        wholeParent.anchoredPosition = goal;
        StartCoroutine(LittleCircleCoroutine());
    }


    IEnumerator LittleCircleCoroutine()
    {
        //yield return new WaitForSeconds(1f);
        float timer = 0;
        int child = 0;
        if (nowLittleButtonIndex == 3)
        {
            child = 3;
        }
        Image image = littleButtonArray[nowLittleButtonIndex].transform.GetChild(child).gameObject.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        image.gameObject.SetActive(true);
        while (timer < 1)
        {
            timer += Time.deltaTime*2f;
            image.color = new Color(1, 1, 1, timer);
            yield return null;
        }
        buttonActive[nowLittleButtonIndex] = true;
        buttonClickable = true;

        image.color = new Color(1, 1, 1, 1);
    }


    IEnumerator ButtonMoveCoroutine()
    {
        soundManager.OpenPaperEffectPlay();
        if (nowLittleButtonIndex == 3)
        {
            littleButtonArray[nowLittleButtonIndex].transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            littleButtonArray[nowLittleButtonIndex].GetComponent<Image>().raycastTarget = false;
        }
        float timer = 0;
        nowImageIndex = 1;
        RectTransform rect = littleButtonArray[nowLittleButtonIndex].GetComponent<RectTransform>();
        for(int i = 0; i < 4; i++)
        {
            if (i == nowLittleButtonIndex)
                continue;
            littleButtonArray[i].SetActive(false);
            returnedObject[i].SetActive(false);
        }
        int child = 0;
        if(nowLittleButtonIndex == 3)
        {
            child = 3;
            littleButtonArray[nowLittleButtonIndex].transform.GetChild(2).gameObject.SetActive(true);
        }
        littleButtonArray[nowLittleButtonIndex].transform.GetChild(child).gameObject.SetActive(false);
        int time = 2;
        if(nowLittleButtonIndex == 0)
        {
            time = 1;
        }
        while (timer < 1)
        {
            timer += Time.deltaTime;
            littleButtonArray[nowLittleButtonIndex].transform.localEulerAngles = Vector3.Lerp(originEulerAngles[nowLittleButtonIndex], Vector3.zero, timer);
            if(nowLittleButtonIndex == 0 || nowLittleButtonIndex==3)
            {
                rect.anchoredPosition = Vector3.Lerp(originPos[nowLittleButtonIndex], Vector3.zero, timer);
            }
            else if(nowLittleButtonIndex == 1)
            {
                rect.anchoredPosition = Vector3.Lerp(originPos[nowLittleButtonIndex], new Vector3(-500,0,0), timer);
            }
            else
            {
                rect.anchoredPosition = Vector3.Lerp(originPos[nowLittleButtonIndex], new Vector3(500, 0, 0), timer);
            }
            
            littleButtonArray[nowLittleButtonIndex].transform.localScale = new Vector3(1, 1, 1) * (1+timer*time);
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
        soundManager.OpenPaperEffectPlay();
        returnCoroutineRunning = true;
        magnified = false;
        float timer = 0;
        nowImageIndex = 1;
        buttonClickable = false;
        RectTransform rect = canvasArray[nowLittleButtonIndex].GetComponent<RectTransform>();
        Vector3 origin = rect.anchoredPosition;
        Vector3 originScale = rect.localScale;


        //littleButtonArray[nowLittleButtonIndex].transform.GetChild(0).gameObject.SetActive(false);
        littleButtonArray[nowLittleButtonIndex].SetActive(false);
        Vector3 goalScale;
        if(nowLittleButtonIndex == 0)
        {
            goalScale = Vector3.one * 1f / 2f;
        }
        else
        {
            goalScale = Vector3.one * 1f / 3f;
        }
        while (timer < 1)
        {
            timer += Time.deltaTime;
            canvasArray[nowLittleButtonIndex].transform.localEulerAngles = Vector3.Lerp(Vector3.zero, originEulerAngles[nowLittleButtonIndex], timer);
            rect.anchoredPosition = Vector3.Lerp( origin, originPos[nowLittleButtonIndex], timer);
            //canvasArray[nowLittleButtonIndex].transform.localScale = originScale * (1 - timer * 2.0f / 3.0f);
            canvasArray[nowLittleButtonIndex].transform.localScale = Vector3.Lerp(originScale, goalScale, timer);
            //if(nowLittleButtonIndex == 3)
            //{
            //    fourthText.fontSize = (int)(49 / (timer*2 + 1));
            //}
            yield return null;
        }
        returnedObject[nowLittleButtonIndex].SetActive(true);
        canvasArray[nowLittleButtonIndex].SetActive(false);

        canvasArray[nowLittleButtonIndex].transform.localEulerAngles = originEulerAngles[nowLittleButtonIndex];
        canvasArray[nowLittleButtonIndex].transform.localScale = goalScale;
        canvasArray[nowLittleButtonIndex].SetActive(false);
        for (int i = 0; i < nowLittleButtonIndex+1; i++)
        {
            returnedObject[i].SetActive(true);
        }
        for (int i = nowLittleButtonIndex+1; i < 4; i++)
        {
            littleButtonArray[i].SetActive(true);
        }
        nowLittleButtonIndex++;
        if(nowLittleButtonIndex < 4)
        {
            StartCoroutine(LittleCircleCoroutine());
            littleButtonArray[nowLittleButtonIndex - 1].GetComponent<Image>().raycastTarget = false;
            if(nowLittleButtonIndex == 3)
            {
                littleButtonArray[nowLittleButtonIndex].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                littleButtonArray[nowLittleButtonIndex].GetComponent<Image>().raycastTarget = true;
            }
            
        }

        if(nowLittleButtonIndex == 4)
        {
            yield return new WaitForSeconds(1f);
            saveData.watchedPrologue = true;
            gameManager.Save();
            gameManager.StartSceneLoad();
        }
        
        returnCoroutineRunning = false;
    }

    bool cameraMoving = false;
    bool returnAfterCamera = false;
    IEnumerator CameraWork()
    {
        cameraMoving =true;
        afterCameraOrigin = false;
        float timer = 0;
        timer = 0;
        Vector3 origin = new Vector3(-500, 0, 0);
        Vector3 goal = new Vector3(500, 0, 0);
        if(nowLittleButtonIndex == 2)
        {
            origin = new Vector3(500, 0, 0);
            goal = new Vector3(-500, 0, 0);
        }
        while (timer < 1)
        {
            timer += Time.deltaTime * 0.3f;
            spriteParentRectArray[nowLittleButtonIndex].anchoredPosition = Vector2.Lerp(origin, goal, timer);
            yield return null;
        }
        spriteParentRectArray[nowLittleButtonIndex].anchoredPosition = goal;


        if (nowLittleButtonIndex == 1)
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (nowImageIndex >= 4 && nowLittleButtonIndex == 1)
                    {
                        break;
                    }

                }
                yield return null;
            }

            origin = new Vector3(500, 0, 0);
            goal = new Vector3(-500, 0, 0);

            timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime * 0.3f;
                spriteParentRectArray[nowLittleButtonIndex].anchoredPosition = Vector2.Lerp(origin, goal, timer);
                yield return null;
            }
            spriteParentRectArray[nowLittleButtonIndex].anchoredPosition = goal;
            if (nowLittleButtonIndex == 1 && nowImageIndex == 4)
            {
                soundManager.DrawingEffectPlay();
                spriteArray[nowLittleButtonIndex, nowImageIndex].SetActive(true);
            }
        }
        


        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (nowImageIndex >= 4 && nowLittleButtonIndex == 1)
                {
                    break;
                }
                if (nowLittleButtonIndex == 2)
                {
                    break;
                }

            }
            yield return null;
        }
        timer = 0;
        if(nowLittleButtonIndex == 1)
        {
            origin = new Vector3(-500, 0, 0);
        }
        else
        {
            origin = new Vector3(-500, 0, 0);
        }
        
        goal = Vector3.zero;
        while (timer < 1)
        {
            timer += Time.deltaTime * 0.6f;
            spriteParentRectArray[nowLittleButtonIndex].anchoredPosition = Vector2.Lerp(origin, goal, timer);
            spriteParentRectArray[nowLittleButtonIndex].localScale = Vector3.Lerp(Vector3.one, Vector3.one / 2f, timer);
            yield return null;
        }

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (nowImageIndex >= 4 && nowLittleButtonIndex == 1)
                {
                    break;
                }
                if (nowLittleButtonIndex == 2)
                {
                    break;
                }

            }
            yield return null;
        }
        nowImageIndex++;

        afterCameraOrigin = true;
        cameraMoving = false;
        if(returnAfterCamera == true)
        {
            StartCoroutine(ButtonReturnCoroutine());
        }
    }
    bool nowTexting = false;
    public IEnumerator LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.05f, bool canClickSkip = true)
    {
        nowTexting = true;
        float miniTimer = 0f; //타이머
        float currentTargetNumber = 0f; // 해당 Time에 출력을 목표로 하는 최소 글자 수
        int currentNumber = 0; // 해당 Time에 출력중인 글자 수
        string displayedText = "";
        StringBuilder builder = new StringBuilder(displayedText);
        while (currentTargetNumber < inputTextString.Length)
        {
            while (currentNumber < currentTargetNumber)
            { // 목표 글자수까지 출력
                //displayedText += inputTextString.Substring(currentNumber,1);
                builder.Append(inputTextString.Substring(currentNumber, 1));
                currentNumber++;
            }
            //inputTextUI.text = displayedText;
            inputTextUI.text = builder.ToString();
            yield return null;
            miniTimer += Time.deltaTime;
            currentTargetNumber = miniTimer / eachTime;
        }
        while (currentNumber < inputTextString.Length)
        { // 목표 글자수까지 출력
            builder.Append(inputTextString.Substring(currentNumber, 1));
            currentNumber++;
        }
        inputTextUI.text = builder.ToString();
        yield return null;
        nowTexting = false;
        if (returnAfterCamera)
        {
            StartCoroutine(ButtonReturnCoroutine());
        }
    }


    float time = 0;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (isInBook)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (time < 1f)
                {
                    return;
                }
                outBookCanvas.SetActive(false);
                inbookCanvas.SetActive(true);
                isInBook = false;
                soundManager.BookEffectPlay();
                StartCoroutine(BookCameraWork());
            }
            

        }
        if (magnified)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (time < 0.5f)
                {
                    return;
                }
                time = 0;
                if((nowLittleButtonIndex == 1||  nowLittleButtonIndex ==2 )&& nowImageIndex == 1)
                {
                    StartCoroutine(CameraWork());
                }
                if (nowImageIndex >= arrayLength[nowLittleButtonIndex])
                {
                    if (!returnCoroutineRunning)
                    {
                        //littleButtonArray[nowLittleButtonIndex].GetComponent<Image>().sprite = firstSpriteArray[firstSpriteArray.Length - 1].GetComponent<Image>().sprite;
                        if (cameraMoving || nowTexting)
                        {
                            //returnAfterCamera = true;
                        }
                        else
                        {
                            StartCoroutine(ButtonReturnCoroutine());
                        }
                        
                    }
                        
                    
                }
                else
                {
                    if(nowLittleButtonIndex == 1 && nowImageIndex == 4 && !afterCameraOrigin)
                    {
                        return;
                    }
                    spriteArray[nowLittleButtonIndex,nowImageIndex].SetActive(true);
                    if(nowLittleButtonIndex == 0)
                    {
                        soundManager.DrawingEffectPlay();
                    }
                    if(nowLittleButtonIndex == 1)
                    {
                        if(nowImageIndex == 1)
                        {
                            soundManager.StepEffectPlay();
                        }
                        if(nowImageIndex == 2)
                        {
                            soundManager.CarEffectPlay();
                        }
                        if(nowImageIndex == 3)
                        {
                            soundManager.CrowdEffectPlay();
                        }
                    }
                    if (nowLittleButtonIndex == 2)
                    {
                        StartCoroutine(MyaoCor());
                    }
                    if (nowLittleButtonIndex == 3 && !nowTexting)
                    {
                        soundManager.DrawingEffectPlay();
                        StartCoroutine(LoadTextOneByOne(textString.text, fourthText,0.1f));
                    }
                    nowImageIndex++;
                }
            }
        }
    }

    IEnumerator MyaoCor()
    {
        soundManager.CrowdEffectPlay();
        yield return new WaitForSeconds(2f);
        soundManager.StoreEffectPlay();
        yield return new WaitForSeconds(2f);
        soundManager.MyaoEffectPlay();
    }
}
