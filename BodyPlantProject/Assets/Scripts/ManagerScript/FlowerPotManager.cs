using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// saveData를 나중에 gameManager로 굳이 넘겨줄 필요는 없다.다만 gameManager.saveData = saveData로 넘겨줄 수 있을 수도 있으니까 나중을 위하여 이렇게 쓴다.
    /// </summary>
    WholeComponents wholeComponents;    //전체 컴포넌트 리스트

    ComponentClass[] componentsInPot;   //화분에 들어가있는 부위들이다.
    public GameObject[] flowerPotArray; //이거는 에디터에서 드래그앤드롭해준다.

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    public int potNumber = 3;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;    //싱글톤을하면 이런식으로 가져올 수 있다. 편하다.
        saveData = gameManager.saveData;
        componentsInPot = saveData.potList;
        wholeComponents = gameManager.wholeComponents;
        //초깃값을 다 설정해준다. gameManager에 있으니까 설정해준다.

        //화분에 있는 부위들을 먼저 가져온다.
        for(int i = 0; i < potNumber; i++)
        {
            int elapsedTime = 0;
            //만약 화분이 비어있다면 다음 인덱스로 넘기기
            if(componentsInPot[i].componentData.name == "null")
            {
                continue;
            }
            elapsedTime = gameManager.TimeSubtractionToSeconds(componentsInPot[i].plantedTime, DateTime.Now.ToString());
            //지난 시간을 구해주고 그것이 건설시간보다 큰지 체크해준다.
            if(elapsedTime > componentsInPot[i].componentData.sproutSeconds)
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
                GameObject prefab = Resources.Load<GameObject>("Components/" + componentsInPot[i].componentData.name);
                Debug.Log(componentsInPot[i].componentData.name);
                //먼저 프리팹을 resource폴더에서 읽어오고
                GameObject obj = Instantiate(prefab, flowerPotArray[i].transform);
                //그 오브젝트를 components in pot 에 넣어준다. 그래야 꺼내서 쓸 수 있다.
                //여기서 유의할 점은 나중에 components in pot이 savedata로 다시 들어갈 텐데, 그 때는 gameobject는 저장이 안된다
                //따라서 savedata를 load할 때마다 gameobject를 프리팹에서 꺼내서 새로 만들어주어야한다.
                componentsInPot[i].realGameobject = obj;
                StartCoroutine(SproutingCoroutine(i));
                //코루틴을 시작해준다.
            }
            else if(componentsInPot[i].isHarvested == false)
            {
                //만약 자라긴 자랐는데 수확이 안된 경우.
                GameObject prefab = Resources.Load<GameObject>("Components/" + componentsInPot[i].componentData.name);
                //먼저 프리팹을 resource폴더에서 읽어오고
                GameObject obj = Instantiate(prefab, flowerPotArray[i].transform);
                //위와 같다. 다만 코루틴 작동을 하지 않는다.
                componentsInPot[i].realGameobject = obj;
                obj.transform.localPosition = componentsInPot[i].componentData.sproutingPosition;
            }

        }


    }

    IEnumerator SproutingCoroutine(int index)
    {
        //지나간 시간

        //꽃피지 않을때만 돌아간다.
        while(componentsInPot[index].isSprotued == false)
        {
            int elapsedTime = 0;
            //몇퍼센트 완성인지.
            float percentage = 0;
            //포지션을 업데이트 해준다. sproutingPosition이 최종 위치니까, 이거에 percentage를 곱해서 해준다.
            yield return new WaitForSeconds(1);
            elapsedTime = gameManager.TimeSubtractionToSeconds(componentsInPot[index].plantedTime, DateTime.Now.ToString());
            if (componentsInPot[index].componentData.sproutSeconds < elapsedTime)
            {
                componentsInPot[index].isSprotued = true;
                //만약 시간이 지났다면 싹틔워준다.
            }
            percentage = elapsedTime / componentsInPot[index].componentData.sproutSeconds;
            componentsInPot[index].realGameobject.transform.localPosition = percentage * componentsInPot[index].componentData.sproutingPosition;
            //   1초에 한번씩 업데이트를 해준다.
           
        }
        //이제 업데이트를 다 해주다가 isSprotued==true가 돼서 탈출을 하게 되면, 수확을 해주어야 한다
        componentsInPot[index].isHarvested = false;
    }

    private void OnApplicationQuit()
    {
        gameManager.Save();
    }

    //심은걸 수확하는거. 터치해서 수확. update에서 호출됨
    void HarvestSprout(int index)
    {
        Debug.Log("이거 되긴 하냐" + index);
        //수확을 할 때에는 먼저 오브젝트를 없애주고
        Destroy(componentsInPot[index].realGameobject);
        //수확이 되었다는거를 세이브해줘야하니까 세이브데이터에 넣어주고
        saveData.owningComponentList.Add(componentsInPot[index]);
        //수확이 된 거는 아무것도 없는 빈 객체를 넣는다.
        componentsInPot[index] = new ComponentClass();
        //지워준 정보도 savedata에 저장을 한다
        //saveData.potList = componentsInPot;
    }

    //상점에서 호출할거 
    public void PlantComponent(string name)
    {
        //자리가 있는지부터 체크해준다.
        bool isPlaceAvailable = false;
        int availablePlace = -1;
        for(int i = 0; i < potNumber; i++)
        {

            //아무것도 심어져있지 않으면 null이다.
            if(componentsInPot[i].componentData.name == "null")
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
        foreach(ComponentDataClass com in wholeComponents.componentList)
        {
            if(com.name == name)
            {
                component.componentData = com;
                break;
            }
        }
        if(component == null)
        {
            Debug.Log("부위가 없다");
            return;
        }
        Debug.Log(availablePlace);
        componentsInPot[availablePlace] = component;
        //이 아래부터는 start에 있는거랑 똑같다.
        GameObject prefab = Resources.Load<GameObject>("Components/" + componentsInPot[availablePlace].componentData.name);
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

        StartCoroutine(SproutingCoroutine(availablePlace));
        //코루틴을 시작해준다.
    }

    // Update is called once per frame
    void Update()
    {
        //수확을 위해 터치했을 때 오브젝트를 판별하는 스크립트
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
        if (Input.GetMouseButtonDown(0))    //터치!
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos,Vector2.zero))
            {
                touchedObject = hit.collider.gameObject; //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                for(int i = 0; i < 3; i++)
                {
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
                    }
                }
            }
        }
    }
}
