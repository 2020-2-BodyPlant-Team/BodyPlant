using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{

    GameManager gameManager;
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
    public Text seed1Price;
    public Text seed2Price;
    public Text seed3Price;
    public Text seed4Price;
    public Text seed5Price;
    public Text seed6Price;
    public Text seed7Price;
    public Text seed8Price;
    public Text seed9Price;
    public Text seed10Price;
    public Text toyPrice;
    public Text beanBagPrice;

    public Button buySeed1Button;
    public Button buySeed2Button;
    public Button buySeed3Button;
    public Button buySeed4Button;
    public Button buySeed5Button;
    public Button buySeed6Button;
    public Button buySeed7Button;
    public Button buySeed8Button;
    public Button buySeed9Button;
    public Button buySeed10Button;
   

    List<Button> seedButtonList;

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




        seedButtonList = new List<Button>();

        seedButtonList.Add(buySeed1Button);
        seedButtonList.Add(buySeed2Button);
        seedButtonList.Add(buySeed3Button);
        seedButtonList.Add(buySeed4Button);
        seedButtonList.Add(buySeed5Button);
        seedButtonList.Add(buySeed6Button);
        seedButtonList.Add(buySeed7Button);
        seedButtonList.Add(buySeed8Button);
        seedButtonList.Add(buySeed9Button);
        seedButtonList.Add(buySeed10Button);

        StartCoroutine(CatEyeCoroutine());
        StartCoroutine(CatTailCoroutine());

    }

    void Update()
    {
        coinAmountText.text = saveData.coin.ToString() ;




        if (saveData.coin >= 50)
            buySeed1Button.interactable = true;
        else
            buySeed1Button.interactable = false;

        if (saveData.coin >= 50)
            buySeed2Button.interactable = true;
        else
            buySeed2Button.interactable = false;

     
        if (saveData.coin >= 50)
            buySeed3Button.interactable = true;
        else
            buySeed3Button.interactable = false;

  
        if (saveData.coin >= 50)
            buySeed4Button.interactable = true;
        else
            buySeed4Button.interactable = false;


        if (saveData.coin >= 70)
            buySeed5Button.interactable = true;
        else
            buySeed5Button.interactable = false;

       
        if (saveData.coin >= 100)
            buySeed6Button.interactable = true;
        else
            buySeed6Button.interactable = false;

        if (saveData.coin >= 70)
            buySeed7Button.interactable = true;
        else
            buySeed7Button.interactable = false;

      
        if (saveData.coin >= 100)
            buySeed8Button.interactable = true;
        else
            buySeed8Button.interactable = false;

        if (saveData.coin >= 150 )
            buySeed9Button.interactable = true;
        else
            buySeed9Button.interactable = false;


        if (saveData.coin >= 30)
            buySeed10Button.interactable = true;
        else
            buySeed10Button.interactable = false;


        if (leftPot <= 0)
        {
            foreach (Button button in seedButtonList)
            {
                button.interactable = false;
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

    public void buySeed1()
    {
        saveData.coin -= 50;
        
        seed1Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("eye");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed1Button.gameObject.SetActive(true);
    }

    public void buySeed2()
    {
        saveData.coin -= 50;
      
        seed2Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("nose");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed2Button.gameObject.SetActive(true);

    }
    public void buySeed3()
    {
        saveData.coin -= 50;
       
        seed3Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("mouth");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed3Button.gameObject.SetActive(true);

    }
    public void buySeed4()
    {
        saveData.coin -= 50;
    
        seed4Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("ear");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed4Button.gameObject.SetActive(true);

    }
    public void buySeed5()
    {
        saveData.coin -= 70;
       
        seed5Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("hand");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed5Button.gameObject.SetActive(true);
    }
    public void buySeed6()
    {
        saveData.coin -= 100;
        
        seed6Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("arm");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed6Button.gameObject.SetActive(true);
    }
    public void buySeed7()
    {
        saveData.coin -= 70;
       
        seed7Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("foot");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed7Button.gameObject.SetActive(true);
    }
    public void buySeed8()
    {
        saveData.coin -= 100;
   
        seed8Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("leg");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed8Button.gameObject.SetActive(true);
    }
    public void buySeed9()
    {
        saveData.coin -= 150;
   
        seed9Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("body");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed9Button.gameObject.SetActive(true);
    }
    public void buySeed10()
    {
        saveData.coin -= 30;
     
        seed10Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("hair");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        PotAlphaChange();
        gameManager.Save();
        buySeed10Button.gameObject.SetActive(true);
    }


    public void buyTrain()
    {
        saveData.coin -= 700;
        saveData.trainSelled = true;
        toyPrice.text = "구매가 완료되었습니다!";
        buyToyButton.gameObject.SetActive(false);

    }
    public void buyChair()
    {
        saveData.coin -= 800;
        saveData.chairSelled = true;
        beanBagPrice.text = "구매가 완료되었습니다!";
        buyBeanBagButton.gameObject.SetActive(false);
    }

    public void exitStore()
    {
        gameManager.HouseSceneLoad();
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
    }

}
