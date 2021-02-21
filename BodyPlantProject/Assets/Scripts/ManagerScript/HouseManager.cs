using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;

    [SerializeField]
    List<CharacterClass> characterList;
    public CharacterMover characterMover;

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

        for (int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i],i);
        }
    }

    void Update()
    {
        characterMover.PositionUpdate();
        characterMover.RotationUpdate();

    }

}
