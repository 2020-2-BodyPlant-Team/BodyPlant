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

        for (int i = 0; i < characterList.Count; i++)
        {
            characterMover.SpawnCharacter(characterList[i],i);
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

    void Update()
    {
        characterMover.PositionUpdate();
        characterMover.RotationUpdate();

    }

}
