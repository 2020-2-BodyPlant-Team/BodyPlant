using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;

    [SerializeField]
    List<CharacterClass> characterList;
    public CharacterMover characterMover;

    public GameObject upParent;
    public GameObject downParent;
    public GameObject leftParent;
    public GameObject rightParent;

    RectTransform upRect;
    RectTransform downRect;
    RectTransform leftRect;
    RectTransform rightRect;

    Vector2 upPos;
    Vector2 leftPos;
    Vector2 rightPos;
    Vector2 downPos;

    public Text coinText;

    public void PotSceneLoad()
    {
        gameManager.PotSceneLoad();
    }

    public GameObject panel;
    public void PanelLoad() //일하기 버튼 팝업 켜고 끄기
    {
        if(panel.activeSelf == false)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void WorkMineSceneLoad()
    {
        gameManager.WorkMineSceneLoad();
    }

    public void WorkHuntSceneLoad()
    {
        gameManager.WorkHuntSceneLoad();
    }

    public void WorkFishingSceneLoad()
    {
        gameManager.WorkFishingSceneLoad();
    }

    public void StoreSceneLoad()
    {
        gameManager.StoreSceneLoad();
    }

    public void ComposeSceneLoad()
    {
        gameManager.ComposeSceneLoad();
    }

    public void BookSceneLoad()
    {
        gameManager.BookSceneLoad();
    }


    bool nowFullScreen;
    bool nowCorRunning;
    public void FullScreenButton()
    {
        if (!nowFullScreen && !nowCorRunning)
        {
            StartCoroutine(FullScreenCoroutine());
        }
        
    }
    
    IEnumerator FullScreenCoroutine()
    {
        float time = 0;
        nowCorRunning = true;
        while(time < 1)
        {
            upRect.anchoredPosition = Vector2.Lerp(Vector2.zero, upPos, time);
            downRect.anchoredPosition = Vector2.Lerp(Vector2.zero, downPos, time);
            rightRect.anchoredPosition = Vector2.Lerp(Vector2.zero, rightPos, time);
            leftRect.anchoredPosition = Vector2.Lerp(Vector2.zero, leftPos, time);
            time += 2* Time.deltaTime;
            yield return null;
        }
        nowCorRunning = false;
        nowFullScreen = true;
    }

    public void FullScreenRemove()
    {
        if (nowFullScreen && !nowCorRunning)
        {
            StartCoroutine(FullScreenRemoveCoroutine());
        }
    }

    IEnumerator FullScreenRemoveCoroutine()
    {
        float time = 0;
        nowCorRunning = true;
        while (time < 1)
        {
            upRect.anchoredPosition = Vector2.Lerp(upPos, Vector2.zero,  time);
            downRect.anchoredPosition = Vector2.Lerp(downPos, Vector2.zero, time);
            rightRect.anchoredPosition = Vector2.Lerp(rightPos, Vector2.zero, time);
            leftRect.anchoredPosition = Vector2.Lerp(leftPos, Vector2.zero,  time);
            time += 2* Time.deltaTime;
            yield return null;
        }
        nowCorRunning = false;
        nowFullScreen = false;
    }



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.characterList;

        coinText.text = saveData.coin.ToString();

        upPos = new Vector2(0, 200);
        leftPos = new Vector2(-350, 0);
        rightPos = new Vector2(400, 0);
        downPos = new Vector2(0, -325);

        for (int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i],i);
        }
        nowFullScreen = false;
        nowCorRunning = false;

        upRect = upParent.GetComponent<RectTransform>();
        downRect = downParent.GetComponent<RectTransform>();
        leftRect = leftParent.GetComponent<RectTransform>();
        rightRect = rightParent.GetComponent<RectTransform>();
    }

    void Update()
    {
        characterMover.PositionUpdate();
        characterMover.RotationUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            FullScreenRemove();
        }
    }

}
