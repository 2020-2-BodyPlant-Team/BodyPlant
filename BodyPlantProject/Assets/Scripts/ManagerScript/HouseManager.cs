using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    [SerializeField]
    List<CharacterClass> characterList;
    List<GameObject> characterObjectList;
    List<float> timerList;
    List<float> randomTimeList;
    List<Vector3> randomPosList;
    List<Vector3> startPosList;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        characterList = saveData.characterList;
        characterObjectList = new List<GameObject>();
        randomPosList = new List<Vector3>();
        startPosList = new List<Vector3>();
        timerList = new List<float>();
        randomTimeList = new List<float>();
        for(int i = 0; i < characterList.Count; i++)
        {
            Debug.Log(characterList[i].name);
            timerList.Add(0);
            randomTimeList.Add(Random.Range(1f, 2f));
            randomPosList.Add(new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 4f), 0));
            startPosList.Add(Vector3.zero);
            GameObject parent = new GameObject();
            foreach (ComponentClass component in characterList[i].components)
            {
                GameObject componentObj = Resources.Load<GameObject>("Components/" + component.name);
                GameObject inst = Instantiate(componentObj, parent.transform);
                inst.transform.localPosition = component.position;
                inst.transform.eulerAngles = component.rotation;

            }
            characterObjectList.Add(parent);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < timerList.Count; i++)
        { 
            characterObjectList[i].transform.position = Vector3.Lerp(startPosList[i], randomPosList[i], timerList[i]/randomTimeList[i]);
            timerList[i] += Time.deltaTime;
            if(timerList[i] > randomTimeList[i])
            {
                timerList[i] = 0;
                randomTimeList[i] = Random.Range(1f, 2f);
                startPosList[i] = randomPosList[i];
                randomPosList[i] = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 4f), 0);
            }
        }
    }
}
