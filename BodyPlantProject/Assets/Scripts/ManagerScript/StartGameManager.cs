using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
    }

    public void PrologueSceneLoad()
    {
        gameManager.PrologueSceneLoad();
    }

    public void HouseSceneLoad()
    {
        gameManager.HouseSceneLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
