using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<CharacterClass> characterList;
    public GameObject diaryPrefab;
    public GameObject buttonPrefab;
    List<GameObject> prefabList;
    List<GameObject> buttonList;
    List<int> passedTime;
    int buttonXgap = 300;
    int buttonYgap = -415;    
    Vector2 buttonStartPoint = new Vector2(-300, 370);
    public GameObject parent;


    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;

        prefabList = new List<GameObject>();
        buttonList = new List<GameObject>();

        for(int i = 0; i < characterList.Count; i++)
        { 
            prefabList.Add(Instantiate(diaryPrefab, parent.transform));                   
            buttonList.Add(Instantiate(buttonPrefab, parent.transform));
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((buttonStartPoint.x + (i % 3) * buttonXgap), (buttonStartPoint.y  + (int)(i / 3) * buttonYgap));
            Button button = buttonList[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { ButtonFunction(); });

            prefabList[i].transform.GetChild(1).GetComponent<Text>().text = characterList[i].createdDate;
            //prefabList[i].transform.GetChild(2).GetComponent<Text>().text = characterList[i].personality;
            prefabList[i].transform.GetChild(3).GetComponent<Text>().text = characterList[i].loveNess.ToString();
            prefabList[i].transform.GetChild(4).GetComponent<Text>().text = characterList[i].name;              
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
            if(prefabList[i] == false)
            {
                prefabList[i].SetActive(true);
            }
        }
    }

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }
    
}
