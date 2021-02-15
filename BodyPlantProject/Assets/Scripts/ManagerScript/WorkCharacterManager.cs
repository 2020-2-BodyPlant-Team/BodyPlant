using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkCharacterManager : MonoBehaviour
{

    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    [SerializeField]
    List<CharacterClass> characterList;
    List<GameObject> characterObjectList;
    List<float> timerList;
    List<float> randomTimeList;

    List<float> rotationList;
    List<float> randomRotateTimeList;
    List<GameObject> rotatingObjectList;
    List<Vector3> randomAngleList;
    List<Vector3> startAngleList;
    List<float> originAngleList;

    // Start is called before the first frame update

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;

        characterObjectList = new List<GameObject>();

        randomRotateTimeList = new List<float>();
        randomAngleList = new List<Vector3>();
        startAngleList = new List<Vector3>();
        originAngleList = new List<float>();
        timerList = new List<float>();
        randomTimeList = new List<float>();
        rotationList = new List<float>();
        rotatingObjectList = new List<GameObject>();
    }

    //0 hunt, 1 mine, 2 fish
    void WorkSceneStart(int sceneIndex)
    {
        if(sceneIndex == 0)
        {
            characterList = saveData.huntCharacterList;
        }
        else if (sceneIndex == 1)
        {
            characterList = saveData.mineCharacterList;
        }
        else
        {
            characterList = saveData.fishCharacterList;
        }

        if (characterList.Count == 0)
        {
            return;
        }
        //트리를 만들건데, 가지가 제일 많은게 중앙 컴포넌트가 된다. 거기서 뻗어나간다.
       
        foreach(CharacterClass character in characterList)
        {
            SpawnCharacter(character,sceneIndex);
        }
    }

    //데려오기 하는거면 adding == true
    public void SpawnCharacter(CharacterClass character,int sceneIndex)
    {
        ComponentClass centerComponent = character.components[0]; //중앙 기본값 넣어주고
        bool[] boolArray = new bool[character.components.Count];
        int bestEdge = 0;
        int centerIndex = 0;
        int characterIndex = 0;


        for(int i = 0; i< characterList.Count; i++)
        {
            if(character == characterList[i])
            {
                characterIndex = i;
                break;
            }
        }



        for (int k = 0; k < boolArray.Length; k++)
        {
            boolArray[k] = false;
        }
        for (int k = 0; k < character.components.Count; k++)
        {
            if (character.components[k].childIndexList.Count + character.components[k].childChildIndexList.Count > bestEdge)
            {
                bestEdge = character.components[k].childIndexList.Count + character.components[k].childChildIndexList.Count;
                centerComponent = character.components[k];
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

                    ComponentClass opposeComponent = character.components[nowComponent.childIndexList[k]];
                    opposeComponent.parentComponentIndex = nowIndex;
                    opposeComponent.parentJointIndex = 0;
                    if (nowComponent.childJointList[k] != 0)
                    {
                        //센터에 전완이 붙었을 때 스위치 해줘야해.
                        opposeComponent.secondSwitch = true;

                    }
                }
            }

            for (int k = 0; k < nowComponent.childChildIndexList.Count; k++)
            {
                if (boolArray[nowComponent.childChildIndexList[k]] == false)
                {
                    childStack.Push(nowComponent.childChildIndexList[k]);
                    boolArray[nowComponent.childChildIndexList[k]] = true;

                    ComponentClass opposeComponent = character.components[nowComponent.childChildIndexList[k]];
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
        Debug.Log(character.name);
        timerList.Add(0);
        GameObject parent = new GameObject();

        int bodyNumber = 0;
        int armLegNumber = 0;
        int handFootNumber = 0;
        int earEyeNumber = 0;   //이목구비
        int hairNumber = 0;
        foreach (ComponentClass component in character.components)
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
                localPos = new Vector3(0, 0, characterIndex * 10 + 4f + bodyNumber * 0.01f);
                bodyNumber++;
            }
            if (name == "arm" || name == "leg")
            {
                localPos = new Vector3(0, 0, characterIndex * 10 + 3f + armLegNumber * 0.01f);
                armLegNumber++;
            }
            if (name == "hand" || name == "foot")
            {
                localPos = new Vector3(0, 0, characterIndex * 10 + 2f + handFootNumber * 0.01f);
                handFootNumber++;
            }
            if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
            {
                localPos = new Vector3(0, 0, characterIndex * 10 + 1f + earEyeNumber * 0.01f);
                earEyeNumber++;
            }
            if (name == "hair")
            {
                localPos = new Vector3(0, 0, characterIndex * 10 + hairNumber * 0.01f);
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

        foreach (ComponentClass component in character.components)
        {
            if (component.parentComponentIndex == -1)
            {
                continue;
            }
            if (component.parentJointIndex == 0)
            {
                if (character.components[component.parentComponentIndex].secondSwitch)
                {
                    component.realGameobject.transform.SetParent
(character.components[component.parentComponentIndex].childObject.transform);
                }
                else
                {
                    component.realGameobject.transform.SetParent
(character.components[component.parentComponentIndex].realGameobject.transform);
                }

            }
            else
            {
                if (character.components[component.parentComponentIndex].secondSwitch)
                {
                    component.realGameobject.transform.SetParent
(character.components[component.parentComponentIndex].realGameobject.transform);
                }
                else
                {
                    component.realGameobject.transform.SetParent
(character.components[component.parentComponentIndex].childObject.transform);
                }


            }

        }
        character.realGameobject = parent;
        characterObjectList.Add(parent);

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

    // Update is called once per frame
    void Update()
    {
        if(characterList.Count > 0)
        {
            for (int i = 0; i < rotationList.Count; i++)
            {
                rotatingObjectList[i].transform.eulerAngles = Vector3.Lerp(startAngleList[i], randomAngleList[i], rotationList[i] / randomRotateTimeList[i]);
                rotationList[i] += Time.deltaTime;
                if (rotationList[i] > randomRotateTimeList[i])
                {
                    rotationList[i] = 0;
                    randomRotateTimeList[i] = Random.Range(1f, 2f);
                    startAngleList[i] = randomAngleList[i];
                    randomAngleList[i] = new Vector3(0, 0, originAngleList[i] + Random.Range(-30, 30));
                }
            }
        }
    }
}
