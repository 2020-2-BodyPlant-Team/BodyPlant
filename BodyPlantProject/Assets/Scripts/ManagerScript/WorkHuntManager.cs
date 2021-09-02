using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkHuntManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    public List<CharacterClass> characterList;
    public GiveCoin coinManager;
    public WorkCharacterManager workCharacterManager;
    public TutorialMngInHunt tutorialManager;
    SoundManager soundManager;

    float waitSec;
    public GameObject sideDeer;
    Animation sideAni;
    public GameObject tearOne;
    Animation tearOneAni;
    public GameObject tearTwo;
    Animation tearTwoAni;
    public GameObject rightHorn;
    Animation rightHornAni;
    public GameObject leftHorn;
    Animation leftHornAni;

    public float limitTime = 2f;
    public GameObject frontDeer;
    Animator fdAnimator;
    public bool isFront = false;
    public CharacterMover characterMover;
    public GameObject[] parentObjectArray;
    public GameObject deerObject;
    public GameObject bringButton;
    float deerMaxX = 8;
    float deerMinX = -8;
    public Text huntElementText;
    public bool nowTutorial;
    public bool tutorialDeerOut;



    GameObject touchedObject;
    RaycastHit2D hit;
    public Camera cam;
    int count;

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }

    public void BringBtnOnClick()
    {
        if (nowTutorial)
        {
            saveData.tutorialOrder++;
            gameManager.Save();
            OptionManager.singleTon.OptionFade(false);
        }
        gameManager.SecretRoomSceneLoad();
    }

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.huntCharacterList;
        soundManager = SoundManager.inst;
        tutorialDeerOut = false;

        if (characterList.Count == 3 || saveData.characterList.Count == 0)
        {
            bringButton.SetActive(false);
        }

        sideAni = sideDeer.GetComponent<Animation>();
        fdAnimator = frontDeer.GetComponent<Animator>();
        tearOneAni = tearOne.GetComponent<Animation>();
        tearTwoAni = tearTwo.GetComponent<Animation>();
        rightHornAni = rightHorn.GetComponent<Animation>();
        leftHornAni = leftHorn.GetComponent<Animation>();

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;


        for (int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i], i);
            characterList[i].realGameobject.transform.SetParent(parentObjectArray[i].transform);
            characterList[i].realGameobject.transform.localPosition = Vector3.zero;
            characterList[i].realGameobject.transform.localScale = new Vector3(1, 1, 1);
        }
        coinManager.SetCharacterList(characterList, 0);

        huntElementText.text = saveData.huntElement.ToString();

        if (!nowTutorial)
        {
            StartCoroutine("DeerOut");
        }

        StartCoroutine(DeerMove());

        workCharacterManager.SetCharacterList(characterList);

        if(!soundManager.EffectPlaying())
            soundManager.GrassEffectPlay();
    }

    IEnumerator DeerOut()
    {
        waitSec = Random.Range(3.0f, 5.0f);
        yield return new WaitForSeconds(waitSec);
        sideAni.Play("sideDeerMove");
        yield return new WaitForSeconds(2);
        if (tutorialDeerOut)
        {
            tutorialManager.OnDeerBash();
        }
        isFront = true;
        frontDeer.SetActive(true);
        soundManager.DeeroutEffectPlay();
    }
    

    IEnumerator DeerMove()
    {
        float velocity = -0.05f;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            deerObject.transform.position = deerObject.transform.position + new Vector3(velocity, 0, 0);
            if(deerObject.transform.position.x > deerMaxX)
            {
                velocity *= -1;
                deerObject.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if(deerObject.transform.position.x < deerMinX)
            {
                velocity *= -1;
                deerObject.transform.eulerAngles = new Vector3(0, -180, 0);
            }
        }
    }

    public void DeerOnTutorial()
    {
        StartCoroutine("DeerOut");
        tutorialDeerOut = true;
    }

    public void TutorialEnd()
    {
        StartCoroutine("DeerOut");
    }
    
    void Update()
    {
        if(isFront == true)
        {
            if (!tutorialDeerOut)
            {
                limitTime -= Time.deltaTime;
            }

            if(limitTime < 0)
            {
                isFront = false;
                limitTime = 2f;
                Debug.Log("타이머 끝");

                if (isFront == false)
                {
                    if (count >= 10)
                    {
                        //보조성분 획득할 곳
                        saveData.huntElement++;
                        gameManager.Save();
                        huntElementText.text = saveData.huntElement.ToString();
                        Debug.Log("힘겨루기 승리");
                        StartCoroutine("DeerTear");
                        count = 0;
                        soundManager.SuccedEffectPlay();
                        if (nowTutorial)
                        {
                            tutorialManager.OnDeerCaught(true);
                        }
                    }
                    else
                    {
                        Debug.Log("힘겨루기 패배");
                        frontDeer.SetActive(false);
                        count = 0;
                        soundManager.DeerfailEffectPlay();
                        if (nowTutorial)
                        {
                            tutorialManager.OnDeerCaught(false);
                        }
                    }
                    if (!nowTutorial)
                    {
                        StartCoroutine("DeerOut");
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (OptionManager.singleTon.optionOn)
            {
                return;
            }
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                touchedObject = hit.collider.gameObject;
                if(touchedObject == frontDeer)
                {
                    if(isFront == true)
                    {
                        count++;
                        soundManager.DeerhornEffectPlay();
                    }
                }
            }
        }

        characterMover.RotationUpdate();
    }

    IEnumerator DeerTear()
    {
        //fdAnimator.SetTrigger("isWin");
        tearOne.SetActive(true);
        tearTwo.SetActive(true);
        rightHorn.SetActive(true);
        leftHorn.SetActive(true);
        tearOneAni.Play("TearOne");
        tearTwoAni.Play("TearTwo");
        rightHornAni.Play("RightHorn");
        leftHornAni.Play("LeftHorn");
        fdAnimator.SetBool("isWinning", true);
        yield return new WaitForSeconds(1);
        fdAnimator.SetBool("isWinning", false);
        frontDeer.SetActive(false);
        tearOne.SetActive(false);
        tearTwo.SetActive(false);
        rightHorn.SetActive(false);
        leftHorn.SetActive(false);
    }
}
