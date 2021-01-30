using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;                 //캐릭터를 저장해줘야하니까.
    [SerializeField]
    List<ComponentClass> harvestedComponent;//버튼형태로 있는 부위.
    [SerializeField]
    List<ComponentClass> activedComponent;  //단상위에 올라가있는 부위
    List<List<GameObject>> attachObjectList;
    WholeComponents wholeComponents;
    public RectTransform contentRect;       //시작값 700. 하나 늘어날떄마다 -400
    public GameObject buttonObject;         //시작값  0,-74f/  하나늘어날때마다 x만 +400
    List<GameObject> buttonList;
    List<int> removedButtonList;        //사라진 버튼의 인덱스 리스트
    CharacterClass loadedCharacter;     //나중에 생길 기존캐릭터에 요소 붙이늰거
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

    public bool rotationMode;
    public bool flipMode;


    private void Start()
    {
        namingObject.SetActive(false);
        nameAskingObject.SetActive(false);
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        harvestedComponent = gameManager.saveData.owningComponentList;
        attachObjectList = new List<List<GameObject>>();
        buttonList = new List<GameObject>();
        removedButtonList = new List<int>();
        rotationMode = false;
        //초기화

        contentRect.anchoredPosition = new Vector2(0, 0);   //자꾸 이거 움직임;; 위치 고정 안해주면 지맘대로 위치가 바껴요
        contentRect.sizeDelta = new Vector2(400 * harvestedComponent.Count, 300);
        //contentRect 사이즈 조정을 해줘야 좌우로 움직일 수 있다.
        
        
        for(int i = 0; i< harvestedComponent.Count; i++)
        {
            ComponentDataClass componentData = FindData(harvestedComponent[i].name);
            GameObject inst = Instantiate(buttonObject, contentRect.transform);
            buttonList.Add(inst);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400*harvestedComponent.Count/2+ i * 400, -74f);
            //Image image = inst.GetComponent<Image>();
            //image.sprite = componentData.componentSpriteArray[0];
            Text text = inst.GetComponentInChildren<Text>();
            text.text = componentData.name;
            string name = componentData.name;
            int index = i;      //delegate에 그냥 i를 넣으면 되는데, 이상하게 int가 callbyRef로 넘어간다.
            Button button = inst.GetComponent<Button>();
            button.onClick.AddListener(delegate { SpawnComponent(name, index); });
            //버튼만드는 for문
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

    //버튼을 누를 때 
    public void SpawnComponent(string name,int buttonIndex)
    {
        int changedIndex = buttonIndex;
        ComponentDataClass data = FindData(name);
        GameObject obj = Resources.Load<GameObject>("Components/Complete/" + name);
        GameObject inst = Instantiate(obj,parentObject.transform);
        inst.transform.eulerAngles = Vector3.zero;
        List<GameObject> objectList = new List<GameObject>();
        attachObjectList.Add(objectList);
        for(int i = 0; i< data.attachPosition.Count; i++)
        {
            GameObject attachInst = Instantiate(attachObject, inst.transform);
            objectList.Add(attachInst);
            attachInst.transform.localPosition = new Vector3(data.attachPosition[i].x, data.attachPosition[i].y, 0);
        }
        

        inst.transform.position = new Vector3(0, 0, 10);
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

        contentRect.sizeDelta = new Vector2(400 * harvestedComponent.Count, 300);

        for (int i  = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-400 * harvestedComponent.Count / 2 + i * 400, -74f);
        }
        saveButton.SetActive(false);
    }

    public void SaveButton()
    {
        if(loadedCharacter != null)
        {
            SaveCharacter(false);
            return;
        }
        else
        {
            namingObject.SetActive(true);
        }
    }

    public void NameButton()
    {
        namingObject.SetActive(false);
        nameAskingObject.SetActive(true);
        nameInput = nameInputField.text;
        nameAskingText.text = "\"" + nameInputField.text + "\"(이)가 맞나요?";
    }

    public void YesButton()
    {
        SaveCharacter(true);
        namingObject.SetActive(false);
        nameAskingObject.SetActive(false);
    }

    public void NoButton()
    {
        namingObject.SetActive(true);
        nameAskingObject.SetActive(false);
    }

    public void BackButton()
    {
        gameManager.PotSceneLoad();
    }

    public void RotationButton()
    {
        rotationMode = !rotationMode;
        flipMode = false;
    }
    public void FlipButton()
    {
        flipMode = !flipMode;
        rotationMode = false;
    }

    public void SaveCharacter(bool isNew)
    {
        if (isNew)
        {
            //새로만든 캐릭터라면 이름까지 저장해야해.
            FindWholeJoint();
            CharacterClass character = new CharacterClass();
            character.components = activedComponent;
            character.name = nameInput;
            character.personality = (CharacterClass.Personality)UnityEngine.Random.Range(0, 3);
            foreach (ComponentClass component in activedComponent)
            {
                component.position = component.realGameobject.transform.localPosition;
                component.rotation = component.realGameobject.transform.eulerAngles;
                component.realGameobject.SetActive(false);
                harvestedComponent.Remove(component);
            }
            saveData.characterList.Add(character);
            gameManager.Save();
            //저장.

            activedComponent = new List<ComponentClass>();  //초기화;

        }
        else
        {
            //기획나오면 추가될거
        }
    }

    //저장가능한지 판별.
    public bool CanSave()
    {
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
                if ((activedComponent[i].realGameobject.transform.position - activedComponent[k].realGameobject.transform.position).magnitude < 1.0f)
                {
                    if((activedComponent[i].realGameobject.transform.eulerAngles - activedComponent[k].realGameobject.transform.eulerAngles).magnitude < 1.0f)
                    {
                        return false;
                    }
                }
        }
        }
      

        return true;

        
      
    }

    public void FindWholeJoint()
    {
        for(int i = 0; i<activedComponent.Count; i++)
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
                        if(n>=1 && m >= 1)
                        {
                            continue;
                        }
                        Vector3 delta = attachListI[n].transform.position - attachListK[m].transform.position;
                        Vector2 deltaVector2 = new Vector2(delta.x, delta.y);

                        if (deltaVector2.magnitude < 0.5f)
                        {
                            if(componentI.name == "body")
                            {
                                componentI.childIndexList.Add(k);
                                componentI.childJointList.Add(m);
                            }
                            else
                            {
                                if (n == 1)
                                {
                                    //팔끝에 붙을 경우.
                                    componentI.childChildIndexList.Add(k);
                                    componentI.childChildJointList.Add(m);

                                }
                                else
                                {
                                    //어깨에 붙을 경우
                                    componentI.childIndexList.Add(k);
                                    componentI.childJointList.Add(m);
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
        Vector3 leastDelta = Vector3.zero;
        for (int i = 0; i < activedComponent.Count; i++)
        {
            if (i == index)
            {
                continue;
            }
            List<GameObject> attachListK = attachObjectList[i];


            for(int j = 0; j<attachListMain.Count; j++)
            {
                for(int k = 0; k < attachListK.Count; k++)
                {
                    if(j==1 && k == 1)
                    {
                        continue;
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
        Vector2 convert = new Vector2(leastDelta.x, leastDelta.y);
        if(convert.sqrMagnitude < 2.0f)
        {
            componentObject.transform.position = componentObject.transform.position - leastDelta;
        }
    }



    private void Update()
    {
        if (Input.GetMouseButtonUp(0))    //터치끝났을 때 adjustattach
        {
            if(!flipMode && !rotationMode)
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {


                    touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                    Debug.Log(touchedObject);
                    for (int i = 0; i < activedComponent.Count; i++)
                    {

                        if (activedComponent[i].realGameobject == touchedObject)
                        {
                            Debug.Log("몇번 실행되나 " + i);
                            Adjustattach(i);
                            FindWholeJoint();
                            saveButton.SetActive(CanSave());
                            break;
                        }
                    }

                }
            }

        }
    }

}
