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
    List<string> boughtNameList;
    List<string> boughtDateList;
    ComponentClass[] potList;
    int leftPot;

    int coinAmount;

    int isSeed1Sold;
    int isSeed2Sold;
    int isSeed3Sold;
    int isSeed4Sold;
    int isSeed5Sold;
    int isSeed6Sold;
    int isSeed7Sold;
    int isSeed8Sold;
    int isSeed9Sold;
    int isSeed10Sold;
    int isToySold;
    int isBeanBagSold;

    public Text coinAmountText;

    public Text[] seedPriceTextArray;


    public Text toyPrice;
    public Text beanBagPrice;


    public Button[] buySeedButtonArray;
   

    string[] namesArray = { "eye", "nose", "mouth", "ear", "hand", "arm", "foot", "leg", "body", "hair" };
    int[] priceArray = { 50, 50, 50, 50, 70, 100, 70, 100, 150, 30 };

    public Button buyToyButton;
    public Button buyBeanBagButton;

    public Image catEyeImage;
    public Image catTailImage;
    public Sprite[] catEyeList;
    public Sprite[] catTailList;
    public Image[] potImageArray;
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

        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        boughtNameList = saveData.boughtNameList;
        boughtDateList = saveData.boughtDateList;
        soundManager = SoundManager.inst;
        coinAmount = saveData.coin;
        potList = saveData.potList;
        leftPotArray = new bool[3];
        leftPot = 0;

        for(int i = 0; i<potList.Length; i++)
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

        for(int i = 0; i < 3; i++)
        {
            if(leftPotArray[i] == false)
            {
                potImageArray[i].color = new Color(1, 1, 1, 0.5f);
            }
        }





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
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if (saveData.coin >= priceArray[i])
                {
                    buySeedButtonArray[i].interactable = true;
                }
                else
                {
                    buySeedButtonArray[i].interactable = false;
                }
            }
        }

        if (saveData.coin >= 700 && !saveData.trainSelled)
            buyToyButton.interactable = true;
        else
            buyToyButton.interactable = false;

        if (saveData.coin >= 800 && !saveData.chairSelled)
            buyBeanBagButton.interactable = true;
        else
            buyBeanBagButton.interactable = false;
    }

    public void buySeed(int index)
    {
        saveData.coin -= priceArray[index];
        seedPriceTextArray[index].text = "구매가 완료되었습니다!";
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
        saveData.coin -= 700;
        saveData.trainSelled = true;
        toyPrice.text = "구매가 완료되었습니다!";
        buyToyButton.gameObject.SetActive(false);
        soundManager.ButtonEffectPlay();

    }
    public void buyChair()
    {
        saveData.coin -= 800;
        saveData.chairSelled = true;
        beanBagPrice.text = "구매가 완료되었습니다!";
        buyBeanBagButton.gameObject.SetActive(false);
        soundManager.ButtonEffectPlay();
    }

    public void exitStore()
    {
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

        for (int i = 0; i < 3; i++)
        {
            if (leftPotArray[i] == false)
            {
                potImageArray[i].color = new Color(1, 1, 1, 0.5f);
            }
        }

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
