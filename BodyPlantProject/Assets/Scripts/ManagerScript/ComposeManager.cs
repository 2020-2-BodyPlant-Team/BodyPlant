 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeManager : MonoBehaviour
{
    GameManager gameManager;
    SoundManager soundManager;
    SaveDataClass saveData;                 //캐릭터를 저장해줘야하니까.
    [SerializeField]
    List<ComponentClass> harvestedComponent;//버튼형태로 있는 부위.
    [SerializeField]
    List<ComponentClass> activedComponent;  //단상위에 올라가있는 부위
    int addedComponentNumber = 0;
    List<List<GameObject>> attachObjectList;
    WholeComponents wholeComponents;
    public RectTransform contentRect;       //시작값 700. 하나 늘어날떄마다 -400
    public GameObject buttonObject;         //시작값  0,-74f/  하나늘어날때마다 x만 +400
    List<GameObject> buttonList;
    List<int> removedButtonList;        //사라진 버튼의 인덱스 리스트
    public InputField nameInputField;       //이름 인풋필드
    string nameInput;                       //이름 넣은거
    public GameObject parentObject;         //부위들의 Parent가 되는 오브젝트

    public GameObject namingObject;
    public GameObject nameAskingObject;
    public Text nameAskingText;            //""(이)가 맞나요?
    public GameObject saveButton;           //저장가능하면 켜주기.

    public GameObject attachObject;         //관절 오브젝트

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    //public bool rotationMode;
    public bool flipMode;
    bool modifyMode;
    public GameObject modifyPanel;
    bool isModifiedCharacter;   //수정된 캐릭터인지 최초로 만드는 캐릭터인지.
    int modifyingIndex;
    public GameObject modifyButtonObject;
    public Text modifyPanelText;

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

    int bodyNumber = 0;
    int armLegNumber = 0;
    int handFootNumber = 0;
    int earEyeNumber = 0;   //이목구비
    int hairNumber = 0;

    bool notAttached = true;

    


    private void Start()
    {
        namingObject.SetActive(false);
        nameAskingObject.SetActive(false);
        gameManager = GameManager.singleTon;
        soundManager = SoundManager.inst;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        harvestedComponent = gameManager.saveData.owningComponentList;
        activedComponent = new List<ComponentClass>();
        attachObjectList = new List<List<GameObject>>();
        buttonList = new List<GameObject>();
        removedButtonList = new List<int>();
        //rotationMode = false;
        modifyMode = false;
        saveButton.SetActive(false);
        isModifiedCharacter = false;
        modifyPanel.SetActive(false);
        modifyingIndex = 0;
        //초기화

        contentRect.anchoredPosition = new Vector2(0, 0);   //자꾸 이거 움직임;; 위치 고정 안해주면 지맘대로 위치가 바껴요
        contentRect.sizeDelta = new Vector2(400* harvestedComponent.Count, 300);
        //contentRect 사이즈 조정을 해줘야 좌우로 움직일 수 있다.
        
        
        for(int i = 0; i< harvestedComponent.Count; i++)
        {
            ComponentDataClass componentData = FindData(harvestedComponent[i].name);
            GameObject inst = Instantiate(buttonObject, contentRect.transform);
            buttonList.Add(inst);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 400 + 200, 0);
            //-400*harvestedComponent.Count/2+ 
            inst.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Image image = inst.GetComponent<Image>();

            image.sprite = componentData.componentSpriteArray[0];
            image.SetNativeSize();
            /*
            Text text = inst.GetComponentInChildren<Text>();
            text.text = componentData.name;*/
            string name = componentData.name;
            int index = i;      //delegate에 그냥 i를 넣으면 되는데, 이상하게 int가 callbyRef로 넘어간다.
            Button button = inst.GetComponent<Button>();
            button.onClick.AddListener(delegate { SpawnComponent(name, index); });
            //버튼만드는 for문
        }

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
            randomTimeList.Add(UnityEngine.Random.Range(1f, 2f));
            randomPosList.Add(new Vector3(UnityEngine.Random.Range(7.5f, 12.5f), UnityEngine.Random.Range(-3f, 0f), 0));
            startPosList.Add(new Vector3(10, 0, 0));
            GameObject parent = new GameObject();
            characterList[i].realGameobject = parent;

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
                randomRotateTimeList.Add(UnityEngine.Random.Range(1f, 2f));
                if (component.rotation.y == 0)
                {
                    randomAngleList.Add(new Vector3(0, 0, UnityEngine.Random.Range(-30, 30) + component.rotation.z));
                }
                else
                {
                    randomAngleList.Add(new Vector3(0, 180, UnityEngine.Random.Range(-30, 30) + component.rotation.z));
                }
                startAngleList.Add(component.rotation);
                originAngleList.Add(component.rotation.z);
                rotatingObjectList.Add(component.realGameobject);

                if (FindData(component.name).isChild)
                {
                    Vector3 angle;

                    if (component.rotation.z > 270 || component.rotation.z < 90)
                    {

                        angle = Vector3.zero;
                    }
                    else
                    {
                        angle = component.rotation;
                    }

                    component.childObject = component.realGameobject.transform.GetChild(0).gameObject;
                    rotationList.Add(0);
                    randomRotateTimeList.Add(UnityEngine.Random.Range(1f, 2f));
                    if (component.rotation.y == 0)
                    {
                        randomAngleList.Add(new Vector3(0, 0, UnityEngine.Random.Range(-30, 30) + component.rotation.z));
                    }
                    else
                    {
                        randomAngleList.Add(new Vector3(0, 180, UnityEngine.Random.Range(-30, 30) + component.rotation.z));
                    }
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

    //이름으로 data찾아주는 함수
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
    int spawnedNumber = 0;
    //버튼을 누를 때 
    public void SpawnComponent(string name,int buttonIndex)
    {
        spawnedNumber++;
        int changedIndex = buttonIndex;
        if (modifyButtonObject.activeSelf)
        {
            modifyButtonObject.SetActive(false);
        }
        soundManager.ButtonEffectPlay();
        ComponentDataClass data = FindData(name);
        GameObject obj = Resources.Load<GameObject>("Components/Complete/" + name);
        GameObject inst = Instantiate(obj,parentObject.transform);
        inst.transform.eulerAngles = Vector3.zero;
        Vector3 localPos = Vector3.zero;
        if(name == "body")
        {
            localPos = new Vector3(0, 0, 4f+ bodyNumber * 0.01f);
            bodyNumber++;
        }
        if (name == "arm" || name == "leg")
        {
            localPos = new Vector3(0, 0, 3f+ armLegNumber * 0.01f);
            armLegNumber++;
        }
        if (name == "hand" || name == "foot")
        {
            localPos = new Vector3(0, 0, 2f+ handFootNumber * 0.01f);
            handFootNumber++;
        }
        if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
        {
            localPos = new Vector3(0, 0,1f+ earEyeNumber * 0.01f);
            earEyeNumber++;
        }
        if (name == "hair")
        {
            localPos = new Vector3(0, 0, hairNumber * 0.01f);
            hairNumber++;
        }
        inst.transform.localPosition = localPos;

        List<GameObject> objectList = new List<GameObject>();
        attachObjectList.Add(objectList);
        for(int i = 0; i< data.attachPosition.Count; i++)
        {
            GameObject attachInst = Instantiate(attachObject, inst.transform);
            objectList.Add(attachInst);
            attachInst.transform.localPosition = new Vector3(data.attachPosition[i].x, data.attachPosition[i].y, 0);
        }
        

        DragAttach drag =  inst.AddComponent<DragAttach>();
        drag.composeManager = this;
        
        //유닛 만들고

        for(int i = 0; i < removedButtonList.Count; i++)
        {
            if(changedIndex > removedButtonList[i])
            {
                changedIndex--;
            }
        }
        removedButtonList.Add(changedIndex);
        buttonList[changedIndex].SetActive(false);
        buttonList.Remove(buttonList[changedIndex]);

        ComponentClass component = harvestedComponent[buttonIndex];
        component.realGameobject = inst;
        activedComponent.Add(component);
        //harvestedComponent.Remove(component);

        contentRect.sizeDelta = new Vector2(400 * buttonList.Count, 300);

        for (int i  = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 400+200, 0);
        }
        saveButton.SetActive(false);
    }


    public void SpawnModifyComponent(ComponentClass component)
    {
        string name = component.name;
        soundManager.ButtonEffectPlay();
        addedComponentNumber++;
        ComponentDataClass data = FindData(name);
        GameObject obj = Resources.Load<GameObject>("Components/Complete/" + name);
        GameObject inst = Instantiate(obj, parentObject.transform);
        component.realGameobject = inst;
        activedComponent.Add(component);
        inst.transform.eulerAngles = component.rotation;
        Vector3 localPos = Vector3.zero;
        localPos.x = component.position.x;
        localPos.y = component.position.y;
        if (name == "body")
        {
            localPos.z =4f + bodyNumber * 0.01f;
            bodyNumber++;
        }
        if (name == "arm" || name == "leg")
        {
            localPos.z = 3f + armLegNumber * 0.01f;
            armLegNumber++;
        }
        if (name == "hand" || name == "foot")
        {
            localPos.z = 2f + handFootNumber * 0.01f;
            handFootNumber++;
        }
        if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
        {
            localPos.z = 1f + earEyeNumber * 0.01f;
            earEyeNumber++;
        }
        if (name == "hair")
        {
            localPos.z =hairNumber * 0.01f;
            hairNumber++;
        }
        inst.transform.localPosition = localPos;

        List<GameObject> objectList = new List<GameObject>();
        attachObjectList.Add(objectList);
        for (int i = 0; i < data.attachPosition.Count; i++)
        {
            GameObject attachInst = Instantiate(attachObject, inst.transform);
            objectList.Add(attachInst);
            attachInst.transform.localPosition = new Vector3(data.attachPosition[i].x, data.attachPosition[i].y, 0);
        }

        DragAttach drag = inst.AddComponent<DragAttach>();
        drag.composeManager = this;

        saveButton.SetActive(false);
    }

    public void SaveButton()
    {
        soundManager.ButtonEffectPlay();
        if (isModifiedCharacter)
        {
            nameAskingObject.SetActive(true);
            nameInput = characterList[modifyingIndex].name;
            string productedName = gameManager.GetCompleteWord(nameInput, "\"이가", "\"가");
            nameAskingText.text = "\"" + productedName + " 맞나요?";
        }
        else
        {
            namingObject.SetActive(true);
        }
            
        
    }

    public void NameButton()
    {
        soundManager.ButtonEffectPlay();
        namingObject.SetActive(false);
        nameAskingObject.SetActive(true);
        nameInput = nameInputField.text;
        string productedName = gameManager.GetCompleteWord(nameInput, "\"이가", "\"가");
        nameAskingText.text = "\"" + productedName + " 맞나요?";
    }

    
    public void NameInputChanging()
    {
        if(nameInputField.text.Length > 10)
        {
            nameInputField.text = nameInputField.text.Remove(10);
            if (!inputCorRunning)
            {
                soundManager.ErrorEffectPlay();
                StartCoroutine(NameInputShake());
            }
        }
    }

    bool inputCorRunning = false;
    IEnumerator NameInputShake()
    {
        inputCorRunning = true;
        Text inputText = nameInputField.textComponent;
        GameObject inputObject = nameInputField.gameObject;
        RectTransform rect = inputObject.GetComponent<RectTransform>();
        Vector2 originPos = rect.anchoredPosition;

        float x1 = -50;
        float x2 = -22;
        float y1 = 36;
        float y2 = 86;

        inputText.color = new Color(0.8f, 0, 0, 1);
        float timer = 0;
        while(timer < 0.3f)
        {
            timer += Time.deltaTime;
            float xRandom = UnityEngine.Random.Range(x1, x2);
            float yRandom = UnityEngine.Random.Range(y1, y2);
            rect.anchoredPosition = new Vector3(xRandom, yRandom);
            yield return null;
        }
        inputCorRunning = false;
        rect.anchoredPosition = originPos;
        inputText.color = new Color(0,0,0, 1);
    }

    public void YesButton()
    {
        soundManager.ButtonEffectPlay();
        SaveCharacter(!isModifiedCharacter);
        namingObject.SetActive(false);
        nameAskingObject.SetActive(false);
    }

    public void NoButton()
    {
        soundManager.ButtonEffectPlay();
        namingObject.SetActive(true);
        nameAskingObject.SetActive(false);
    }

    public void BackButton()
    {
        soundManager.ButtonEffectPlay();
        if (gameManager.fromPotScene)
        {
            gameManager.PotSceneLoad();
        }
        else
        {
            gameManager.HouseSceneLoad();
        }
        
    }

    /*public void RotationButton()
    {
        rotationMode = !rotationMode;
        flipMode = false;
    }*/

    public void FlipButton()
    {
        soundManager.ButtonEffectPlay();
        flipMode = !flipMode;
        //rotationMode = false;
    }
    public void ModifyButton()
    {
        soundManager.ButtonEffectPlay();
        cam.gameObject.transform.position = new Vector3(10, 0, -10);
        modifyMode = true;
    }

    public void ModifyYesButton()
    {
        soundManager.ButtonEffectPlay();
        cam.gameObject.transform.position = new Vector3(0, 0, -10);
        modifyPanel.SetActive(false);
        modifyButtonObject.SetActive(false);
        isModifiedCharacter = true;

        foreach (ComponentClass data in characterList[modifyingIndex].components)
        {
            SpawnModifyComponent(data);
        }
    }

    public void ModifyBackButton()
    {
        soundManager.ButtonEffectPlay();
        cam.gameObject.transform.position = new Vector3(0, 0, -10);

        isModifiedCharacter = false;
        modifyMode = false;
    }

    public void ModifyNoButton()
    {
        soundManager.ButtonEffectPlay();
        modifyPanel.SetActive(false);
        modifyMode = true;
    }

    public void ChooseCharacter(GameObject touchedObject)
    {
        GameObject characterObject = touchedObject;
        int characterIndex = -1;
        while(characterObject.transform.parent != null)
        {
            characterObject = characterObject.transform.parent.gameObject;
        }
        for(int i = 0; i<characterList.Count; i++)
        {
            if (characterObject == characterList[i].realGameobject)
            {
                characterIndex = i;
                break;
            }
        }
        if(characterIndex == -1)
        {
            Debug.Log("좆됐다 캐릭터를 못찾았다");
            return;
        }
        Debug.Log(characterList[characterIndex].name);
        modifyingIndex = characterIndex;
        modifyMode = false;
        string productedName = gameManager.GetCompleteWord(characterList[characterIndex].name, "\"이를", "\"를");
        modifyPanelText.text = "\"" + productedName + " 데려갈까요?";
        soundManager.CheerEffectPlay();
        modifyPanel.SetActive(true);
    }



    public void SaveCharacter(bool isNew)
    {
        Vector3 centerPosition = Vector3.zero;
        soundManager.FanfareEffectPlay();
        soundManager.ButtonEffectPlay();
        CharacterClass character;
        if (isNew)
        {
            //새로만든 캐릭터라면 이름까지 저장해야해.

            FindWholeJoint();
            character = new CharacterClass();
            character.components = activedComponent;
            character.name = nameInput;
            character.personality = (CharacterClass.Personality)UnityEngine.Random.Range(0, 4);

            if(character.personality == CharacterClass.Personality.Ggumul)
            {
                character.fishWorkRatio += 0.03f;
                character.huntWorkRatio += 0.03f;
                character.mineWorkRatio += 0.03f;
            }

            float count = 0;

            foreach (ComponentClass component in activedComponent)
            {
                count++;
                centerPosition = centerPosition + component.realGameobject.transform.position;
                if (FindData(component.name).isChild)
                {
                    count++;
                    centerPosition = centerPosition + component.realGameobject.transform.GetChild(1).position;
                }
            }
            centerPosition = centerPosition / count;


            foreach (ComponentClass component in activedComponent)
            {
                if (FindData(component.name).isChild)
                {
                    component.secondPosition = component.realGameobject.transform.GetChild(1).position - centerPosition;
                    component.secondRotation = component.realGameobject.transform.GetChild(1).eulerAngles; 
                }
                component.position = component.realGameobject.transform.position - centerPosition;
                component.rotation = component.realGameobject.transform.eulerAngles;
                component.realGameobject.SetActive(false);
                harvestedComponent.Remove(component);
            }
            saveData.characterList.Add(character);

        }
        else
        {

            //기획나오면 추가될거
            
            FindWholeJoint();
            character = characterList[modifyingIndex];
            int componentNumber = activedComponent.Count;
            character.components = activedComponent;

            foreach (ComponentClass component in activedComponent)
            {
                centerPosition = centerPosition + component.realGameobject.transform.position;
            }
            centerPosition = centerPosition / activedComponent.Count;

            for (int i = addedComponentNumber; i < activedComponent.Count; i++)
            {
                harvestedComponent.Remove(activedComponent[i]);
            }
            foreach (ComponentClass component in character.components)
            {
                if (FindData(component.name).isChild)
                {
                    component.secondPosition = component.realGameobject.transform.GetChild(1).position - centerPosition ;
                    component.secondRotation = component.realGameobject.transform.GetChild(1).eulerAngles;
                }
                component.position = component.realGameobject.transform.position - centerPosition;
                component.rotation = component.realGameobject.transform.eulerAngles;
                component.realGameobject.SetActive(false); 
            }

        }

        float Xmin = character.components[0].position.x;
        float Xmax = character.components[0].position.x;
        float Ymin = character.components[0].position.y;
        float Ymax = character.components[0].position.y;

        // 최댓값 최솟값 구하기
        for (int j = 1; j < character.components.Count; j++)
        {
            if (Xmin > character.components[j].position.x)
            {
                Xmin = character.components[j].position.x;
            }
            if (Xmax < character.components[j].position.x)
            {
                Xmax = character.components[j].position.x;
            }
            if (Ymin > character.components[j].position.y)
            {
                Ymin = character.components[j].position.y;
            }
            if (Ymax < character.components[j].position.y)
            {
                Ymax = character.components[j].position.y;
            }
        }

        // 팔, 다리, 머리카락의 경우 세컨드 포지션까지 최대 최소 구하는데 포함해준다.
        for (int j = 0; j < character.components.Count; j++)
        {
            if (gameManager.FindData(character.components[j].name).isChild)
            {
                if (Xmin > character.components[j].secondPosition.x)
                {
                    Xmin = character.components[j].secondPosition.x;
                }
                if (Xmax < character.components[j].secondPosition.x)
                {
                    Xmax = character.components[j].secondPosition.x;
                }
                if (Ymin > character.components[j].secondPosition.y)
                {
                    Ymin = character.components[j].secondPosition.y;
                }
                if (Ymax < character.components[j].secondPosition.y)
                {
                    Ymax = character.components[j].secondPosition.y;
                }
            }
        }

        float Xgap = Xmax - Xmin;
        float Ygap = Ymax - Ymin;
        character.xGap = Xgap;
        character.yGap = Ygap;
        gameManager.Save();
        //저장.

        activedComponent = new List<ComponentClass>();  //초기화;

        gameManager.HouseSceneLoad();
    }





    //저장가능한지 판별.
    public bool CanSave()
    {
        if (notAttached)
        {
            return false;
        }

        bool[] boolArray = new bool[activedComponent.Count];
        for (int i = 0; i < boolArray.Length; i++)
        {
            boolArray[i] = false;
        }
        Stack<int> indexStack = new Stack<int>();
        indexStack.Push(0);     //0을 넣고 0부터 시작.
        boolArray[0] = true;
        while (indexStack.Count != 0)
        {
            int nowIndex = indexStack.Pop();
            for (int i = 0; i < activedComponent[nowIndex].childIndexList.Count; i++)
            {
                if (!boolArray[activedComponent[nowIndex].childIndexList[i]])
                {
                    indexStack.Push(activedComponent[nowIndex].childIndexList[i]);
                    boolArray[activedComponent[nowIndex].childIndexList[i]] = true;
                }
            }

            for (int i = 0; i < activedComponent[nowIndex].childChildIndexList.Count; i++)
            {
                if (!boolArray[activedComponent[nowIndex].childChildIndexList[i]])
                {
                    indexStack.Push(activedComponent[nowIndex].childChildIndexList[i]);
                    boolArray[activedComponent[nowIndex].childChildIndexList[i]] = true;
                }

            }
        }

        for (int i = 0; i < boolArray.Length; i++)
        {
            if (boolArray[i] == false)
            {
                Debug.Log("불 어레이" + i);
                return false;
            }
        }

     
        for (int i = 0; i < activedComponent.Count; i++)
        {
            string nowName = activedComponent[i].name;
            for (int k = i + 1; k < activedComponent.Count; k++)
            {
                if (nowName != activedComponent[k].name)
                {
                    continue;
                }
                if ((activedComponent[i].realGameobject.transform.position - activedComponent[k].realGameobject.transform.position).magnitude < 0.2f)
                {
                    if((activedComponent[i].realGameobject.transform.eulerAngles - activedComponent[k].realGameobject.transform.eulerAngles).magnitude < 1.0f)
                    {
                        Debug.Log("겹치기");
                        return false;
                    }
                }
        }
        }

        return true;

        
      
    }

    public void FindWholeJoint()
    {
        for (int i = 0; i < activedComponent.Count; i++)
        {
            activedComponent[i].cover = false;
        }
        for (int i = 0; i<activedComponent.Count; i++)
        {
            List<GameObject> attachListI = attachObjectList[i];
            ComponentClass componentI = activedComponent[i];
            componentI.childIndexList.Clear();
            componentI.childJointList.Clear();
            componentI.childChildIndexList.Clear();
            componentI.childChildJointList.Clear();
            for (int k = 0; k < activedComponent.Count; k++)
            {
                if(i==k)
                {
                    continue;
                }
                List<GameObject> attachListK = attachObjectList[k];
                ComponentClass componentK = activedComponent[k];

                for (int n = 0; n < attachListI.Count; n++)
                {
                    for (int m = 0; m < attachListK.Count; m++)
                    {
                        /*
                        if(n>=1 && m >= 1)
                        {
                            if(componentI.name != "body" && componentK.name != "body")
                            {
                                continue;
                            }
                            
                        }*/
                        Vector3 delta = attachListI[n].transform.position - attachListK[m].transform.position;
                        Vector2 deltaVector2 = new Vector2(delta.x, delta.y);
                 
                        if (deltaVector2.magnitude <0.1f)
                        {
                            int joint = m;
                            if (componentK.name == "body")
                            {
                                joint = 0;
                            }
                            if(componentI.name == "body")
                            {
                                if (!componentI.childIndexList.Contains(k))
                                {
                                    componentI.childIndexList.Add(k);
                                    componentI.childJointList.Add(joint);
                                }
                            }
                            else
                            {
                                if (n == 1)
                                {
                                    //팔끝에 붙을 경우.
                                    componentI.childChildIndexList.Add(k);
                                    componentI.childChildJointList.Add(joint);
                                    if(componentI.name == "arm" || componentI.name == "leg")
                                    {
                                        if(componentK.name == "hand" || componentK.name == "foot")
                                        {
                                            componentK.cover = true;
                                            componentI.cover = true;
                                        }
                                    }
                                }
                                else
                                {
                                    //어깨에 붙을 경우
                                    componentI.childIndexList.Add(k);
                                    componentI.childJointList.Add(joint);
                                }
                            }
                           
  
                        }
                    }
                }
            }
        }
    }




    void Adjustattach(int index)
    {
        //이제 가장 가까운 관절을 찾아줘야해.
        GameObject componentObject = activedComponent[index].realGameobject;
        List<GameObject> attachListMain = attachObjectList[index];
        ComponentClass indexComponent = activedComponent[index];
        indexComponent.attached = false;
        Vector3 leastDelta = Vector3.zero;
        notAttached = true;
        for (int i = 0; i < activedComponent.Count; i++)
        {
            if (i == index)
            {
                continue;
            }
            List<GameObject> attachListK = attachObjectList[i];
            ComponentClass nowComponent = activedComponent[i];

            for(int j = 0; j<attachListMain.Count; j++)
            {
                for(int k = 0; k < attachListK.Count; k++)
                {
                    if(indexComponent.cover)
                    {
                        continue;
                    }
                    if(nowComponent.cover)
                    {
                        if(attachListK.Count == 2)
                        {
                            if(k==1)
                                continue;
                        }
                        else
                        {
                            continue;
                        }
                    
                    }
                    Vector3 delta = attachListMain[j].transform.position - attachListK[k].transform.position;
                    Vector2 deltaVector2 = new Vector2(delta.x, delta.y);
                    Vector2 leastDeltaVector2 = new Vector2(leastDelta.x, leastDelta.y);
                    if (leastDelta == Vector3.zero)
                    {
                        leastDelta = delta;
                    }
                    if (deltaVector2.sqrMagnitude < leastDeltaVector2.sqrMagnitude)
                    {
                        leastDelta = delta;
                    }
                }
            }

        }

        Vector3 convert = new Vector3(leastDelta.x, leastDelta.y,0);
        if(convert.sqrMagnitude < 2.0f)
        {
            componentObject.transform.position = componentObject.transform.position - convert;
            if (convert != Vector3.zero)
            {
                soundManager.ComposeEffectPlay();
                notAttached = false;
            }
        }
    }



    private void Update()
    {
        if (Input.GetMouseButtonUp(0))    //터치끝났을 때 adjustattach
        {
            if(!flipMode)
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {
                    touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                    //Debug.Log(touchedObject);
                    for (int i = 0; i < activedComponent.Count; i++)
                    {

                        if (activedComponent[i].realGameobject == touchedObject)
                        {
                            Adjustattach(i);
                            FindWholeJoint();
                            saveButton.SetActive(CanSave());
                            break;
                        }
                    }
                }
            }
           
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (modifyMode)
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {
                    touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                    ChooseCharacter(touchedObject);

                }
            }
        }

        for (int i = 0; i < timerList.Count; i++)
        {
            characterObjectList[i].transform.position = Vector3.Lerp(startPosList[i], randomPosList[i], timerList[i] / randomTimeList[i]);
            timerList[i] += Time.deltaTime;
            if (timerList[i] > randomTimeList[i])
            {
                timerList[i] = 0;
                randomTimeList[i] = UnityEngine.Random.Range(1f, 2f);
                startPosList[i] = randomPosList[i];
                randomPosList[i] = new Vector3(UnityEngine.Random.Range(7.5f, 12.5f), UnityEngine.Random.Range(-3f, 0f), 0);
            }
        }
        //-2.5~2.5, 0~-3

        for (int i = 0; i < rotationList.Count; i++)
        {
            rotatingObjectList[i].transform.eulerAngles = Vector3.Lerp(startAngleList[i], randomAngleList[i], rotationList[i] / randomRotateTimeList[i]);
            rotationList[i] += Time.deltaTime;
            if (rotationList[i] > randomRotateTimeList[i])
            {
                rotationList[i] = 0;
                randomRotateTimeList[i] = UnityEngine.Random.Range(1f, 2f);
                startAngleList[i] = randomAngleList[i];
                if (startAngleList[i].y == 0)
                {
                    randomAngleList[i] = new Vector3(0, 0, originAngleList[i] + UnityEngine.Random.Range(-30, 30));
                }
                else
                {
                    randomAngleList[i] = new Vector3(0, 180, originAngleList[i] + UnityEngine.Random.Range(-30, 30));
                }
            }
        }
    }

}
