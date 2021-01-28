using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkMineManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    WholeComponents wholeComponents;

    public Image barImage;
    float maxBar = 100f;
    public static float barAmount;
    

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }
    
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wholeComponents = gameManager.wholeComponents;

        barImage = GetComponent<Image>();
        barAmount = 0f;
        
    }
    
    void Update()
    {
        barImage.fillAmount = barAmount / maxBar;
    }

    public void FillingBar()
    {
        barAmount += 10f;
    }

    
}
