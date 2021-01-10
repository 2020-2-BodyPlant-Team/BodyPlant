using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<CharacterClass> characterList;
    public GameObject canvasPrefab;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
