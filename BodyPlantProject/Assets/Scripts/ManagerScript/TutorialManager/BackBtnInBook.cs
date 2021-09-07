using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBtnInBook : MonoBehaviour
{
    TutorialMngInBook tutorialMngInBook;
    BookManager bookManager;
    // Start is called before the first frame update
    void Start()
    {
        tutorialMngInBook = FindObjectOfType<TutorialMngInBook>();
        bookManager = FindObjectOfType<BookManager>();

        if(tutorialMngInBook != null)
        {
            tutorialMngInBook.backBtn = gameObject;
        }
        
    }

    public void SetActiveCanvas2()
    {
        bookManager.canvas2.SetActive(true);
    }
}
