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
    public WholeComponents wholeComponents;    //테스트용
    public SaveDataClass saveData;             //세이브데이터
    public static GameManager singleTon;    //싱글톤을 만들기위해 public static으로 만든다. 어디서든 참조가 가능하기 위함.
    

    void Awake()
    {

        if(singleTon == null)
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
        
        //제이슨매니저 할당.
        jsonManager = new JsonManager();
        //wholeComponents는 전체 부위인데, 이걸 로딩해준다. 기획자가 만든 데이터 로딩.
        jsonManager.SaveWholeComponent();
        wholeComponents = jsonManager.LoadComponents();
        //load는 세이브데이터 로드다.
        //saveData = new SaveDataClass();
        //Save();  // 이거는 필요할 때만 있는 코드. 디버그용
        //jsonManager.SaveWholeComponent();
        Load();
        
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
        SceneManager.LoadScene("ComposeScene");
    }
    public void PotSceneLoad()
    {
        SceneManager.LoadScene("PotScene");
    }

    public void HouseSceneLoad()
    {
        SceneManager.LoadScene("HouseScene");
    }

    public void WorkMineSceneLoad()
    {
        SceneManager.LoadScene("WorkMineScene");
    }

    public void StoreSceneLoad()
    {
        SceneManager.LoadScene("StoreScene");
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
            Application.Quit();
            return 0;
        }

        TimeSpan span = latest - past;
        int seconds = (int)span.TotalSeconds;
        //Debug.Log("subtraction is " + seconds);

        return seconds;
    }


}
