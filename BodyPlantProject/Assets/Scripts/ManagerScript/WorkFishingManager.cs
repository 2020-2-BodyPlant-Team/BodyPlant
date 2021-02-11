using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkFishingManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;

    public GameObject canvas;
    public GameObject icon;
    Animator iconAnimator;
    public float posX;
    public float animSpeed;
    float loopTime;

    public GameObject panjung;
    float a = 0;
    public float b = 0;

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

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        iconAnimator = icon.GetComponent<Animator>();
        canvas.SetActive(false);
        
        StartCoroutine("Coloring");
    }
    

    IEnumerator Coloring()
    {
        canvas.SetActive(true);
        animSpeed = Random.Range(1f, 5f);
        iconAnimator.SetFloat("fishingSpeed", animSpeed);
        iconAnimator.SetTrigger("isFishing");
        while (posX <= 0)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(Color.red, Color.yellow, a);
            a = (posX + 300) / 300;
            yield return new WaitForSeconds(0.03f);
        }
        while (posX >= 0)
        {
            posX = icon.transform.localPosition.x;
            panjung.GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.green, b);
            b = posX / 300;
            yield return new WaitForSeconds(0.03f);
            if (b >= 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //보조성분 추가할 곳
                    Debug.Log("보조성분 획득");
                }
            }
            //판정 바가 검정색으로 변하는 것 아직 구현x
        }
        canvas.SetActive(false);
        loopTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(loopTime);
        StartCoroutine("Coloring");
    }
}
