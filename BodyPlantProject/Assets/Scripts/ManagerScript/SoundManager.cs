using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;

    public AudioSource bgmSource;
    public AudioSource effectSource;
    public AudioSource buttonSource;
    
    public AudioClip mainBGM;
    public AudioClip fishBGM;
    public AudioClip mineBGM;
    public AudioClip huntBGM;

    public AudioClip bookEffect;
    public AudioClip storeEffect;
    public AudioClip workEffect;
    public AudioClip houseEffect;
    public AudioClip cheerEffect;
    public AudioClip composeEffect;
    public AudioClip fanfareEffect;
    public AudioClip errorEffect;
    public AudioClip harvestEffect;
    public AudioClip plantEffect;
    public AudioClip expandEffect;
    public AudioClip coinEffect;

    // Start is called before the first frame update
    void Start()
    {
        if(inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        bgmSource.clip = mainBGM;
        bgmSource.Play();

    }

    public void MainBGMPlay()
    {
        if(bgmSource.clip == mainBGM)
        {
            return;
        }
        bgmSource.clip = mainBGM;
        bgmSource.Play();
    }

    public void FishBGMPlay()
    {
        if (bgmSource.clip == fishBGM)
        {
            return;
        }
        bgmSource.clip = fishBGM;
        bgmSource.Play();
        WorkEffectPlay();
    }

    public void MineBGMPlay()
    {
        if (bgmSource.clip == mineBGM)
        {
            return;
        }
        bgmSource.clip = mineBGM;
        bgmSource.Play();
        WorkEffectPlay();
    }

    public void HuntBGMPlay()
    {
        if (bgmSource.clip == huntBGM)
        {
            return;
        }
        bgmSource.clip = huntBGM;
        bgmSource.Play();
        WorkEffectPlay();
    }

    void WorkEffectPlay()
    {
        effectSource.clip = workEffect;
        effectSource.Play();
    }

    public void BookEffectPlay()
    {
        effectSource.clip = bookEffect;
        effectSource.Play();
    }

    public void StoreEffectPlay()
    {
        effectSource.clip = storeEffect;
        effectSource.Play();
    }

    public void HouseEffectPlay()
    {
        if(effectSource.clip == fanfareEffect)
        {
            return;
        }
        effectSource.clip = houseEffect;
        effectSource.Play();
    }

    public void CheerEffectPlay()
    {
        effectSource.clip = cheerEffect;
        effectSource.Play();
    }

    public void ComposeEffectPlay()
    {
        effectSource.clip = composeEffect;
        effectSource.Play();
    }

    public void FanfareEffectPlay()
    {
        effectSource.clip = fanfareEffect;
        effectSource.Play();
    }

    public void ErrorEffectPlay()
    {
        effectSource.clip = errorEffect;
        effectSource.Play();
    }

    public void HarvestEffectPlay()
    {
        effectSource.clip = harvestEffect;
        effectSource.Play();
    }

    public void PlantEffectPlay()
    {
        effectSource.clip = plantEffect;
        effectSource.Play();
    }

    public void ExpandEffectPlay()
    {
        effectSource.clip = expandEffect;
        effectSource.Play();
    }

    public void CoinEffectPlay()
    {
        effectSource.clip = coinEffect;
        effectSource.Play();
    }

    public void ButtonEffectPlay()
    {
        buttonSource.Play();
    }

    public void Mute(bool muteOnTrue)
    {
        bgmSource.mute = muteOnTrue;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
