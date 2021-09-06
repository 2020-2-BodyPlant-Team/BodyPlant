using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{

    GameManager gameManager;
    SoundManager soundManager;
    SaveDataClass saveData;
    List<ComponentDataClass> componentDataList;

    List<string> boughtNameList;
    List<string> boughtDateList;
    ComponentClass[] potList;
    int leftPot;

    int coinAmount;

    public Text coinAmountText;
    public Text[] componentCoinText;
    public Text trainText;
    public Text chairText;

    Button[] buySeedButtonArray;
    public Transform[] seedImageArray;
    GameObject[] checkObjectArray;
    public GameObject fullPotObject;
    public GameObject noMoneyObject;
   

    string[] namesArray = { "arm", "leg", "mouth", "nose", "eye", "hair", "foot", "hand", "ear","body" };
    int[] priceArray;// = { 50, 50, 50, 50, 70, 100, 70, 100, 150, 30 };
    int chairPrice;
    int trainPrice;

    public Button buyToyButton;
    public GameObject toyCheckObject;
    public Button buySofaButton;
    public GameObject sofaCheckObject;

    public Image catEyeImage;
    public Image catTailImage;
    public Sprite[] catEyeList;
    public Sprite[] catTailList;
    bool[] leftPotArray;
    /*
     0 eye
     1 nose
     2 lip
     3 ear
     4 hand
     5 arm
     6 foot
     7 leg
     8 body
     9 hair
    */

    IEnumerator CatEyeCoroutine()
    {
        int eyeIndex = 0;
        float frame = 0.125f;
        bool goingPlus = true;
        while (true)
        {
            catEyeImage.sprite = catEyeList[eyeIndex];
            if(eyeIndex == 0)
            {
                frame = 3f;
                catEyeImage.gameObject.SetActive(false);
            }
            else
            {
                catEyeImage.gameObject.SetActive(true);
            }

            if (goingPlus)
                eyeIndex++;
            else
                eyeIndex--;
            yield return new WaitForSeconds(frame);
            frame = 0.125f;
            if(eyeIndex == catEyeList.Length-1)
            {
                goingPlus = false;
            }
            if(eyeIndex == 0)
            {
                goingPlus = true;
            }

        }
    }

    IEnumerator CatTailCoroutine()
    {
        int tailIndex = 0;
        float frame = 0.125f;
        while (true)
        {
            catTailImage.sprite = catTailList[tailIndex];
            if (tailIndex == 0)
            {
                frame = 5f;
            }
            tailIndex++;
            yield return new WaitForSeconds(frame);
            frame = 0.125f;
            if (tailIndex >= catTailList.Length)
            {
                tailIndex = 0;
            }

        }
    }

    void Start()
    {
        OptionManager.singleTon.OptionButtonActive(false);
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        componentDataList = gameManager.wholeComponents.componentList;
        boughtNameList = saveData.boughtNameList;
        boughtDateList = saveData.boughtDateList;
        soundManager = SoundManager.inst;
        coinAmount = saveData.coin;
        potList = saveData.potList;
        leftPotArray = new bool[3];
        leftPot = 0;
        priceArray = new int[10];

        noMoneyObject.SetActive(false);
        checkObjectArray = new GameObject[seedImageArray.Length];
        buySeedButtonArray = new Button[seedImageArray.Length];
        componentCoinText = new Text[seedImageArray.Length];
        for (int i = 0; i < seedImageArray.Length; i++)
        {
            checkObjectArray[i] = seedImageArray[i].GetChild(1).gameObject;
            componentCoinText[i] = seedImageArray[i].GetChild(2).GetComponent<Text>();
            buySeedButtonArray[i] = seedImageArray[i].GetChild(4).GetComponent<Button>();
            checkObjectArray[i].SetActive(false);
        }

        bool puksinAvail = false;
        List<CharacterClass> characterList = saveData.characterList;
        for(int i = 0; i < characterList.Count; i++)
        {
            if(characterList[i].personality == CharacterClass.Personality.Puksin)
            {
                puksinAvail = true;
                break;
            }
        }
        if (!puksinAvail)
        {
            characterList = saveData.fishCharacterList;
            for (int i = 0; i < characterList.Count; i++)
            {
                if (characterList[i].personality == CharacterClass.Personality.Puksin)
                {
                    puksinAvail = true;
                    break;
                }
            }
        }
        if (!puksinAvail)
        {
            characterList = saveData.huntCharacterList;
            for (int i = 0; i < characterList.Count; i++)
            {
                if (characterList[i].personality == CharacterClass.Personality.Puksin)
                {
                    puksinAvail = true;
                    break;
                }
            }
        }
        if (!puksinAvail)
        {
            characterList = saveData.mineCharacterList;
            for (int i = 0; i < characterList.Count; i++)
            {
                if (characterList[i].personality == CharacterClass.Personality.Puksin)
                {
                    puksinAvail = true;
                    break;
                }
            }
        }
        if (puksinAvail)
        {
            for (int i = 0; i < componentDataList.Count; i++)
            {
                priceArray[i] = componentDataList[i].discountPrice;
                componentCoinText[i].text = componentDataList[i].discountPrice.ToString();
            }
            chairPrice = 1900;
            trainPrice = 2800;
        }
        else
        {
            for (int i = 0; i < componentDataList.Count; i++)
            {
                priceArray[i] = componentDataList[i].price;
                componentCoinText[i].text = componentDataList[i].price.ToString();
            }

            chairPrice = 2000;
            trainPrice = 3000;
        }
        chairText.text = chairPrice.ToString();
        trainText.text = trainPrice.ToString();




        if (saveData.trainSelled)
        {
            toyCheckObject.SetActive(true);
            buyToyButton.interactable = false;
        }

        if (saveData.chairSelled)
        {
            sofaCheckObject.SetActive(true);
            buySofaButton.interactable = false;
        }
        for (int i = 0; i<potList.Length; i++)
        {
            leftPotArray[i] = false;
            if(potList[i].name == "null")
            {
                leftPot++;
                leftPotArray[i] = true;
            }
        }
        if (boughtNameList.Count > 0)
        {
            for (int i = 0; i < boughtNameList.Count; i++)
            {
                if (leftPotArray[0] == true)
                {
                    leftPotArray[0] = false;
                }
                else if (leftPotArray[1] == true)
                {
                    leftPotArray[1] = false;
                }
                else if (leftPotArray[2] == true)
                {
                    leftPotArray[2] = false;
                }
            }
        }
        leftPot -= boughtNameList.Count;

        //for(int i = 0; i < 3; i++)
        //{
        //    if(leftPotArray[i] == false)
        //    {
        //        potImageArray[i].color = new Color(1, 1, 1, 0.5f);
        //    }
        //}





        StartCoroutine(CatEyeCoroutine());
        StartCoroutine(CatTailCoroutine());

        BuyUpdate();

    }

    //adManager에서도 불러옴
    public void BuyUpdate()
    {
        coinAmountText.text = saveData.coin.ToString() ;


        if (leftPot <= 0)
        {
            
            for (int i = 0; i < 10; i++)
            {
                buySeedButtonArray[i].interactable = false;
            }
            fullPotObject.SetActive(true);

        }
        //else
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        if (saveData.coin >= priceArray[i])
        //        {
        //            buySeedButtonArray[i].interactable = true;
        //        }
        //        else
        //        {
        //            buySeedButtonArray[i].interactable = false;
        //        }
        //    }
        //}

        if (saveData.coin >= trainPrice && !saveData.trainSelled)
            buyToyButton.interactable = true;
        else
            buyToyButton.interactable = false;

        if (saveData.coin >= chairPrice && !saveData.chairSelled)
            buySofaButton.interactable = true;
        else
            buySofaButton.interactable = false;
    }

    public void buySeed(int index)
    {
        if(saveData.tutorialOrder == 1)
        {
            FindObjectOfType<TutorialMngInStore>().isSeedBtnClicked = true;
        }
        if (saveData.coin < priceArray[index])
        {
            noMoneyObject.SetActive(true);
            return;
        }
        else
        {
            noMoneyObject.SetActive(false);
        }
        saveData.coin -= priceArray[index];
        checkObjectArray[index].SetActive(true);
        boughtNameList.Add(namesArray[index]);
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        BuyUpdate();

        soundManager.ButtonEffectPlay();
    }



    public void buyTrain()
    {
        if (saveData.coin < trainPrice)
        {
            noMoneyObject.SetActive(true);
            return;
        }
        else
        {
            noMoneyObject.SetActive(false);
        }
        saveData.coin -= trainPrice;
        saveData.trainSelled = true;
        toyCheckObject.SetActive(true);
        buyToyButton.gameObject.SetActive(false);
        soundManager.ButtonEffectPlay();
        gameManager.Save();
        BuyUpdate();
    }
    public void buyChair()
    {
        if (saveData.coin < chairPrice)
        {
            noMoneyObject.SetActive(true);
            return;
        }
        else
        {
            noMoneyObject.SetActive(false);
        }
        saveData.coin -= chairPrice;
        saveData.chairSelled = true;
        sofaCheckObject.SetActive(true);
        buySofaButton.gameObject.SetActive(false);
        soundManager.ButtonEffectPlay();
        gameManager.Save();
        BuyUpdate();
    }

    public void exitStore()
    {
        if(saveData.tutorialOrder == 1)
        {
            saveData.tutorialOrder++;
            gameManager.Save();
        }
        
        soundManager.ButtonEffectPlay();
        gameManager.BackToStoreScene();

    }

    void PotAlphaChange()
    {
        for (int i = 0; i < boughtNameList.Count; i++)
        {
            if (leftPotArray[0] == true)
            {
                leftPotArray[0] = false;
            }
            else if (leftPotArray[1] == true)
            {
                leftPotArray[1] = false;
            }
            else if (leftPotArray[2] == true)
            {
                leftPotArray[2] = false;
            }
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    if (leftPotArray[i] == false)
        //    {
        //        potImageArray[i].color = new Color(1, 1, 1, 0.5f);
        //    }
        //}

    }

    /*
    public void resetPlayerPrefs()
    {
        coinAmount = 0;
        buySeed1Button.gameObject.SetActive(true);
        buySeed2Button.gameObject.SetActive(true);
        buySeed3Button.gameObject.SetActive(true);
        buySeed4Button.gameObject.SetActive(true);
        buySeed5Button.gameObject.SetActive(true);
        buySeed6Button.gameObject.SetActive(true);
        buySeed7Button.gameObject.SetActive(true);
        buySeed8Button.gameObject.SetActive(true);
        buySeed9Button.gameObject.SetActive(true);
        buySeed10Button.gameObject.SetActive(true);
        buyToyButton.gameObject.SetActive(true);
        buyBeanBagButton.gameObject.SetActive(true);

        seed1Price.text = "50G";
        seed2Price.text = "50G";
        seed3Price.text = "50G";
        seed4Price.text = "50G";
        seed5Price.text = "70G";
        seed6Price.text = "100G";
        seed7Price.text = "70G";
        seed8Price.text = "100G";
        seed9Price.text = "150G";
        seed10Price.text = "30G";

        toyPrice.text = "700G";
        beanBagPrice.text = "800G";

        PlayerPrefs.DeleteAll();
    }*/

}
