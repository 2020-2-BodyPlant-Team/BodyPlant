using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
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

    public bool rotationBool;
    public bool positionBool;




    void Start()
    {

        gameManager = GameManager.singleTon;
        wholeComponents = gameManager.wholeComponents;
        characterObjectList = new List<GameObject>();
        characterList = new List<CharacterClass>();
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
        rotationBool = true;
        positionBool = true;
    }
    


    //이제 다른 스크립트에서 요걸 쓸거에요.
    public void SpawnCharacter(CharacterClass character,int characterIndex)
    {
        ComponentClass centerComponent = character.components[0]; //중앙 기본값 넣어주고
        bool[] boolArray = new bool[character.components.Count];
        int bestEdge = 0;
        int centerIndex = 0;
        characterList.Add(character);
        randomTimeList.Add(Random.Range(1f, 2f));
        randomPosList.Add(new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 0f), 0));
        startPosList.Add(Vector3.zero);


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
                    if (nowComponent.childChildJointList[k] != 0)
                    {
                        //센터에 전완이 붙었을 때 스위치 해줘야해.
                        opposeComponent.secondSwitch = true;

                    }
                }
            }
        }
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

        rotationList.Add(0);
        randomRotateTimeList.Add(Random.Range(1f, 2f));
        randomAngleList.Add(new Vector3(0, 0, Random.Range(-5, 5)));
        startAngleList.Add(Vector3.zero);
        originAngleList.Add(0);
        rotatingObjectList.Add(parent);


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

    public void PositionUpdate()
    {
        for (int i = 0; i < timerList.Count; i++)
        {
            characterObjectList[i].transform.position = Vector3.Lerp(startPosList[i], randomPosList[i], timerList[i] / randomTimeList[i]);
            timerList[i] += Time.deltaTime;
            if (timerList[i] > randomTimeList[i])
            {
                timerList[i] = 0;
                randomTimeList[i] = Random.Range(1f, 2f);
                startPosList[i] = randomPosList[i];
                randomPosList[i] = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 0f), 0);
            }
        }

    }

    public void RotationUpdate()
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

    public void FishingUpdate()
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
                randomAngleList[i] = new Vector3(0, 0, originAngleList[i] + Random.Range(-5, 5));
            }
        }
    }

    public CharacterClass ChooseCharacter(GameObject touchedObject)
    {
        GameObject characterObject = touchedObject;
        int characterIndex = -1;
        while (characterObject.transform.parent != null)
        {
            characterObject = characterObject.transform.parent.gameObject;
        }
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterObject == characterList[i].realGameobject)
            {
                characterIndex = i;
                break;
            }
        }
        if (characterIndex == -1)
        {
            Debug.Log("좆됐다 캐릭터를 못찾았다");
            return null;
        }
        return characterList[characterIndex];
    }



}
