using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    GameManager gameManager;
    SoundManager soundManager;
    public StoreManager storeManager;
    public Button adButton;
    public string[] chatTextArray;
    public GameObject chatObject;
    public Text chatText;
    public GameObject[] elementArray;
    public GameObject elementObject;



    //�̰� �츮��
    //const string adUnitId = "ca-app-pub-6023793752348178/6634578309";
    
    //�̰� �׽�Ʈ
    const string adUnitId = "ca-app-pub-3940256099942544/5224354917";
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        rewardedAd = new RewardedAd(adUnitId);
        gameManager = GameManager.singleTon;
        soundManager = SoundManager.inst;
        // Create an empty ad request.

        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);

        string nowString = chatTextArray[UnityEngine.Random.Range(0, 12)];
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand == 1)
        {
            nowString = chatTextArray[UnityEngine.Random.Range(12, chatTextArray.Length)];
        }
        StartCoroutine(LoadTextOneByOne(nowString, chatText));
        chatObject.SetActive(true);
        for(int i = 0; i < elementArray.Length; i++)
        {
            elementArray[i].SetActive(false);
        }
        elementObject.SetActive(false);
    }

    public IEnumerator LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.1f, bool canClickSkip = true)
    {
        float miniTimer = 0f;
        float currentTargetNumber = 0f;
        int currentNumber = 0;
        string displayedText = "";
        StringBuilder builder = new StringBuilder(displayedText);
        while (currentTargetNumber < inputTextString.Length)
        {

            while (currentNumber < currentTargetNumber)
            {
                //displayedText += inputTextString.Substring(currentNumber,1);
                builder.Append(inputTextString.Substring(currentNumber, 1));
                currentNumber++;
            }
            //inputTextUI.text = displayedText;
            inputTextUI.text = builder.ToString();
            yield return null;

            miniTimer += Time.deltaTime;
            currentTargetNumber = miniTimer / eachTime;
        }
        while (currentNumber < inputTextString.Length)
        {
            builder.Append(inputTextString.Substring(currentNumber, 1));
            currentNumber++;
        }
        inputTextUI.text = builder.ToString();
        yield return null;

    }

    void ShowAd(object sender, EventArgs args)
    {
        this.rewardedAd.Show();
        this.rewardedAd.OnAdLoaded -= ShowAd;
    }
    public void OnAdButton()
    {
        adButton.interactable = false;
        
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        else
        {
            this.rewardedAd.OnAdLoaded += ShowAd;
        }
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        soundManager.BGMStop();   
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        adButton.interactable = true;
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        soundManager.BGMPlay();
        storeManager.BuyUpdate();
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        int rand = UnityEngine.Random.Range(0, 3);
        chatObject.SetActive(false);
        elementObject.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            elementArray[i].SetActive(false);
        }
        elementArray[rand].SetActive(true);


        if(rand == 0)
        {
            gameManager.saveData.huntElement += 3;
        }
        else if(rand == 1)
        {
            gameManager.saveData.mineElement += 3;
        }
        else if(rand == 2)
        {
            gameManager.saveData.fishElement += 3;
        }
        gameManager.Save();
        //soundManager.BGMPlay();
        //storeManager.BuyUpdate();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
