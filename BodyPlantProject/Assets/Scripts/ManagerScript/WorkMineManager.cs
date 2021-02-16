using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkMineManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    List<CharacterClass> characterList;

    public CharacterMover characterMover;


    Image barImage;
    float maxBar = 100f;
    public static float barAmount;

    public GameObject aim;


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
        characterList = saveData.mineCharacterList;

        barImage = GameObject.Find("Bar").GetComponent<Image>();
        barAmount = 0f;

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;


        for (int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i], i);
        }


        aim.SetActive(false);
        InvokeRepeating("SpawnAim", 2, 1);
    }
    
    void Update()
    {
        barImage.fillAmount = barAmount / maxBar;
        if(barAmount >= maxBar)
        {
            Debug.Log("게이지 만땅");
            barAmount = 0;
            //코인, 자원 추가할 코드 자리
        }
    }

    public void FillingBar()
    {
        barAmount += 10f;
        aim.SetActive(false);
    }

    void SpawnAim()
    {
        //Debug.Log("move");
        aim.SetActive(true);
        float posX = Random.Range(-1.0f, 1.5f);
        float posY = Random.Range(-4.0f, -1.0f);
        aim.transform.position = new Vector3(posX, posY, 0);

        characterMover.RotationUpdate();
    }
}
