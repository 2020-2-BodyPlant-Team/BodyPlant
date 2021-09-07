using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBtn : MonoBehaviour
{
    TutorialMngInBook tutorialMngInBook;
    GameManager gameManager;
    SaveDataClass saveData;

    // Start is called before the first frame update
    void Start()
    {
        tutorialMngInBook = FindObjectOfType<TutorialMngInBook>();
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
    }

    public void backBtnClicked()
    {
        if(tutorialMngInBook != null)
        {
            if(tutorialMngInBook.textOrder == 10)
            {
                tutorialMngInBook.isBackBtnClicked = true;
            }

            else if(tutorialMngInBook.textOrder == 11)
            {
                tutorialMngInBook.isBackToHouseBtnClicked = true;
            }

            else if(tutorialMngInBook.textOrder == 12)
            {
                saveData.tutorialOrder++;
                gameManager.Save();
            }
        }
        
    }
}
