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
            PlayerPrefs.SetFloat("SoundEffect",1); ;
            soundEffectSlider.value = 1;
            soundEffectToggle.isOn = false;
        }
        if (!PlayerPrefs.HasKey("BGM"))
        {
            PlayerPrefs.SetFloat("BGM", 1); ;
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

        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffect");
        if (soundEffectSlider.value == 0)
        {
            soundEffectToggle.isOn = true;
        }
        else
        {
            soundEffectToggle.isOn = false;
        }
    }

    public void SoundEffectToggle()
    {
        soundManager.effectSource.mute = soundEffectToggle.isOn;
        soundManager.buttonSource.mute = soundEffectToggle.isOn;

    }
    public void BgmToggle()
    {
        soundManager.bgmSource.mute = bgmToggle.isOn;
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

    public void OptionActive()
    {
        if (optionCanvas.activeSelf)
        {
            PlayerPrefs.SetFloat("SoundEffect", soundEffectSlider.value);
            PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        }
        soundManager.ButtonEffectPlay();
        optionCanvas.SetActive(!optionCanvas.activeSelf);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}