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
    List<List<GameObject>> jointObjectList;
    WholeComponents wholeComponents;
    public RectTransform contentRect;       //시작값 700. 하나 늘어날떄마다 -400
    public GameObject buttonObject;         //시작값  0,-74f/  하나늘어날때마다 x만 +400
    List<GameObject> buttonList;
    List<int> removedButtonList;        //사라진 버튼의 인덱스 리스트
    CharacterClass loadedCharacter;     //나중에 생길 기존캐릭터에 요소 붙이늰거
    public InputField nameInputField;       //이름 인풋필드
    string nameInput;                       //이름 넣은거
    public GameObject parentObject;         //부위들의 Parent가 되는 오브젝트

    public GameObject NamingObject;
    public GameObject NameAskingObject;
    public Text NameAskingText;            //""(이)가 맞나요?

    public GameObject jointObject;         //관절 오브젝트


    private void Start()
    {
        NamingObject.SetActive(false);
        NameAskingObject.SetActive(false);
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        harvestedComponent = gameManager.saveData.owningComponentList;
        jointObjectList = new List<List<GameObject>>();
        //초기화
        contentRect.anchoredPosition = new Vector2(0, 0);
        contentRect.sizeDelta = new Vector2(400 * harvestedComponent.Count, 300);
        //contentRect 사이즈 조정을 해줘야 좌우로 움직일 수 있다.
        buttonList = new List<GameObject>();
        removedButtonList = new List<int>();
        for(int i = 0; i< harvestedComponent.Count; i++)
        {
            ComponentDataClass componentData = FindData(harvestedComponent[i].name);
            GameObject inst = Instantiate(buttonObject, contentRect.transform);
            buttonList.Add(inst);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400*harvestedComponent.Count/2+ i * 400, -74f);
            Image image = inst.GetComponent<Image>();
            image.sprite = componentData.componentSprite;
            Text text = inst.GetComponentInChildren<Text>();
            text.text = componentData.name;
            string name = componentData.name;
            int index = i;
            Button button = inst.GetComponent<Button>();
            button.onClick.AddListener(delegate { SpawnComponent(name, index); });
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

    public void SpawnComponent(string name,int buttonIndex)
    {
        Debug.Log(name + buttonIndex);
        int changedIndex = buttonIndex;
        ComponentDataClass data = FindData(name);
        GameObject obj = Resources.Load<GameObject>("Components/" + name);
        GameObject inst = Instantiate(obj,parentObject.transform);
        List<GameObject> objectList = new List<GameObject>();
        jointObjectList.Add(objectList);
        for(int i = 0; i< data.jointPosition.Count; i++)
        {
            GameObject jointInst = Instantiate(jointObject, inst.transform);
            objectList.Add(jointInst);
            jointInst.transform.localPosition = new Vector3(data.jointPosition[i].x, data.jointPosition[i].y, 5);
        }
        

        inst.transform.position = new Vector3(0, 0, 10);
        inst.AddComponent<DragAttach>();
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

        ComponentClass component = harvestedComponent[changedIndex];
        component.realGameobject = inst;
        activedComponent.Add(component);
        harvestedComponent.Remove(component);

        contentRect.sizeDelta = new Vector2(400 * harvestedComponent.Count, 300);

        for (int i  = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-400 * harvestedComponent.Count / 2 + i * 400, -74f);
        }
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
            NamingObject.SetActive(true);
        }
    }

    public void NameButton()
    {
        NamingObject.SetActive(false);
        NameAskingObject.SetActive(true);
        nameInput = nameInputField.text;
        NameAskingText.text = "\"" + nameInputField.text + "\"(이)가 맞나요?";
    }

    public void YesButton()
    {
        SaveCharacter(true);
        NamingObject.SetActive(false);
        NameAskingObject.SetActive(false);
    }

    public void NoButton()
    {
        NamingObject.SetActive(true);
        NameAskingObject.SetActive(false);
    }

    public void BackButton()
    {
        gameManager.PotSceneLoad();
    }

    public void SaveCharacter(bool isNew)
    {
        if (isNew)
        {
            CharacterClass character = new CharacterClass();
            character.components = activedComponent;
            character.name = nameInput;
            character.personality = (CharacterClass.Personality)UnityEngine.Random.Range(0, 3);
            foreach (ComponentClass component in activedComponent)
            {
                component.position = component.realGameobject.transform.localPosition;
                component.realGameobject.SetActive(false);
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

    void AdjustJoint()
    {
        for(int i = 0; i < activedComponent.Count-1; i++)
        {
            GameObject realObjectI = activedComponent[i].realGameobject;
            List<GameObject> jointListI = jointObjectList[i];
            for(int j = i+1;j < activedComponent.Count; j++)
            {
                List<GameObject> jointListJ = jointObjectList[j];

                foreach(GameObject objI in jointListI)
                {
                    foreach(GameObject objJ in jointListJ)
                    {
                        Vector3 delta = objI.transform.position - objJ.transform.position;
                        if (delta.magnitude < 0.5f)
                        {
                            realObjectI.transform.position = realObjectI.transform.position - delta;
                        }
                    }
                }

            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //뗄 때마다 조인트를 찾아줘야한다.
            //어떤 거를 떼는지부터 알아야한다.
            AdjustJoint();
        }
    }

}
