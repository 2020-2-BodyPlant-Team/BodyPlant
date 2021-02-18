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
    public GameObject[] parentObjectArray;
    public GameObject deerObject;
    float deerMaxX = 8;
    float deerMinX = -8;

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
            characterList[i].realGameobject.transform.SetParent(parentObjectArray[i].transform);
            characterList[i].realGameobject.transform.localPosition = Vector3.zero;
            characterList[i].realGameobject.transform.localScale = new Vector3(1, 1, 1);
        }

        

        StartCoroutine("DeerOut");
        StartCoroutine(DeerMove());

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
    

    IEnumerator DeerMove()
    {
        float velocity = -0.015f;
        while (true)
        {
            yield return null;
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
