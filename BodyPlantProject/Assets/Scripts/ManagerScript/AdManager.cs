using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    GameManager gameManager;
    SoundManager soundManager;

    //이게 우리거
    //const string adUnitId = "ca-app-pub-6023793752348178/6634578309";
    
    //이건 테스트
    const string adUnitId = "ca-app-pub-3940256099942544/5224354917";
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        rewardedAd = new RewardedAd(adUnitId);
        gameManager = GameManager.singleTon;
        soundManager = SoundManager.inst;
        // Create an empty ad request.
        
    }

    public void OnAdButton()
    {



        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }


    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        soundManager.BGMStop();   
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        soundManager.BGMPlay();
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        gameManager.saveData.coin += 50;
        gameManager.Save();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
