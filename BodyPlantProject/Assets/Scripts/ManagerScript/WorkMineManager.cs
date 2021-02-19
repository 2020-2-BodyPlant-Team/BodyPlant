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
    public GiveCoin coinManager;
    public GameObject[] parentObjectArray;
    public GameObject bringButton;


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

        if (characterList.Count > 3)
        {
            Application.Quit(); //몰라 꺼버려 ㅋㅋ
        }
        if (characterList.Count == 3 || saveData.characterList.Count == 0)
        {
            bringButton.SetActive(false);
        }


        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < parentObjectArray.Length; i++)
        {
            parentObjectArray[i].SetActive(false);
        }


        for (int i = 0; i < characterList.Count; i++)
        {
            parentObjectArray[i].SetActive(true);
            GameObject characterObject;
            characterMover.SpawnCharacter(characterList[i], i);
            characterObject = characterList[i].realGameobject;
            characterObject.transform.position = new Vector3(parentObjectArray[i].transform.position.x, parentObjectArray[i].transform.position.y, 0);
            characterObject.transform.localScale = parentObjectArray[i].transform.localScale;
            parentObjectArray[i].transform.SetParent(characterObject.transform);
        }

        coinManager.SetCharacterList(characterList, 2);
        


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
        characterMover.RotationUpdate();
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


    }
}
