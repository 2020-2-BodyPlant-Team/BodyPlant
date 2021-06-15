using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WorkCharacterManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<CharacterClass> characterList;

    public GameObject buttonPrefab;
    List<int> restIndex;    //이미 눌린 버튼의 인덱스.]
    public CharacterMover characterMover;
    List<GameObject> buttonList;
    public GameObject bringButton;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        restIndex = new List<int>();
        buttonList = new List<GameObject>();
    }

    public void SetCharacterList(List<CharacterClass> list)
    {
        characterList = list;

        for(int i =0; i<characterList.Count; i++)
        {
            GameObject obj = characterList[i].realGameobject;
            GameObject buttonObject = Instantiate(buttonPrefab);
            characterList[i].getOutButton = buttonObject;
            buttonObject.GetComponent<Canvas>().sortingOrder = 3;
            buttonObject.transform.SetParent(obj.transform);
            buttonList.Add(buttonObject);
            buttonObject.transform.localPosition = Vector3.zero;
            buttonObject.SetActive(false);
            int index = i;
            buttonObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { RestButton(index); });
        }
    }

    public void RestButton(int index)
    {
        int minus = 0;
        for(int i = 0; i< restIndex.Count; i++)
        {
            if(index > restIndex[i])
            {
                minus++;
            }
        }
        restIndex.Add(index);
        index -= minus;

        CharacterClass item = characterList[index];
        characterList.Remove(item);

        gameManager.UpdateLoveness();

        saveData.characterList.Add(item);
        item.loveStartTime = DateTime.Now.ToString();

        item.realGameobject.SetActive(false);
        bringButton.SetActive(true);

        gameManager.Save();
    }

    public void ChooseCharacter(CharacterClass character,GameObject touched)
    {
        if (character == null)
        {
            return;
        }
        for (int i = 0; i < characterList.Count; i++)
        {
            if(character == characterList[i])
            {
                /*
                int plus = 0;
                int index = i;

                for (int j = 0; j < restIndex.Count; j++)
                {
                    if (index >= restIndex[j])
                    {
                        plus++;
                    }
                }
                index += plus;

                break;*/

                characterList[i].getOutButton.SetActive(true);


                //buttonList[index].SetActive(true);
                StartCoroutine(ButtonFalse(characterList[i].getOutButton));

            }
        }
    }

    IEnumerator ButtonFalse(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                ChooseCharacter(characterMover.ChooseCharacter(touchedObject),touchedObject);

            }
        }

        for(int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].transform.rotation = Quaternion.identity;
        }

        
    }
}
