using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//게임 시작 및 씬을 넘나들 때 사용하는 스크립트
//매우중요!!!!!!!!!!!!!!!!!!!
//왜냐면은 씬을 넘나들 때는 dont destroy on load를 사용하게 된다.
//그렇기 때문에 single ton 이란 스킬을 사용하게 된다.
//single ton은 똑같은 객체가 여러 개 생기는 걸 방지하는 스킬이다.
//gameManager은 전체 게임에서 객체가 단 하나만 존재햐아 한다. 왜냐면은 씬을 넘나들면서 여러개가 생길 수 있는데, 그러면은 관리자가 여러개가 되니까 그건안된다.
//세이브데이터 관리, 씬 넘나들기를 여기서 한다.
public class GameManager : MonoBehaviour
{
    //제이슨 세이브로드하기위한 제이슨매니저
    JsonManager jsonManager;
    SoundManager soundManager;
    public WholeComponents wholeComponents;    //테스트용
    public SaveDataClass saveData;             //세이브데이터
    public static GameManager singleTon;    //싱글톤을 만들기위해 public static으로 만든다. 어디서든 참조가 가능하기 위함.
    public bool fromPotScene;
    public OptionManager optionManager;

    public float loveRatio = 0.00001f;  //애정도 * 초 를 할건데 그냥 변수 여따 만듬

    public int workSceneIndex;  //일하기 <-> 캐릭터 데려오기 씬 이동용입니다

    void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
        if (singleTon == null)
        {
            singleTon = this;
            DontDestroyOnLoad(gameObject);
            //싱글톤이 지정되어있지 않다면. 이 객체(this)를 지정;
        }
        else
        {
            Destroy(gameObject);
            //싱글톤이 이미 지정되어있다면,(한번이라도 위의 코드가 발동했다는 의미) 게임오브젝트 파괴
        }
        fromPotScene = false;
        //제이슨매니저 할당.
        jsonManager = new JsonManager();
        //wholeComponents는 전체 부위인데, 이걸 로딩해준다. 기획자가 만든 데이터 로딩.
        //jsonManager.SaveWholeComponent();
        wholeComponents = jsonManager.LoadComponents();
        //load는 세이브데이터 로드다.
        //saveData = new SaveDataClass();
        //Save();  // 이거는 필요할 때만 있는 코드. 디버그용
        //jsonManager.SaveWholeComponent();
        Load();
        
    }

    void Start()
    {
        soundManager = SoundManager.inst;
    }


    
    //세이브데이터 세이브
    public void Save()
    {
        //제이슨에서 세이브제이슨. 아니 이럴거면 함수를 왜만들어요??
        //그러게요....혹시나 뭐가 늘어날지도 모르잖아요.
        jsonManager.SaveJson(saveData);
    }

    //데이터 로드
    public void Load()
    {//제이슨에서 세이브데이터 로드. 아니 이럴거면 함수를 왜만들어요??
        //그러게요....혹시나 뭐가 늘어날지도 모르잖아요.
        saveData = jsonManager.LoadSaveData();
    }
    
    //합성 씬 로드. flowerPotManager의 ComposeSceneLoad에서 호출-> 합성버튼에서 호출
    public void ComposeSceneLoad()
    {
        fromPotScene = false;
        SceneManager.LoadScene("ComposeScene");
        optionManager.OptionButtonActive(false);
    }
    public void PotSceneLoad()
    {
        fromPotScene = true;
        SceneManager.LoadScene("PotScene");
        optionManager.OptionButtonActive(true);
    }

    public void HouseSceneLoad()
    {
        fromPotScene = false;
        SceneManager.LoadScene("HouseScene");
        soundManager.MainBGMPlay();
        soundManager.HouseEffectPlay();
        optionManager.OptionButtonActive(true);
    }

    public void WorkMineSceneLoad() //일하기 광산
    {
        SceneManager.LoadScene("WorkMineScene");
        soundManager.MineBGMPlay();
        optionManager.OptionButtonActive(true);
    }

    public void WorkHuntSceneLoad() //일하기 사냥
    {
        SceneManager.LoadScene("WorkHuntScene");
        soundManager.HuntBGMPlay();
        optionManager.OptionButtonActive(true);
    }

    public void WorkFishingSceneLoad() //일하기 낚시
    {
        SceneManager.LoadScene("WorkFishingScene");
        soundManager.FishBGMPlay();
        optionManager.OptionButtonActive(true);
    }

    public void StoreSceneLoad()
    {
        SceneManager.LoadScene("StoreScene");
        soundManager.StoreEffectPlay();
        optionManager.OptionButtonActive(false);
    }

    public void BookSceneLoad()
    {
        SceneManager.LoadScene("BookScene");
        soundManager.BookEffectPlay();
        optionManager.OptionButtonActive(false);
    }

    public void SecretRoomSceneLoad()
    {
        SceneManager.LoadScene("SecretRoomScene");
        optionManager.OptionButtonActive(false);
    }

    public void BackToStoreScene()
    {
        if (fromPotScene)
        {
            PotSceneLoad();
        }
        else
        {
            HouseSceneLoad();
        }
    }

    //전역변수 workindex사용
    public void BackToWorkScene()
    {

        SceneManager.LoadScene(workSceneIndex);
        optionManager.OptionButtonActive(true);

    }

    public string GetCompleteWord(string name, string firstVal, string secondVal)
    {
        //char lastName = name.ElementAt(name.Length - 1);
        char lastName = name[name.Length - 1];
        int index = (lastName - 0xAC00) % 28; Console.WriteLine(index);
        //한글의 제일 처음과 끝의 범위 밖일경우 에러
        if (lastName < 0xAC00 || lastName > 0xD7A3)
        {
            return name + secondVal;
        }
        string selectVal = (lastName - 0xAC00) % 28 > 0 ? firstVal : secondVal;
        return name + selectVal;

    }

    //시간계산기입니다. 예전시간과 현재시간을 넣으면 그 사이의 초가 나와요.
    //초를 알아야 얼마나 지났는지 아니까 그거에 맞게 방치가 되어있다는 걸 깨닫고 화분을 싹틔우던 하겠죠?
    //게임이 켜져있지 않을 때 시간이 얼마나 지났는지 체크하기 위한 함수입니다.
    public int TimeSubtractionToSeconds(string pastTime, string latestTime)
    {
        DateTime past = DateTime.Parse(pastTime);
        DateTime latest = DateTime.Parse(latestTime);
        //string to datetime
        if (past == null || latest == null)
        {
            Debug.Log("time is null");
            //Application.Quit();
            return 0;
        }

        TimeSpan span = latest - past;
        int seconds = (int)span.TotalSeconds;
        //Debug.Log("subtraction is " + seconds);

        return seconds;
    }

    public ComponentDataClass FindData(string name)
    {
        foreach (ComponentDataClass data in wholeComponents.componentList)
        {
            if (name == data.name)
            {
                return data;
            }
        }
        return null;
    }

    public void UpdateLoveness()
    {
        float loveRatio = 1.0f;
        if (saveData.chairSelled)
        {
            loveRatio = 1.03f;
        }
        List<CharacterClass> characterList = saveData.characterList;
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].personality == CharacterClass.Personality.Jogon)
            {
                loveRatio += 0.2f;
            }
        }

        for (int i = 0; i < characterList.Count; i++)
        {
            float ratio = loveRatio;
            if (characterList[i].personality == CharacterClass.Personality.Mongsil)
            {
                ratio += 0.5f;
            }
            int time = TimeSubtractionToSeconds(characterList[i].loveStartTime, DateTime.Now.ToString());
            characterList[i].loveTime += time;
            characterList[i].loveStartTime = DateTime.Now.ToString();
            characterList[i].loveNess += this.loveRatio * ratio * time;
            if (characterList[i].loveNess > 100)
            {
                characterList[i].loveNess = 100;
            }
        }
    }


}
