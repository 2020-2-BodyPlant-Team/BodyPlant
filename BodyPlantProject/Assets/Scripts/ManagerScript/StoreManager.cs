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
    int isSeed11Sold;
    int isSeed12Sold;
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
    public Text seed11Price;
    public Text seed12Price;
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
    public Button buySeed11Button;
    public Button buySeed12Button;

    List<Button> seedButtonList;

    public Button buyToyButton;
    public Button buyBeanBagButton;

    /*
     0 body
     1 arm
     2 leg
     3 ear
     4 eye
     5 foot
     6 hair
     7 hand
     8 mouth
     9 nose
    */

    void Start()
    {

        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        boughtNameList = saveData.boughtNameList;
        boughtDateList = saveData.boughtDateList;
        coinAmount = saveData.coin;
        potList = saveData.potList;
        leftPot = 0;
        foreach(ComponentClass component in potList)
        {
            if(component.name == "null")
            {
                leftPot++;
            }
        }
        leftPot -= boughtNameList.Count;

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
        seedButtonList.Add(buySeed11Button);
        seedButtonList.Add(buySeed12Button);

    }

    void Update()
    {
        coinAmountText.text = saveData.coin.ToString() ;



        
        isSeed1Sold = PlayerPrefs.GetInt("IsSeed1Sold");
        if (saveData.coin >= 10)
            buySeed1Button.interactable = true;
        else
            buySeed1Button.interactable = false;

        isSeed2Sold = PlayerPrefs.GetInt("IsSeed2Sold");
        if (saveData.coin >= 10)
            buySeed2Button.interactable = true;
        else
            buySeed2Button.interactable = false;

        isSeed3Sold = PlayerPrefs.GetInt("IsSeed3Sold");
        if (saveData.coin >= 10)
            buySeed3Button.interactable = true;
        else
            buySeed3Button.interactable = false;

        isSeed4Sold = PlayerPrefs.GetInt("IsSeed4Sold");
        if (saveData.coin >= 10)
            buySeed4Button.interactable = true;
        else
            buySeed4Button.interactable = false;

        isSeed5Sold = PlayerPrefs.GetInt("IsSeed5Sold");
        if (saveData.coin >= 10)
            buySeed5Button.interactable = true;
        else
            buySeed5Button.interactable = false;

        isSeed6Sold = PlayerPrefs.GetInt("IsSeed6Sold");
        if (saveData.coin >= 10)
            buySeed6Button.interactable = true;
        else
            buySeed6Button.interactable = false;

        isSeed7Sold = PlayerPrefs.GetInt("IsSeed7Sold");
        if (saveData.coin >= 10)
            buySeed7Button.interactable = true;
        else
            buySeed7Button.interactable = false;

        isSeed8Sold = PlayerPrefs.GetInt("IsSeed8Sold");
        if (saveData.coin >= 10)
            buySeed8Button.interactable = true;
        else
            buySeed8Button.interactable = false;

        isSeed9Sold = PlayerPrefs.GetInt("IsSeed9Sold");
        if (saveData.coin >= 10 )
            buySeed9Button.interactable = true;
        else
            buySeed9Button.interactable = false;

        isSeed10Sold = PlayerPrefs.GetInt("IsSeed10Sold");
        if (saveData.coin >= 10)
            buySeed10Button.interactable = true;
        else
            buySeed10Button.interactable = false;

        isSeed11Sold = PlayerPrefs.GetInt("IsSeed11Sold");
        if (saveData.coin >= 10)
            buySeed11Button.interactable = true;
        else
            buySeed11Button.interactable = false;

        isSeed12Sold = PlayerPrefs.GetInt("IsSeed12Sold");
        if (saveData.coin >= 10)
            buySeed12Button.interactable = true;
        else
            buySeed12Button.interactable = false;

        if (leftPot <= 0)
        {
            foreach (Button button in seedButtonList)
            {
                button.interactable = false;
            }
        }



        isToySold = PlayerPrefs.GetInt("IsToySold");
        if (saveData.coin >= 20 && isToySold == 0)
            buyToyButton.interactable = true;
        else
            buyToyButton.interactable = false;

        isBeanBagSold = PlayerPrefs.GetInt("IsBeanBagSold");
        if (saveData.coin >= 20 && isBeanBagSold == 0)
            buyBeanBagButton.interactable = true;
        else
            buyBeanBagButton.interactable = false;
    }

    public void buySeed1()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed1Sold", 1);
        seed1Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("body");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed1Button.gameObject.SetActive(true);
    }

    public void buySeed2()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed2Sold", 1);
        seed2Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("arm");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed2Button.gameObject.SetActive(true);

    }
    public void buySeed3()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed3Sold", 1);
        seed3Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("leg");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed3Button.gameObject.SetActive(true);

    }
    public void buySeed4()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed4Sold", 1);
        seed4Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("ear");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed4Button.gameObject.SetActive(true);

    }
    public void buySeed5()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed5Sold", 1);
        seed5Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("eye");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed5Button.gameObject.SetActive(true);
    }
    public void buySeed6()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed6Sold", 1);
        seed6Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("foot");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed6Button.gameObject.SetActive(true);
    }
    public void buySeed7()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed7Sold", 1);
        seed7Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("hair");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed7Button.gameObject.SetActive(true);
    }
    public void buySeed8()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed8Sold", 1);
        seed8Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("hand");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed8Button.gameObject.SetActive(true);
    }
    public void buySeed9()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed9Sold", 1);
        seed9Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("mouth");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed9Button.gameObject.SetActive(true);
    }
    public void buySeed10()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed10Sold", 1);
        seed10Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("nose");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed10Button.gameObject.SetActive(true);
    }

    public void buySeed11()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed11Sold", 1);
        seed11Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("body");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed11Button.gameObject.SetActive(true);
    }    

    public void buySeed12()
    {
        saveData.coin -= 10;
        PlayerPrefs.SetInt("IsSeed12Sold", 1);
        seed12Price.text = "구매가 완료되었습니다!";
        boughtNameList.Add("body");
        boughtDateList.Add(DateTime.Now.ToString());
        leftPot--;
        gameManager.Save();
        buySeed12Button.gameObject.SetActive(true);
    }

    public void buyChair()
    {
        saveData.coin -= 20;
        PlayerPrefs.SetInt("IsToySold", 1);
        toyPrice.text = "구매가 완료되었습니다!";
        buyToyButton.gameObject.SetActive(false);

    }
    public void buyTrain()
    {
        saveData.coin -= 20;
        PlayerPrefs.SetInt("IsBeanBagSold", 1);
        beanBagPrice.text = "구매가 완료되었습니다!";
        buyBeanBagButton.gameObject.SetActive(false);
    }

    public void exitStore()
    {
        PlayerPrefs.SetInt("CoinAmount", coinAmount);
        SceneManager.LoadScene("HouseScene");
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
        buySeed11Button.gameObject.SetActive(true);
        buySeed12Button.gameObject.SetActive(true);
        buyToyButton.gameObject.SetActive(true);
        buyBeanBagButton.gameObject.SetActive(true);

        seed1Price.text = "10코인";
        seed2Price.text = "10코인";
        seed3Price.text = "10코인";
        seed4Price.text = "10코인";
        seed5Price.text = "10코인";
        seed6Price.text = "10코인";
        seed7Price.text = "10코인";
        seed8Price.text = "10코인";
        seed9Price.text = "10코인";
        seed10Price.text = "10코인";
        seed11Price.text = "10코인";
        seed12Price.text = "10코인";

        toyPrice.text = "20코인";
        beanBagPrice.text = "20코인";

        PlayerPrefs.DeleteAll();
    }

}
