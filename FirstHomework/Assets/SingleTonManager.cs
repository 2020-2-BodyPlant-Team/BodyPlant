using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleTonManager : MonoBehaviour
{
    public static SingleTonManager singleTon;

    ComponentClass component1;
    ComponentClass component2;
    //전체 객체 중에서 단 하나의 값을 공유한다.
    
    void Start()
    {
        if(singleTon == null)
        {
            singleTon = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
