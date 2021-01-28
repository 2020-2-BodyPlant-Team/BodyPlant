using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<CharacterClass> characterList;
    public GameObject DiaryPrefab;
    public List<GameObject> ButtonList;
    List<GameObject> prefabList;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;

        for(int i = 0; i < characterList.Count; i++)
        {
            if(characterList[i].name != null)
            {
                prefabList[i] = Instantiate(DiaryPrefab);
                ButtonList[i].SetActive(true);
                
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonFunction()
    {
        for(int i = 0; i < characterList.Count; i++)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) == ButtonList[i])
            {
                if(prefabList[i] == false)
                {
                    prefabList[i].SetActive(true);
                }
            }
        }
    }
    
}
