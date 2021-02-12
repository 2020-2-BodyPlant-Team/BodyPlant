﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    [SerializeField]
    List<CharacterClass> characterList;
    List<GameObject> characterObjectList;
    List<float> timerList;
    List<float> randomTimeList;
    List<Vector3> randomPosList;
    List<Vector3> startPosList;

    List<float> rotationList;
    List<float> randomRotateTimeList;
    List<GameObject> rotatingObjectList;
    List<Vector3> randomAngleList;
    List<Vector3> startAngleList;
    List<float> originAngleList;


    public void PotSceneLoad()
    {
        gameManager.PotSceneLoad();
    }

    public GameObject panel;
    public void PanelLoad() //일하기 버튼 팝업 켜고 끄기
    {
        if(panel.activeSelf == false)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void WorkMineSceneLoad()
    {
        gameManager.WorkMineSceneLoad();
    }

    public void WorkHuntSceneLoad()
    {
        gameManager.WorkHuntSceneLoad();
    }

    public void WorkFishingSceneLoad()
    {
        gameManager.WorkFishingSceneLoad();
    }

    public void StoreSceneLoad()
    {
        gameManager.StoreSceneLoad();
    }

    public void ComposeSceneLoad()
    {
        gameManager.ComposeSceneLoad();
    }

    public static int coinAmount;   //상점에서 가구샀을때 불러오기
    int isToySold;
    int isBeanBagSold;
    public GameObject toy;
    public GameObject beanBag;

    public void BookSceneLoad()
    {
        gameManager.BookSceneLoad();
    }

    ComponentDataClass FindData(string name)
    {
        foreach (ComponentDataClass data in wholeComponents.componentList)
        {
            if (name == data.name)
            {
                return data;
            }
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.characterList;
        characterObjectList = new List<GameObject>();
        randomPosList = new List<Vector3>();
        startPosList = new List<Vector3>();

        randomRotateTimeList = new List<float>();
        randomAngleList = new List<Vector3>();
        startAngleList = new List<Vector3>();
        originAngleList = new List<float>();
        timerList = new List<float>();
        randomTimeList = new List<float>();
        rotationList = new List<float>();
        rotatingObjectList = new List<GameObject>();

        //캐릭터들 불러오는칸
        for(int i = 0; i < characterList.Count; i++)
        {
            Debug.Log(characterList[i].name);
            timerList.Add(0);
            randomTimeList.Add(Random.Range(1f, 2f));
            randomPosList.Add(new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 0f), 0));
            startPosList.Add(Vector3.zero);
            GameObject parent = new GameObject();

            int bodyNumber = 0;
            int armLegNumber = 0;
            int handFootNumber = 0;
            int earEyeNumber = 0;   //이목구비
            int hairNumber = 0;
            foreach (ComponentClass component in characterList[i].components)
            {
                GameObject componentObj = Resources.Load<GameObject>("Components/Complete/" + component.name);
                GameObject inst = Instantiate(componentObj, parent.transform);
                string name = component.name;

                Vector3 localPos = Vector3.zero;
                if (name == "body")
                {
                    localPos = new Vector3(0, 0,i * 10 +4f + bodyNumber * 0.01f);
                    bodyNumber++;
                }
                if (name == "arm" || name == "leg")
                {
                    localPos = new Vector3(0, 0, i * 10 + 3f + armLegNumber * 0.01f);
                    armLegNumber++;
                }
                if (name == "hand" || name == "foot")
                {
                    localPos = new Vector3(0, 0,i * 10 + 2f + handFootNumber * 0.01f);
                    handFootNumber++;
                }
                if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
                {
                    localPos = new Vector3(0, 0, i * 10 + 1f + earEyeNumber * 0.01f);
                    earEyeNumber++;
                }
                if (name == "hair")
                {
                    localPos = new Vector3(0, 0, i * 10 + hairNumber * 0.01f);
                    hairNumber++;
                }

                component.realGameobject = inst;


                rotationList.Add(0);
                randomRotateTimeList.Add(Random.Range(1f, 2f));
                randomAngleList.Add(new Vector3(0, 0, Random.Range(-30, 30) + component.rotation.z));
                startAngleList.Add(component.rotation);
                originAngleList.Add(component.rotation.z);
                rotatingObjectList.Add(component.realGameobject);

                if (FindData(component.name).isChild)
                {
                    Vector3 angle;

                    if (component.rotation.z >270 || component.rotation.z < 90)
                    {
                        
                        angle = Vector3.zero;
                    }
                    else
                    {
                        angle = component.rotation;
                    }

                    component.childObject = component.realGameobject.transform.GetChild(0).gameObject;
                    rotationList.Add(0);
                    randomRotateTimeList.Add(Random.Range(1f, 2f));
                    randomAngleList.Add(new Vector3(0, 0, angle.z + Random.Range(-30, 30)));
                    startAngleList.Add(angle);
                    originAngleList.Add(angle.z);
                    rotatingObjectList.Add(component.childObject);
                }
                inst.transform.localPosition = component.position;
                inst.transform.position = new Vector3(inst.transform.position.x, inst.transform.position.y, localPos.z);
                inst.transform.eulerAngles = component.rotation;

            }
            characterObjectList.Add(parent);
        }

        //트리를 만들건데, 가지가 제일 많은게 중앙 컴포넌트가 된다. 거기서 뻗어나간다.
        for(int i = 0; i < characterList.Count; i++)
        {
            ComponentClass centerComponent = characterList[i].components[0]; //중앙 기본값 넣어주고
            CharacterClass character = characterList[i];
            bool[] boolArray = new bool[characterList[i].components.Count];
            int bestEdge = 0;
            int centerIndex = 0;
            for(int k = 0; k < boolArray.Length; k++)
            {
                boolArray[k] = false;
            }
            for (int k = 0; k < characterList[i].components.Count; k++)
            {
                /*if(characterList[i].components[k].name == "body")
                {
                    centerComponent = characterList[i].components[k];
                    centerIndex = k;
                    boolArray[k] = true;
                    break;
                }*/
                if(characterList[i].components[k].childIndexList.Count + characterList[i].components[k].childChildIndexList.Count > bestEdge)
                {
                    bestEdge = characterList[i].components[k].childIndexList.Count + characterList[i].components[k].childChildIndexList.Count;
                    centerComponent = characterList[i].components[k];
                    centerIndex = k;
                }
            }
            ComponentClass nowComponent = centerComponent;
            Stack<int> childStack = new Stack<int>();
            boolArray[centerIndex] = true;

            while (childStack.Count >0)
            {

                for (int k = 0; k < nowComponent.childIndexList.Count; k++)
                {
                    if(boolArray[nowComponent.childIndexList[k]] == false)
                    {
                        childStack.Push(nowComponent.childIndexList[k]);
                        boolArray[nowComponent.childIndexList[k]] = true;

                        if (characterList[i].components[k].childJointList[k] == 0)
                        {

                        }
                        else
                        {

                        }

                        characterList[i].components[k].realGameobject.transform.
                            SetParent(nowComponent.realGameobject.transform);

                    }
                }

                for (int k = 0; k < nowComponent.childChildIndexList.Count; k++)
                {
                    if (boolArray[nowComponent.childChildIndexList[k]] == false)
                    {
                        childStack.Push(nowComponent.childChildIndexList[k]);
                        boolArray[nowComponent.childChildIndexList[k]] = true;
                        characterList[i].components[k].realGameobject.transform.
                            SetParent(nowComponent.childObject.transform);
                    }
                }
                nowComponent = character.components[childStack.Pop()];


            }
            /*
            for (int k = 0; k < characterList[i].components.Count; k++)
            {
                GameObject componentObj = characterList[i].components[k].realGameobject;
                GameObject childObj = characterList[i].components[k].childObject;


                for (int n = 0; n<characterList[i].components[k].childIndexList.Count; n++)
                {
                    if(characterList[i].components[k].childJointList[n] == 0)
                    {
                        characterList[i].components[k].realGameobject.transform.
                            SetParent(characterList[i].components[characterList[i].components[k].childIndexList[n]].realGameobject.transform);
                    }
                    else
                    {
                        characterList[i].components[k].realGameobject.transform.
                            SetParent(characterList[i].components[characterList[i].components[k].childIndexList[n]].childObject.transform);
                    }
                    
                }
                for (int n = 0; n < characterList[i].components[k].childChildIndexList.Count; n++)
                {
                    characterList[i].components[characterList[i].components[k].childChildIndexList[n]].realGameobject.
                        transform.SetParent(childObj.transform);
                }

            }*/
        }

        coinAmount = PlayerPrefs.GetInt ("CoinAmount"); //상점에서 가구 샀을 때 불러오기
        isToySold = PlayerPrefs.GetInt ("IsToySold");
        isBeanBagSold = PlayerPrefs.GetInt ("IsBeanBagSold");
        
        if (isToySold == 1)
            toy.SetActive (true);
        else
            toy.SetActive(false);

        if (isBeanBagSold == 1)
            beanBag.SetActive(true);
        else
            beanBag.SetActive(false);
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
                randomPosList[i] = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 0f), 0);
            }
        }
        //-2.5~2.5, 0~-3

        for(int i = 0; i< rotationList.Count; i++)
        {
            rotatingObjectList[i].transform.eulerAngles = Vector3.Lerp(startAngleList[i], randomAngleList[i], rotationList[i] / randomRotateTimeList[i]);
            rotationList[i] += Time.deltaTime;
            if (rotationList[i] > randomRotateTimeList[i])
            {
                rotationList[i] = 0;
                randomRotateTimeList[i] = Random.Range(1f, 2f);
                startAngleList[i] = randomAngleList[i];
                randomAngleList[i] = new Vector3(0, 0, originAngleList[i]+ Random.Range(-30, 30));
            }
        }
    }
}
