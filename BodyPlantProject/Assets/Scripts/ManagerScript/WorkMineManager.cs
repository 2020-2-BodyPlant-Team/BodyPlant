using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkMineManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;
    List<CharacterClass> characterList;
    SoundManager soundManager;

    public CharacterMover characterMover;
    public GiveCoin coinManager;
    public GameObject[] parentObjectArray;
    public GameObject bringButton;
    public Text mineElementText;

    public WorkCharacterManager workCharacterManager;

    Image barImage;
    float maxBar = 100f;
    public static float barAmount;

    public GameObject aim;
    public GameObject rock;
    public RectTransform barMineral;

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }

    public void BringBtnOnClick()
    {
        gameManager.SecretRoomSceneLoad();
    }

    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;
        characterList = saveData.mineCharacterList;
        soundManager = SoundManager.inst;

        barImage = GameObject.Find("Bar").GetComponent<Image>();
        barAmount = 0f;

        if (characterList.Count > 3)
        {
            Application.Quit(); //몰라 꺼버려 ㅋㅋ
        }
        if (characterList.Count == 3 || saveData.characterList.Count == 0)
        {
            bringButton.SetActive(false);
        }

        gameManager.workSceneIndex = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < parentObjectArray.Length; i++)
        {
            parentObjectArray[i].SetActive(false);
        }

        for (int i = 0; i < characterList.Count; i++)
        {
            parentObjectArray[i].SetActive(true);
            GameObject characterObject;
            characterMover.SpawnCharacter(characterList[i], i);
            characterObject = characterList[i].realGameobject;
            characterObject.transform.position = new Vector3(parentObjectArray[i].transform.position.x, parentObjectArray[i].transform.position.y, 0);
            characterObject.transform.localScale = parentObjectArray[i].transform.localScale;
            parentObjectArray[i].transform.SetParent(characterObject.transform);
        }

        coinManager.SetCharacterList(characterList, 2);

        mineElementText.text = saveData.mineElement.ToString();

        aim.SetActive(false);
        InvokeRepeating("SpawnAim", 2, 1.8f);
        workCharacterManager.SetCharacterList(characterList);
    }

    void Update()
    {
        barImage.fillAmount = barAmount / maxBar;
        if(barAmount >= maxBar)
        {
            //보조성분 획득할 곳
            saveData.mineElement++;
            gameManager.Save();
            mineElementText.text = saveData.mineElement.ToString();
            Debug.Log("게이지 만땅");
            barAmount = 0;
            soundManager.SuccedEffectPlay();
            barMineral.anchoredPosition =new Vector3(0, -340, 0);
        }
        characterMover.RotationUpdate();
    }

    public void FillingBar()
    {
        barAmount += 10f;
        barMineral.anchoredPosition = Vector3.Lerp(new Vector3(0, -340, 0), new Vector3(0, 360, 0), barAmount / maxBar);
        aim.SetActive(false);
        StartCoroutine("MineSound");
    }

    IEnumerator MineSound()
    {
        AudioClip[] mineAudio = new AudioClip[2];
        mineAudio[0] = soundManager.gangEffect;
        mineAudio[1] = soundManager.stoneEffect;
        for(int i = 0; i < 2; i++)
        {
            if(barAmount != 100)
            {
                soundManager.effectSource.clip = mineAudio[i];
                soundManager.effectSource.Play();
                yield return new WaitForSeconds(0.4f);
            }
        }
    }

    void SpawnAim()
    {
        //Debug.Log("move");
        aim.SetActive(true);
        float posX = Random.Range(-1.0f, 1.5f);
        float posY = Random.Range(-4.0f, -1.0f);
        aim.transform.position = new Vector3(posX, posY, 0);
    }
}
