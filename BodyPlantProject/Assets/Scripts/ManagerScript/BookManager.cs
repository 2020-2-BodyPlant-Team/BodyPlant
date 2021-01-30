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
    public GameObject ButtonPrefab;
    List<GameObject> prefabList;
    List<GameObject> buttonList;
    List<int> passedTime;
    int buttonXgap = 300;
    int buttonYgap = -415;    
    Vector3 buttonStartPoint = new Vector3(-300, 370, 0);
    public GameObject Parent;


    // Start is called before the first frame update
    void Awake()
    {
        
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;

        prefabList = new List<GameObject>();
        buttonList = new List<GameObject>();

        for(int i = 0; i < characterList.Count; i++)
        {
            
            
            prefabList.Add(Instantiate(DiaryPrefab, Parent.transform));
                               
            buttonList.Add(Instantiate(ButtonPrefab, Parent.transform));
            
            

            prefabList[i].transform.GetChild(1).GetComponent<Text>().text = characterList[i].createdDate;
            //prefabList[i].transform.GetChild(2).GetComponent<Text>().text = characterList[i].personality;
            prefabList[i].transform.GetChild(3).GetComponent<Text>().text = characterList[i].loveNess.ToString();
            prefabList[i].transform.GetChild(4).GetComponent<Text>().text = characterList[i].name;
            
                      
        }
    }

    // Update is called once per frame
    void Update()
    {
        ButtonFunction();
    }

    public void ButtonFunction()
    {
        for(int i = 0; i < characterList.Count; i++)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) == buttonList[i])
            {
                if(prefabList[i] == false)
                {
                    prefabList[i].SetActive(true);
                }
            }
        }
    }
    
}
