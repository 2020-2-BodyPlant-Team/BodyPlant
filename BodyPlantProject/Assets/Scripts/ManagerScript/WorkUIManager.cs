using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkUIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject touchedObject;
    RaycastHit2D hit;
    public Camera cam;

    public int sceneNum;
    public GameObject panel;
    public bool isPanel = false;

    [SerializeField]
    List<CharacterClass> characterList;
    List<GameObject> characterObjectList;
    List<float> timerList;
    List<float> randomTimeList;
    List<Vector3> randomPosList;
    List<Vector3> startPosList;

    List<float> rotationList;
    List<float> randomRotateTimeList;
    List<GameObject> rotatingObjectList;
    List<Vector3> randomAngleList;
    List<Vector3> startAngleList;
    List<float> originAngleList;


    void Start()
    {
        gameManager = GameManager.singleTon;
        
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //flowerpotmanager에 있던 raycast로 해봤습니다
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
            if (!isPanel)
            {
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {
                    isPanel = true;
                    touchedObject = hit.collider.gameObject; 
                    panel.SetActive(true);
                }   
            }
        }
    }

    public void YesBtn()
    {
        //touchedObject에 들어간 캐릭터를 밑에 씬으로 이동시키고 싶습니다
        SceneManager.LoadScene(gameManager.workSceneIndex);
    }

    public void NoBtn()
    {
        panel.SetActive(false);
        isPanel = false;
    }
}
