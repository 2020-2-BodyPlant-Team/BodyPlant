using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkUIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject touchedObject;
    RaycastHit2D hit;
    public Camera cam;


    public int lastSceneNumber;
    public GameObject panel;
    public bool isPanel = false;


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


        //트리를 만들건데, 가지가 제일 많은게 중앙 컴포넌트가 된다. 거기서 뻗어나간다.
        for (int i = 0; i < characterList.Count; i++)
        {
            ComponentClass centerComponent = characterList[i].components[0]; //중앙 기본값 넣어주고
            CharacterClass character = characterList[i];
            bool[] boolArray = new bool[characterList[i].components.Count];
            int bestEdge = 0;
            int centerIndex = 0;

            for (int k = 0; k < boolArray.Length; k++)
            {
                boolArray[k] = false;
            }
            for (int k = 0; k < characterList[i].components.Count; k++)
            {
                if (characterList[i].components[k].childIndexList.Count + characterList[i].components[k].childChildIndexList.Count > bestEdge)
                {
                    bestEdge = characterList[i].components[k].childIndexList.Count + characterList[i].components[k].childChildIndexList.Count;
                    centerComponent = characterList[i].components[k];
                    centerIndex = k;
                }
            }
            ComponentClass nowComponent = centerComponent;
            int nowIndex = centerIndex;
            nowComponent.parentComponentIndex = -1;
            Stack<int> childStack = new Stack<int>();
            boolArray[centerIndex] = true;
            childStack.Push(centerIndex);

            while (childStack.Count > 0)
            {
                nowIndex = childStack.Pop();    //시작할때 팝해야해.
                nowComponent = character.components[nowIndex];
                Debug.Log(nowIndex + " 팝하는 인덱스");
                for (int k = 0; k < nowComponent.childIndexList.Count; k++)
                {
                    if (boolArray[nowComponent.childIndexList[k]] == false)
                    {
                        childStack.Push(nowComponent.childIndexList[k]);
                        boolArray[nowComponent.childIndexList[k]] = true;

                        ComponentClass opposeComponent = characterList[i].components[nowComponent.childIndexList[k]];
                        opposeComponent.parentComponentIndex = nowIndex;
                        opposeComponent.parentJointIndex = 0;
                        if (nowComponent.childJointList[k] != 0)
                        {
                            //센터에 전완이 붙었을 때 스위치 해줘야해.
                            opposeComponent.secondSwitch = true;

                        }
                        /*
                        characterList[i].components[k].realGameobject.transform.
                            SetParent(nowComponent.realGameobject.transform);
                        */
                    }
                }

                for (int k = 0; k < nowComponent.childChildIndexList.Count; k++)
                {
                    if (boolArray[nowComponent.childChildIndexList[k]] == false)
                    {
                        childStack.Push(nowComponent.childChildIndexList[k]);
                        boolArray[nowComponent.childChildIndexList[k]] = true;

                        ComponentClass opposeComponent = characterList[i].components[nowComponent.childChildIndexList[k]];
                        opposeComponent.parentComponentIndex = nowIndex;
                        opposeComponent.parentJointIndex = 1;
                        Debug.Log(opposeComponent.name);
                        if (nowComponent.childChildJointList[k] != 0)
                        {
                            //센터에 전완이 붙었을 때 스위치 해줘야해.
                            opposeComponent.secondSwitch = true;

                        }
                    }
                }
            }
        }

        //캐릭터들 불러오는칸
        for (int i = 0; i < characterList.Count; i++)
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
                string path;
                if (component.secondSwitch)
                {
                    path = "Components/Complete/" + component.name + "2";
                }
                else
                {
                    path = "Components/Complete/" + component.name;
                }

                GameObject componentObj = Resources.Load<GameObject>(path);
                GameObject inst = Instantiate(componentObj, parent.transform);
                string name = component.name;

                Vector3 localPos = Vector3.zero;
                if (name == "body")
                {
                    localPos = new Vector3(0, 0, i * 10 + 4f + bodyNumber * 0.01f);
                    bodyNumber++;
                }
                if (name == "arm" || name == "leg")
                {
                    localPos = new Vector3(0, 0, i * 10 + 3f + armLegNumber * 0.01f);
                    armLegNumber++;
                }
                if (name == "hand" || name == "foot")
                {
                    localPos = new Vector3(0, 0, i * 10 + 2f + handFootNumber * 0.01f);
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

                    if (component.rotation.z > 270 || component.rotation.z < 90)
                    {
                        if (component.secondSwitch)
                        {
                            angle = component.rotation;
                        }
                        else
                        {
                            angle = Vector3.zero;
                        }

                    }
                    else
                    {
                        if (component.secondSwitch)
                        {
                            angle = Vector3.zero;

                        }
                        else
                        {
                            angle = component.rotation;

                        }
                    }

                    component.childObject = component.realGameobject.transform.GetChild(0).gameObject;
                    rotationList.Add(0);
                    randomRotateTimeList.Add(Random.Range(1f, 2f));
                    randomAngleList.Add(new Vector3(0, 0, angle.z + Random.Range(-30, 30)));
                    startAngleList.Add(angle);
                    originAngleList.Add(angle.z);
                    rotatingObjectList.Add(component.childObject);
                }
                if (component.secondSwitch)
                {
                    inst.transform.position = new Vector3(component.secondPosition.x, component.secondPosition.y, localPos.z);
                }
                else
                {
                    inst.transform.position = new Vector3(component.position.x, component.position.y, localPos.z);
                }
                //inst.transform.position = new Vector3(inst.transform.position.x, inst.transform.position.y, localPos.z);
                inst.transform.eulerAngles = component.rotation;
            }

            foreach (ComponentClass component in characterList[i].components)
            {
                if (component.parentComponentIndex == -1)
                {
                    continue;
                }
                if (component.parentJointIndex == 0)
                {
                    if (characterList[i].components[component.parentComponentIndex].secondSwitch)
                    {
                        component.realGameobject.transform.SetParent
    (characterList[i].components[component.parentComponentIndex].childObject.transform);
                    }
                    else
                    {
                        component.realGameobject.transform.SetParent
    (characterList[i].components[component.parentComponentIndex].realGameobject.transform);
                    }

                }
                else
                {
                    if (characterList[i].components[component.parentComponentIndex].secondSwitch)
                    {
                        component.realGameobject.transform.SetParent
(characterList[i].components[component.parentComponentIndex].realGameobject.transform);
                    }
                    else
                    {
                        component.realGameobject.transform.SetParent
(characterList[i].components[component.parentComponentIndex].childObject.transform);
                    }


                }

            }

            characterObjectList.Add(parent);
        }
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //flowerpotmanager에 있던 raycast로 해봤습니다
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
            if (!isPanel)
            {
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {
                    isPanel = true;
                    touchedObject = hit.collider.gameObject; 
                    panel.SetActive(true);
                }   
            }
        }
    }

    public void YesBtn()
    {
        //touchedObject에 들어간 캐릭터를 밑에 씬으로 이동시키고 싶습니다
        SceneManager.LoadScene(gameManager.workSceneIndex);
    }

    public void NoBtn()
    {
        panel.SetActive(false);
        isPanel = false;
    }
}
