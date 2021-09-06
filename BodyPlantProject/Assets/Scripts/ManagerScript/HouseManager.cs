using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    SoundManager soundManager;
    OptionManager optionManager;

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
    Vector2 optionUpPosChanged;
    Vector2 optionUpPosOrigin;

    public GameObject buttonBundle;
    Button[] buttonBundleArray;



    public Text coinText;

    public GameObject trainObject;
    public GameObject sofaObject;

    

    public GameObject panel;
    public void PanelLoad() //일하기 버튼 팝업 켜고 끄기
    {
        soundManager.ButtonEffectPlay();
        if (panel.activeSelf == false)
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
        soundManager.MineBGMPlay();
        soundManager.ButtonEffectPlay();
    }

    public void WorkHuntSceneLoad()
    {
        gameManager.WorkHuntSceneLoad();
        soundManager.HuntBGMPlay();
        soundManager.ButtonEffectPlay();
    }

    public void WorkFishingSceneLoad()
    {
        gameManager.WorkFishingSceneLoad();
        soundManager.FishBGMPlay();
        soundManager.ButtonEffectPlay();
    }
    public void PotSceneLoad()
    {
        gameManager.PotSceneLoad();
        soundManager.ButtonEffectPlay();
        if(saveData.tutorialOrder == 2)
        {
            saveData.tutorialOrder++;
            gameManager.Save();
        }
    }

    public void StoreSceneLoad()
    {
        gameManager.StoreSceneLoad();
        soundManager.ButtonEffectPlay();
        if(saveData.tutorialOrder == 0)
        {
            saveData.tutorialOrder++;
            gameManager.Save();
        }
    }

    public void ComposeSceneLoad()
    {
        gameManager.ComposeSceneLoad();
        soundManager.ButtonEffectPlay();
    }

    public void BookSceneLoad()
    {
        if(saveData.tutorialOrder == 5)
        {
            saveData.tutorialOrder++;
            gameManager.Save();
        }
        gameManager.BookSceneLoad();
        soundManager.ButtonEffectPlay();
    }


    bool nowFullScreen;
    bool nowCorRunning;
    public void FullScreenButton()
    {
        if (!nowFullScreen && !nowCorRunning)
        {
            soundManager.ButtonEffectPlay();
            StartCoroutine(FullScreenCoroutine());
        }
        
    }
    
    IEnumerator FullScreenCoroutine()
    {
        float time = 0;
        nowCorRunning = true;
        RectTransform optionRect = optionManager.optionButtonObject.GetComponent<RectTransform>();

        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
        for(int i = 0;i< buttonBundleArray.Length; i++)
        {
            buttonBundleArray[i].interactable = false;
        }
        while (time < 1)
        {
            upRect.anchoredPosition = Vector2.Lerp(Vector2.zero, upPos, time);
            downRect.anchoredPosition = Vector2.Lerp(Vector2.zero, downPos, time);
            rightRect.anchoredPosition = Vector2.Lerp(Vector2.zero, rightPos, time);
            leftRect.anchoredPosition = Vector2.Lerp(Vector2.zero, leftPos, time);
            optionRect.anchoredPosition = Vector2.Lerp(optionUpPosOrigin, optionUpPosChanged, time);
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
            soundManager.ButtonEffectPlay();
            StartCoroutine(FullScreenRemoveCoroutine());
        }
    }

    IEnumerator FullScreenRemoveCoroutine()
    {
        float time = 0;
        nowCorRunning = true;
        RectTransform optionRect = optionManager.optionButtonObject.GetComponent<RectTransform>();
        while (time < 1)
        {
            upRect.anchoredPosition = Vector2.Lerp(upPos, Vector2.zero,  time);
            downRect.anchoredPosition = Vector2.Lerp(downPos, Vector2.zero, time);
            rightRect.anchoredPosition = Vector2.Lerp(rightPos, Vector2.zero, time);
            leftRect.anchoredPosition = Vector2.Lerp(leftPos, Vector2.zero,  time);
            optionRect.anchoredPosition = Vector2.Lerp(optionUpPosChanged, optionUpPosOrigin, time);
            time += 2* Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < buttonBundleArray.Length; i++)
        {
            buttonBundleArray[i].interactable = true;
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
        soundManager = SoundManager.inst;
        optionManager = OptionManager.singleTon;

        coinText.text = saveData.coin.ToString();

        upPos = new Vector2(0, 600);
        leftPos = new Vector2(-700, 0);
        rightPos = new Vector2(800, 0);
        downPos = new Vector2(0, -900);
        optionUpPosChanged = new Vector2(423, 1500);
        optionUpPosOrigin = new Vector2(423, 840);

        buttonBundleArray = buttonBundle.GetComponentsInChildren<Button>();

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

        if (saveData.chairSelled)
        {
            sofaObject.SetActive(true);
        }
        else
        {
            sofaObject.SetActive(false);
        }

        if (saveData.trainSelled)
        {
            trainObject.SetActive(true);
        }
        else
        {
            trainObject.SetActive(false);
        }
    }



    void Update()
    {
        characterMover.PositionUpdate();
        characterMover.RotationUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            if (OptionManager.singleTon.optionOn)
            {
                return;
            }
            FullScreenRemove();
        }
    }

}
