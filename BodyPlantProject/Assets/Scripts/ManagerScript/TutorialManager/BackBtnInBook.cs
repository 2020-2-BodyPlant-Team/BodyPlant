using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBtnInBook : MonoBehaviour
{
    TutorialMngInBook tutorialMngInBook;
    // Start is called before the first frame update
    void Start()
    {
        tutorialMngInBook = FindObjectOfType<TutorialMngInBook>();

        tutorialMngInBook.backBtn = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
