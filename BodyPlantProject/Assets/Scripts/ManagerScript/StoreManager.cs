using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
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
    public Button buyToyButton;
    public Button buyBeanBagButton;


    void Start()
    {
        coinAmount = PlayerPrefs.GetInt("CoinAmount");
    }

    void Update()
    {
        coinAmountText.text = coinAmount.ToString() ;

        isSeed1Sold = PlayerPrefs.GetInt("IsSeed1Sold");
        if (coinAmount >= 10 && isSeed1Sold == 0)
            buySeed1Button.interactable = true;
        else
            buySeed1Button.interactable = false;

        isSeed2Sold = PlayerPrefs.GetInt("IsSeed2Sold");
        if (coinAmount >= 10 && isSeed2Sold == 0)
            buySeed2Button.interactable = true;
        else
            buySeed2Button.interactable = false;

        isSeed3Sold = PlayerPrefs.GetInt("IsSeed3Sold");
        if (coinAmount >= 10 && isSeed3Sold == 0)
            buySeed3Button.interactable = true;
        else
            buySeed3Button.interactable = false;

        isSeed4Sold = PlayerPrefs.GetInt("IsSeed4Sold");
        if (coinAmount >= 10 && isSeed4Sold == 0)
            buySeed4Button.interactable = true;
        else
            buySeed4Button.interactable = false;

        isSeed5Sold = PlayerPrefs.GetInt("IsSeed5Sold");
        if (coinAmount >= 10 && isSeed5Sold == 0)
            buySeed5Button.interactable = true;
        else
            buySeed5Button.interactable = false;

        isSeed6Sold = PlayerPrefs.GetInt("IsSeed6Sold");
        if (coinAmount >= 10 && isSeed6Sold == 0)
            buySeed6Button.interactable = true;
        else
            buySeed6Button.interactable = false;

        isSeed7Sold = PlayerPrefs.GetInt("IsSeed7Sold");
        if (coinAmount >= 10 && isSeed7Sold == 0)
            buySeed7Button.interactable = true;
        else
            buySeed7Button.interactable = false;

        isSeed8Sold = PlayerPrefs.GetInt("IsSeed8Sold");
        if (coinAmount >= 10 && isSeed8Sold == 0)
            buySeed8Button.interactable = true;
        else
            buySeed8Button.interactable = false;

        isSeed9Sold = PlayerPrefs.GetInt("IsSeed9Sold");
        if (coinAmount >= 10 && isSeed9Sold == 0)
            buySeed9Button.interactable = true;
        else
            buySeed9Button.interactable = false;

        isSeed10Sold = PlayerPrefs.GetInt("IsSeed10Sold");
        if (coinAmount >= 10 && isSeed10Sold == 0)
            buySeed10Button.interactable = true;
        else
            buySeed10Button.interactable = false;

        isSeed11Sold = PlayerPrefs.GetInt("IsSeed11Sold");
        if (coinAmount >= 10 && isSeed11Sold == 0)
            buySeed11Button.interactable = true;
        else
            buySeed11Button.interactable = false;

        isSeed12Sold = PlayerPrefs.GetInt("IsSeed12Sold");
        if (coinAmount >= 10 && isSeed12Sold == 0)
            buySeed12Button.interactable = true;
        else
            buySeed12Button.interactable = false;





        isToySold = PlayerPrefs.GetInt("IsToySold");
        if (coinAmount >= 20 && isToySold == 0)
            buyToyButton.interactable = true;
        else
            buyToyButton.interactable = false;

        isBeanBagSold = PlayerPrefs.GetInt("IsBeanBagSold");
        if (coinAmount >= 20 && isBeanBagSold == 0)
            buyBeanBagButton.interactable = true;
        else
            buyBeanBagButton.interactable = false;
    }

    public void buySeed1()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed1Sold", 1);
        seed1Price.text = "구매가 완료되었습니다!";
        buySeed1Button.gameObject.SetActive(true);
    }

    public void buySeed2()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed2Sold", 1);
        seed2Price.text = "구매가 완료되었습니다!";
        buySeed2Button.gameObject.SetActive(true);
    }
    public void buySeed3()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed3Sold", 1);
        seed3Price.text = "구매가 완료되었습니다!";
        buySeed3Button.gameObject.SetActive(true);
    }
    public void buySeed4()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed4Sold", 1);
        seed4Price.text = "구매가 완료되었습니다!";
        buySeed4Button.gameObject.SetActive(true);
    }
    public void buySeed5()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed5Sold", 1);
        seed5Price.text = "구매가 완료되었습니다!";
        buySeed5Button.gameObject.SetActive(true);
    }
    public void buySeed6()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed6Sold", 1);
        seed6Price.text = "구매가 완료되었습니다!";
        buySeed6Button.gameObject.SetActive(true);
    }
    public void buySeed7()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed7Sold", 1);
        seed7Price.text = "구매가 완료되었습니다!";
        buySeed7Button.gameObject.SetActive(true);
    }
    public void buySeed8()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed8Sold", 1);
        seed8Price.text = "구매가 완료되었습니다!";
        buySeed8Button.gameObject.SetActive(true);
    }
    public void buySeed9()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed9Sold", 1);
        seed9Price.text = "구매가 완료되었습니다!";
        buySeed9Button.gameObject.SetActive(true);
    }
    public void buySeed10()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed10Sold", 1);
        seed10Price.text = "구매가 완료되었습니다!";
        buySeed10Button.gameObject.SetActive(true);
    }

    public void buySeed11()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed11Sold", 1);
        seed11Price.text = "구매가 완료되었습니다!";
        buySeed11Button.gameObject.SetActive(true);
    }    

    public void buySeed12()
    {
        coinAmount -= 10;
        PlayerPrefs.SetInt("IsSeed12Sold", 1);
        seed12Price.text = "구매가 완료되었습니다!";
        buySeed12Button.gameObject.SetActive(true);
    }

    public void buyToy()
    {
        coinAmount -= 20;
        PlayerPrefs.SetInt("IsToySold", 1);
        toyPrice.text = "구매가 완료되었습니다!";
        buyToyButton.gameObject.SetActive(false);
    }
    public void buyBeanBag()
    {
        coinAmount -= 20;
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
