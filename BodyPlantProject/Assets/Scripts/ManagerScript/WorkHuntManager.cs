using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkHuntManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;

    float waitSec;
    public GameObject sideDeer;
    Animation sideAni;

    public float limitTime = 2f;
    public GameObject frontDeer;
    public bool isFront = false;

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

        sideAni = sideDeer.GetComponent<Animation>();
        waitSec = Random.Range(3.0f, 5.0f);
        
        StartCoroutine("DeerOut");
    }

    IEnumerator DeerOut()
    {
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
                    }
                    else
                    {
                        Debug.Log("힘겨루기 패배");
                        frontDeer.SetActive(false);
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
    }
}
