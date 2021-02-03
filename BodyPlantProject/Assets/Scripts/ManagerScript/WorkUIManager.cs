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
        //touchedObject의 정보를 받아 밑에 씬으로 넘기고 싶습니다
        gameManager.WorkMineSceneLoad();
    }

    public void NoBtn()
    {
        panel.SetActive(false);
        isPanel = false;
    }
}
