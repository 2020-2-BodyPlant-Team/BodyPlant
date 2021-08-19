using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
    }

    IEnumerator TutorialCoroutine()
    {
        while(saveData.isFirstPlay)
        {
            yield return null;

            
        }
    }
}
