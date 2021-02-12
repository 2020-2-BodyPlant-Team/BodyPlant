using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//화분을 키우는 스크립트.
/// <summary>
/// 화분을 키우는데는 어떻게 하느냐.
/// 1. 세이브데이터를 받아와서 현재 화분이 얼마나 차있는지 확인한다
/// 2. 그 화분이 이미 다 컸으면 완료상태로 놔둔다
/// 3. 그 화분이 크지 않았으면 로딩바랑 올라오게 놔둔다.
/// </summary>
public class FlowerPotManager : MonoBehaviour
{
    GameManager gameManager;        //게임매니저를 통해 세이브데이터를 참조해야 한다.
    SaveDataClass saveData;         //게임매니저를 통해 가져올 세이브 데이터
    /// <summary>
    /// 이 세이브데이터를 왜 가져와요? 그냥 gameManager.saveData로 쓰면 되죠?? 하실 분들을 위한 설명
    /// 첫번쨰로는 gameManager.를 쓰기 일일히 귀찮아서 그렇다;;;
    /// saveData를 나중에 gameManager로 굳이 넘겨줄 필요는 없다. 어차피 참조에 의한 호출이어서 여기서 값을 변경해도 gameManager에 있는 값이 변경된다.
    /// 그.런.데. 가끔가다가 그게 안되는 경우가 있다. 나도 모른다. 왜 안되는지 모른다.
    /// 그래서 gameManager.saveData = saveData로 넘겨줄 수 있을 수도 있으니까 나중을 위하여 이렇게 쓴다.
    /// </summary>
    WholeComponents wholeComponents;    //전체 컴포넌트 리스트

    ComponentClass[] componentsInPot;   //화분에 들어가있는 부위들이다.
    public GameObject[] flowerPotArray; //이거는 에디터에서 드래그앤드롭해준다.
    public GameObject[] magnifierArray; //에디터에서 드래그앤드롭, 돋보기 오브젝트입니다.

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.
    public int potNumber = 3;               //화분 개수

    //확대와 관련된 변수들
    public GameObject cameraObject;         //카메라 무빙쳐야돼서
    Vector3 originCameraPos;                //카메라가 원래 있는 자리. 그 자리를 알아야 확대 애니메이션 하고 뒤로가기 했을 때 돌아옵니다.
    int nowMagnifiedPotIndex = -1;               //현재 확대해서 보고있는 화분. 평소에는 -1. 확대했을 때는 index값

    public GameObject magnifiedUIObject;     //확대했을 때의 UI를 통쨰로 껐다 켰다 해줘야해요.
    bool nowMagnified = false;              //현재 확대되어있는 상태인지.
    public GameObject buttonBundle;         //버튼모음집 껐다켰따.

    public GameObject progressBar;          //확대했을 때 성장도 오브젝트
    public GameObject harvestButton;        //수확버튼이 수확 가능할 때 떠야한다;
    public Text nameText;
    public GameObject harvestCanvas;        //수확버튼 누르면 뜨는 캔버스
    


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;    //싱글톤을하면 이런식으로 가져올 수 있다. 편하다.
        //saveData = GameManager.saveData;
        saveData = gameManager.saveData;
        componentsInPot = saveData.potList;
        wholeComponents = gameManager.wholeComponents;
        originCameraPos = cameraObject.transform.position;
        magnifiedUIObject.SetActive(false);
        //초깃값을 다 설정해준다. gameManager에 있으니까 설정해준다.

        //화분에 있는 부위들을 먼저 가져온다.
        for (int i = 0; i < potNumber; i++)
        {
            //돋보기 오브젝트 꺼주기
            magnifierArray[i].SetActive(false);
            int elapsedTime = 0;

            //만약 화분이 비어있다면 다음 인덱스로 넘기기
            if (componentsInPot[i].name == "null")
            {
                Debug.Log("이름이 null이니까 컨티뉴");
                continue;
            }
            ComponentDataClass componentData = FindData(componentsInPot[i].name);
            //이쪽 데이터에 arm이 들어갈거에요
            if (componentData == null)
            {
                Debug.LogError("data가 null이다");
                return;
            }

            elapsedTime = gameManager.TimeSubtractionToSeconds(componentsInPot[i].plantedTime, DateTime.Now.ToString());
            componentsInPot[i].percentage = elapsedTime / componentData.sproutSeconds;
            if (componentsInPot[i].percentage > 1)
            {
                componentsInPot[i].percentage = 1;
            }
            //지난 시간을 구해주고 그것이 건설시간보다 큰지 체크해준다.
            if(elapsedTime > componentData.sproutSeconds)
            {
                componentsInPot[i].isSprotued = true;//크면은 트류
            }
            else
            {
                componentsInPot[i].isSprotued = false;  //아니면은 폴스
            }
            //만약 false라면 자라나고있는 상태니까 건물을 올려주어야겠죠?
            if(componentsInPot[i].isSprotued == false)
            {
                GameObject prefab;
                if (componentsInPot[i].percentage == 1)
                {
                    prefab = Resources.Load<GameObject>("Components/Complete/" + componentData.name);
                }
                else if(componentsInPot[i].percentage >= 0.5)
                {
                    prefab = Resources.Load<GameObject>("Components/Growing2/" + componentData.name);
                }
                else
                {
                    string seedName = "seed1";
                    string name = componentData.name;
                    if (name == "arm" || name == "leg" || name == "hand" || name == "foot")
                    {
                        seedName = "seed2";
                    }
                    if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
                    {
                        seedName = "seed3";
                    }
                    if (name == "hair")
                    {
                        seedName = "hair";
                    }
                    prefab = Resources.Load<GameObject>("Components/Growing1/" + seedName);
                }
                    //Resources/Components/arm 
                    Debug.Log(componentData.name);
                //먼저 프리팹을 resource폴더에서 읽어오고
                GameObject obj = Instantiate(prefab, flowerPotArray[i].transform);
                
                //그 오브젝트를 components in pot 에 넣어준다. 그래야 꺼내서 쓸 수 있다.
                //여기서 유의할 점은 나중에 components in pot이 savedata로 다시 들어갈 텐데, 그 때는 gameobject는 저장이 안된다
                //따라서 savedata를 load할 때마다 gameobject를 프리팹에서 꺼내서 새로 만들어주어야한다.
                componentsInPot[i].realGameobject = obj;
                StartCoroutine(SproutingCoroutine(i, componentData));
                //코루틴을 시작해준다.
            }
            else if(componentsInPot[i].isHarvested == false)
            {
                //만약 자라긴 자랐는데 수확이 안된 경우.
                GameObject prefab = Resources.Load<GameObject>("Components/Complete/" + componentData.name);
                //먼저 프리팹을 resource폴더에서 읽어오고
                GameObject obj = Instantiate(prefab, flowerPotArray[i].transform);
                obj.transform.localPosition = new Vector3(0, 0, -5);
                //위와 같다. 다만 코루틴 작동을 하지 않는다.
                componentsInPot[i].realGameobject = obj;
                obj.transform.localPosition = new Vector3(componentData.sproutingPosition.x, componentData.sproutingPosition.y,-5);
            }
            StartCoroutine(PlantShake(componentsInPot[i]));
        }


    }

    ComponentDataClass FindData(string name)
    {
        for(int i = 0; i < wholeComponents.componentList.Count; i++)
        {
            ComponentDataClass data = wholeComponents.componentList[i];
            if (name == data.name)
            {
                return data;
            }
        }
        return null;
    }

    //꽃피워올리는 코루틴. 
    IEnumerator SproutingCoroutine(int index,ComponentDataClass componentData)
    {
        float lastPercentage = 0;
        //꽃피지 않을때만 돌아간다.
        float percentage = 0;
        while (componentsInPot[index].isSprotued == false)
        {
            int elapsedTime = 0;
            //몇퍼센트 완성인지.

            //포지션을 업데이트 해준다. sproutingPosition이 최종 위치니까, 이거에 percentage를 곱해서 해준다.
            lastPercentage = percentage;

            yield return new WaitForSeconds(1); //   1초에 한번씩 업데이트를 해준다.

            elapsedTime = gameManager.TimeSubtractionToSeconds(componentsInPot[index].plantedTime, DateTime.Now.ToString());
            if (componentData.sproutSeconds < elapsedTime)
            {
                componentsInPot[index].isSprotued = true;
                Debug.Log("스프라이트 바꿔주는거");
                GameObject prefab = Resources.Load<GameObject>("Components/Complete/" + componentData.name);
                GameObject obj = Instantiate(prefab, flowerPotArray[index].transform);
                obj.transform.localPosition = componentsInPot[index].realGameobject.transform.localPosition;
                componentsInPot[index].realGameobject.SetActive(false);
                componentsInPot[index].realGameobject = obj;
                //만약 시간이 지났다면 싹틔워준다.
            }
            percentage = elapsedTime / componentData.sproutSeconds;
            if (percentage >= 0.5 && lastPercentage < 0.5)
            {
                Debug.Log(percentage + " 라스트" + lastPercentage);
                GameObject prefab = Resources.Load<GameObject>("Components/Growing2/" + componentData.name);
                GameObject obj = Instantiate(prefab, flowerPotArray[index].transform);
                obj.transform.localPosition = componentsInPot[index].realGameobject.transform.localPosition;
                componentsInPot[index].realGameobject.SetActive(false);
                componentsInPot[index].realGameobject = obj;
            }
            //마지막 1초의 순간엔 이게 1을 넘어가버려서, 1로 맞춰준다.
            if (percentage >= 1)
            {
                percentage = 1;
            }
            if (index == nowMagnifiedPotIndex)
            {
                //만약 현재 확대한 pot하고 코루틴돌아가는 index하고 일치한다면 progressBar를 계속 업데이트 해준다.
                progressBar.transform.localScale = new Vector3(1, percentage, 1);
            }
            componentsInPot[index].percentage = percentage;
            if (componentsInPot[index].realGameobject.transform.localPosition.y == 1.2)
            {
                componentsInPot[index].realGameobject.transform.localPosition = new Vector3(0,1.0f, -5);
            }
            else
            {
                componentsInPot[index].realGameobject.transform.localPosition = new Vector3(0,1.2f, -5);
            }
            
        }
        //1하고 1.2 왔다리 갔다리
        //이제 업데이트를 다 해주다가 isSprotued==true가 돼서 탈출을 하게 되면, 수확을 해주어야 한다
        componentsInPot[index].isHarvested = false;
        //만약 현재 확대된 상태면 수확버튼 활성화.
        if (nowMagnified)
        {
            harvestButton.SetActive(true);
        }
    }

    IEnumerator PlantShake(ComponentClass component)
    {
        float timer = 0;
        bool plus = true;
        while(component.isHarvested == false)
        {
            if(plus)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            if (timer > 1)
            {
                plus = false;
            }
            if (timer < 0)
            {
                plus = true;
            }

            
            component.realGameobject.transform.localPosition =  new Vector3(0,1.0f +0.1f*timer, -5);
            yield return null;
        }
    }

    //게임 끌 때 저장
    private void OnApplicationQuit()
    {
        gameManager.Save();
    }



    //심은걸 수확하는거. 터치해서 수확. 버튼에서 호출
    public void HarvestSprout()
    {
        int index = nowMagnifiedPotIndex;   //어차피 확대된 거에서만 호출하니까 확대된 index를 불러온다.
        Debug.Log("이거 되긴 하냐" + index);
        //수확을 할 때에는 먼저 오브젝트를 없애주고
        Destroy(componentsInPot[index].realGameobject);

        //수확이 되었다는거를 세이브해줘야하니까 세이브데이터에 넣어주고
        componentsInPot[index].isHarvested = true;
        saveData.owningComponentList.Add(componentsInPot[index]);
        //수확이 된 거는 아무것도 없는 빈 객체를 넣는다.
        //왜냐하면 ㅡㅡ componentsInPot[index] = null을하니까 new ComponentClass()가 생겨버리드라 ㅎㅎ 화나게...null이 안들어간다ㅣ...
        componentsInPot[index] = new ComponentClass();
        //지워준 정보도 savedata에 저장을 안해도 자동저장이 된다. saveData에서 참조로 가져온 값이라 된다.
        progressBar.transform.localScale = new Vector3(1, 0, 1);
        //로딩바 0으로.
        harvestButton.SetActive(false);
        //버튼은 비활
        nameText.gameObject.SetActive(false);
        //텍스트 비활
        gameManager.Save();
        //세이브해준다.
    }

    //상점에서 호출할거 
    void PlantComponent(string name)
    {
        //자리가 있는지부터 체크해준다.
        bool isPlaceAvailable = false;
        int availablePlace = -1;
        for(int i = 0; i < potNumber; i++)
        {

            //아무것도 심어져있지 않으면 null이다. 이름이 null이다. 나이렇게 코딩하는거 싫어요 ㅠㅠ
            if(componentsInPot[i].name == "null")
            {

                //아무것도 심어져있지않은 화분이 있으면 거기다가 심기.
                isPlaceAvailable = true;
                availablePlace = i;
                break;
            }
        }
        if(isPlaceAvailable == false)
        {
            //만약 자리가 없다면 리턴.
            Debug.Log("자리ㅏ가 없다");
            return;
        }

        //이제 전체 컴포넌트중에서 이름을통해 특정 컴포넌트를 찾는다. 이름은 파라미터로 받았따.
        ComponentClass component = new ComponentClass();
        ComponentDataClass componentData = null;
        foreach(ComponentDataClass com in wholeComponents.componentList)
        {
            if(com.name == name)
            {
                component.name = name;
                componentData = com;
                break;
            }
        }
        if(component.name == "null")
        {
            Debug.LogError("화분의 이름을 사전에서 못찾았다");
            return;
        }
        Debug.Log(availablePlace);
        componentsInPot[availablePlace] = component;
        //이 아래부터는 start에 있는거랑 똑같다.
        //GameObject prefab = Resources.Load<GameObject>("Components/Growing2/" + componentData.name);
        string seedName = "seed1";
        if (name == "arm" || name == "leg"|| name == "hand" || name == "foot")
        {
            seedName = "seed2";
        }
        if (name == "ear" || name == "eye" || name == "mouth" || name == "nose")
        {
            seedName = "seed3";
        }
        if (name == "hair")
        {
            seedName = "hair";
        }
        GameObject prefab = Resources.Load<GameObject>("Components/Growing1/" + seedName);
        //먼저 프리팹을 resource폴더에서 읽어오고
        GameObject obj = Instantiate(prefab, flowerPotArray[availablePlace].transform);
        //그 오브젝트를 components in pot 에 넣어준다. 그래야 꺼내서 쓸 수 있다.
        //여기서 유의할 점은 나중에 components in pot이 savedata로 다시 들어갈 텐데, 그 때는 gameobject는 저장이 안된다
        //따라서 savedata를 load할 때마다 gameobject를 프리팹에서 꺼내서 새로 만들어주어야한다.
        componentsInPot[availablePlace].realGameobject = obj;

        //지금 막 심은거니까 심은시간을 지금으로 해준다.
        componentsInPot[availablePlace].plantedTime = DateTime.Now.ToString();
        componentsInPot[availablePlace].isSprotued = false;
        componentsInPot[availablePlace].isHarvested = false;
        //변수들도 다 false로 해준다.
        gameManager.Save();
        StartCoroutine(SproutingCoroutine(availablePlace,componentData));
        StartCoroutine(PlantShake(component));
        //코루틴을 시작해준다.
    }

    //화분 돋보기 애니메이션 켜주고, 돋보기 켜주고.
    void ActiveMagnifier(int index)
    {
        magnifierArray[index].SetActive(true);          //돋보기 켜주고
        StartCoroutine(MagnifierAnimation(index));      //애니메이션 켜주고
    }

    IEnumerator MagnifierAnimation(int index)
    {
        float timer = 0;        //애니메이션 타이머
        SpriteRenderer renderer = magnifierArray[index].GetComponent<SpriteRenderer>(); //스프라이트 색 조정해줘야돼서 렌더러 가져오고
        while(timer < 2)    //타이머가 돌아갑니다
        {
            timer += Time.deltaTime;    //타이머
            renderer.color = new Color(1, 1, 1, (2 - timer)/2f);     //페이드아웃 애니메이션
            yield return null;
        }
        renderer.color = new Color(1, 1, 1, 1); //색 원상복구
        magnifierArray[index].SetActive(false);     //다시 꺼준다
    }

    //돋보기가 생긴 후 한 번 더 클릭했을 때 카메라가 이동하는 함수.
    void CameraMove(int index,bool goBack)
    {

        if (goBack)
        {
            buttonBundle.SetActive(true);
            nowMagnifiedPotIndex = -1;
            nowMagnified = false;
            //뒤로가기 눌러서 원상복구할 때.
            for (int i = 0; i < potNumber; i++)
            {
                flowerPotArray[i].SetActive(true);
            }
        }
        else
        {
            buttonBundle.SetActive(false);
            magnifierArray[index].SetActive(false);
            //뒤로가기가 아닐때. 화분 확대를 할 때. UI켜주는건 코루틴에서 한다. 다 움직이고 나서 해야되기 때문에
            nowMagnifiedPotIndex = index;           //현재 확대된 화분 인덱스 저장
            nowMagnified = true;                    //현재 확대되었음을 알려주고(사실 안해도됨, 그냥 magnifiedObject.activeSelf로 받아와도 댐)
            
            for (int i = 0; i < potNumber; i++)
            {
                if (i == index)
                {
                    flowerPotArray[i].SetActive(true);
                }
                else
                {
                    flowerPotArray[i].SetActive(false);
                }
            }
        }
        //코루틴 시작.
        StartCoroutine(CameraAnimation(index, goBack));
    }

    //확대했다가 뒤로가기버튼 누르면 실행하는 함수.
    public void MagnifyBackButton()
    {
        CameraMove(nowMagnifiedPotIndex, true);
    }

    //취소할때도 이거쓴다
    IEnumerator CameraAnimation(int index,bool goBack)
    {
        float timer = 0;    //타이머
        Vector3 startPosition;
        //카메라 시작점
        Vector3 endPosition;
        //카메라의 최종 목적지
        Vector3 potScaleStart;
        //화분이 커질지 작아질지도 lerp로 할거다. 화분의 시작 scale
        Vector3 potScaleEnd;
        //화분의 끝 scale;


        if (goBack)
        {
            //화분에서 원래위치로 돌아온다.
            startPosition = cameraObject.transform.position;   
            //카메라 시작점
            endPosition = originCameraPos;
            //끝점
            potScaleStart = new Vector3(1.5f, 1.5f, 1);
            potScaleEnd = new Vector3(1, 1, 1);
            //큰 스케일에서 작아진다.
            magnifiedUIObject.SetActive(false);
            //UI도 꺼준다
        }
        else
        {
            //화분위로 카메라가 올라간다.
            startPosition = originCameraPos;
            //카메라 시작점
            endPosition = new Vector3(flowerPotArray[index].transform.position.x, flowerPotArray[index].transform.position.y, originCameraPos.z);
            //끝점.

            potScaleStart = new Vector3(1, 1, 1);
            potScaleEnd = new Vector3(1.5f, 1.5f, 1);
            //작은 스케일에서 커진다.

        }

        while (timer < 1)
        {
            timer += Time.deltaTime;
            cameraObject.transform.position = Vector3.Lerp(startPosition, endPosition, timer);
            //Lerp는 startPosition하고 endPosition사이 지점 중,timer 값이 0은 startpos, 1은 endpos, 0.5는 그 중간지점. 그런식으로 값이 나오는 함수다.
            flowerPotArray[index].transform.localScale = Vector3.Lerp(potScaleStart, potScaleEnd, timer);
            yield return null;
        }
        if (!goBack)
        {
            //뒤로가기가 아니라면 UI를 지금 켜준다.
            magnifiedUIObject.SetActive(true);
            harvestCanvas.SetActive(false); //수확하시겠습니까 캔버스는 꺼준다.
            //만약 화분이 비어있다면.
            if (componentsInPot[index].name == "null")
            {
                //화분이 비면 프로그레스바가 없어.
                progressBar.transform.localScale = new Vector3(1, 0, 1);
                //이름도없어
                nameText.gameObject.SetActive(false);
            }
            else
            {
                //화분이 차있으면 프로그레스 바 percentage값 받아온다.
                progressBar.transform.localScale = new Vector3(1, componentsInPot[index].percentage, 1);
                
                nameText.gameObject.SetActive(true);
                nameText.text = componentsInPot[index].name;
                //이름텍스트도 켜주고 이름도 넣어준다.
            }

            Debug.Log(componentsInPot[index].isSprotued + " " + componentsInPot[index].isHarvested);
            //만약 수확 가능하다면 == 피워났고 수확은 안했다면
            if (componentsInPot[index].isSprotued == true && componentsInPot[index].isHarvested == false)
            {
                //버튼 활성화
                harvestButton.SetActive(true);
            }
            else
            {
                //버튼 없애.
                harvestButton.SetActive(false);
            }



        }

        cameraObject.transform.position = endPosition;
        flowerPotArray[index].transform.localScale = potScaleEnd;
        //lerp애니메이션이 끝날 떄 timer가 정확히 1값이 아니어서 살짝의 오차가 발생한다. 그래서 최종값으로 맞춰주어야한다.

    }

    //합성 씬 로드.
    public void ComposeSceneLoad()
    {
        gameManager.ComposeSceneLoad();
    }

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }

    public GameObject panel;
    public void PanelLoad() //일하기 버튼 팝업 켜고 끄기
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void WorkMineSceneLoad()
    {
        gameManager.WorkMineSceneLoad();
    }

    public void WorkHuntSceneLoad()
    {
        gameManager.WorkHuntSceneLoad();
    }

    public void WorkFishingSceneLoad()
    {
        gameManager.WorkFishingSceneLoad();
    }

    // Update is called once per frame
    void Update()
    {
        //abcd눌렀을 때 식물을 심는ㄴ다.
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlantComponent("mouth");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlantComponent("arm");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlantComponent("leg");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlantComponent("nose");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlantComponent("hand");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlantComponent("foot");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlantComponent("hair");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlantComponent("eye");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlantComponent("body");
        }

        //수확을 위해 터치했을 때 오브젝트를 판별하는 스크립트
        if (Input.GetMouseButtonDown(0))    //터치!
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos,Vector2.zero))
            {
                touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                for(int i = 0; i < 3; i++)
                {
                    /*
                     *아까운코드ㅠㅠ 이제 터치가 아니라 버튼이 나온다.
                    if(componentsInPot[i] != null)
                    {
                        if(touchedObject == componentsInPot[i].realGameobject)
                        {
                            //만약 같은데 수확을 해 줄 수 있으면 수확을 해 준다.
                            if(componentsInPot[i].isHarvested==false && componentsInPot[i].isSprotued == true)
                            {
                                HarvestSprout(i);
                                break;
                            }
                        }
                    }*/

                    //확대되어있지 않다면 확대를 할 수 있다.
                    if (!nowMagnified)
                    {
                        if (touchedObject == flowerPotArray[i])
                        {

                            //만약 터치한게 화분이라면
                            if (magnifierArray[i].activeSelf)
                            {
                                //화분의 돋보기가 켜져있다면 바로 카메라 무브 무브
                                CameraMove(i, false);

                            }
                            else
                            {
                                //돋보기가 안켜져있다면 돋보기부터 켜준다.
                                ActiveMagnifier(i);
                            }
                        }
                    }
                   
                }
            }
        }
    }
}
