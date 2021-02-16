using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkHuntManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    List<CharacterClass> characterList;

    float waitSec;
    public GameObject sideDeer;
    Animation sideAni;

    public float limitTime = 2f;
    public GameObject frontDeer;
    public bool isFront = false;
    public CharacterMover characterMover;

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
        gameManager.SecretRoomSceneLoad();
    }

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.huntCharacterList;
        
        sideAni = sideDeer.GetComponent<Animation>();

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;


        for (int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i], i);
        }

        StartCoroutine("DeerOut");

    }

    IEnumerator DeerOut()
    {
        waitSec = Random.Range(3.0f, 5.0f);
        yield return new WaitForSeconds(waitSec);
        sideAni.Play("sideDeerMove");
        yield return new WaitForSeconds(2);
        {
            isFront = true;
            frontDeer.SetActive(true);
        }
    }
    
    void Update()
    {
        if(isFront == true)
        {
            limitTime -= Time.deltaTime;
            if(limitTime < 0)
            {
                isFront = false;
                limitTime = 2f;
                Debug.Log("타이머 끝");

                if (isFront == false)
                {
                    if (count >= 10)
                    {
                        Debug.Log("힘겨루기 승리");
                        frontDeer.SetActive(false);
                        count = 0;
                    }
                    else
                    {
                        Debug.Log("힘겨루기 패배");
                        frontDeer.SetActive(false);
                        count = 0;
                    }
                    StartCoroutine("DeerOut");
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                touchedObject = hit.collider.gameObject;
                if(touchedObject == frontDeer)
                {
                    count++;
                }
            }
        }

        characterMover.RotationUpdate();
    }
}
