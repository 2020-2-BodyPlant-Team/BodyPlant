using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{

    public Slider soundEffectSlider;
    public Slider bgmSlider;

    public Toggle soundEffectToggle;
    public Toggle bgmToggle;

    public SoundManager soundManager;

    public static OptionManager singleTon;

    public GameObject optionCanvas;

    public GameObject optionButtonObject;
    public GameObject optionFade;
    public GameObject creditCanvas;
    public bool optionOn;

    // Start is called before the first frame update
    void Start()
    {
        if(singleTon == null)
        {
            singleTon = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!PlayerPrefs.HasKey("SoundEffect"))
        {
            PlayerPrefs.SetFloat("SoundEffect",1);
        }
        if (!PlayerPrefs.HasKey("BGM"))
        {
            PlayerPrefs.SetFloat("BGM", 1);
        }
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        if (bgmSlider.value == 0)
        {
            bgmToggle.isOn = true;
        }
        else
        {
            bgmToggle.isOn = false;
        }

        soundManager.bgmSource.volume = bgmSlider.value;

        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffect");
        if (soundEffectSlider.value == 0)
        {
            soundEffectToggle.isOn = true;
        }
        else
        {
            soundEffectToggle.isOn = false;
        }

        soundManager.effectSource.volume = soundEffectSlider.value;
        soundManager.buttonSource.volume = soundEffectSlider.value;

    }

    public void SoundEffectToggle()
    {
        soundManager.effectSource.mute = soundEffectToggle.isOn;
        soundManager.buttonSource.mute = soundEffectToggle.isOn;
        if (soundEffectToggle.isOn)
        {
            soundEffectSlider.value = 0;
            SoundEffectSlideValue();
            PlayerPrefs.SetFloat("SoundEffect", 0);
        }

    }
    public void BgmToggle()
    {
        soundManager.bgmSource.mute = bgmToggle.isOn;
        if (bgmToggle.isOn)
        {
            bgmSlider.value = 0;
            BgmSlideValue();
            PlayerPrefs.SetFloat("BGM", 0);
        }
    }

    public void SoundEffectSlideValue()
    {
        soundManager.effectSource.volume = soundEffectSlider.value;
        soundManager.buttonSource.volume = soundEffectSlider.value;

    }

    public void BgmSlideValue()
    {
        soundManager.bgmSource.volume = bgmSlider.value;
    }

    public void ExitButton()
    {
        soundManager.ButtonEffectPlay();
        Application.Quit();
    }

    public void OptionButtonActive(bool active)
    {
        optionButtonObject.SetActive(active);
    }

    public void OptionFade(bool active)
    {
        optionFade.SetActive(active);
    }

    public void CreditActive(bool active)
    {
        creditCanvas.SetActive(active);
        optionCanvas.SetActive(!active);
    }

    public void OptionActive()
    {
        soundManager.ButtonEffectPlay();
        if (creditCanvas.activeSelf)
        {
            return;
        }
        if (optionCanvas.activeSelf)
        {
            PlayerPrefs.SetFloat("SoundEffect", soundEffectSlider.value);
            PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        }

        optionCanvas.SetActive(!optionCanvas.activeSelf);
        optionOn = optionCanvas.activeSelf;
        if(optionOn== false && creditCanvas.activeSelf == true)
        {
            optionOn = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
