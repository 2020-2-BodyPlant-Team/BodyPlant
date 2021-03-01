using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkUIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject touchedObject;
    RaycastHit2D hit;
    public Camera cam;


    public GameObject panel;
    public bool isPanel = false;
    public Text bringText;

    CharacterClass chosenCharacter;


    SaveDataClass saveData;
    WholeComponents wholeComponents;
    [SerializeField]
    List<CharacterClass> characterList;
    public CharacterMover characterMover;


    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.characterList;

        for(int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i], i);
        }
    }

    void Update()
    {
        characterMover.PositionUpdate();
        characterMover.RotationUpdate();

        if (Input.GetMouseButtonDown(0))    //flowerpotmanager에 있던 raycast로 해봤습니다
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
            if (!isPanel)
            {
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {

                    touchedObject = hit.collider.gameObject;
                    chosenCharacter = characterMover.ChooseCharacter(touchedObject);
                    if (chosenCharacter != null)
                    {
                        isPanel = true;
                        panel.SetActive(true);
                        string productedName = gameManager.GetCompleteWord(chosenCharacter.name, "\"이를", "\"를");
                        bringText.text = "\"" + productedName + " 데려갈까요?";
                    }

                }   
            }
        }
    }


    //0 hunt, 1 mine, 2 fish
    //6 hunt, 7 mine, 8 fish
    public void YesBtn()
    {
        List<CharacterClass> loveCharacterList = saveData.characterList;
        float loveRatio = 1.0f;
        for(int i = 0; i< loveCharacterList.Count; i++)
        {
            if(loveCharacterList[i].personality == CharacterClass.Personality.Jogon)
            {
                loveRatio += 0.02f;
            }
        }

        for (int i = 0; i < loveCharacterList.Count; i++)
        {
            float ratio = loveRatio;
            if (loveCharacterList[i].personality == CharacterClass.Personality.Mongsil)
            {
                ratio += 0.05f;
            }
            int time = gameManager.TimeSubtractionToSeconds(loveCharacterList[i].loveStartTime, DateTime.Now.ToString());
            loveCharacterList[i].loveTime += time;
            loveCharacterList[i].loveStartTime = DateTime.Now.ToString();
            loveCharacterList[i].loveNess += gameManager.loveRatio * ratio * time;
        }



        List<CharacterClass> characterList = saveData.huntCharacterList;
        if (gameManager.workSceneIndex == 7)
        {
            characterList = saveData.mineCharacterList;
        }
        else if (gameManager.workSceneIndex == 8)
        {
            characterList = saveData.fishCharacterList;
        }

        if (characterList.Count < 5)
        {
            saveData.characterList.Remove(chosenCharacter);
            characterList.Add(chosenCharacter);
        }
        chosenCharacter.lastEarnedTime = DateTime.Now.ToString();


        gameManager.Save();

        //touchedObject에 들어간 캐릭터를 밑에 씬으로 이동시키고 싶습니다
        SceneManager.LoadScene(gameManager.workSceneIndex);

    }

    public void NoBtn()
    {
        panel.SetActive(false);
        isPanel = false;
    }
}
