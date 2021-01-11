using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int index = 0;
    List<ComponentClass> componentsList;
    List<float> timerList;
    public GameObject imageObject;
    public Sprite unSproutedSprite;
    public Sprite sproutedSprite;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        componentsList = new List<ComponentClass>();
        timerList = new List<float>();
    }


    // Update is called once per frame
   
    void Update()
    {

        //update한번 돌아갈 때 걸리는 시간.
        
        for(int i = 0; i < index; i++)
        {
            timerList[i] += Time.deltaTime;
            if(timerList[i] > componentsList[i].sproutTime)
            {
                componentsList[i].spriteRenderer.sprite = sproutedSprite;
                //componentsList[i].realGameobject.SetActive(false);
            }
        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            componentsList.Add(new ComponentClass());
            timerList.Add(0);

            componentsList[index].realGameobject = Instantiate(imageObject);
            componentsList[index].spriteRenderer = componentsList[index].realGameobject.GetComponent<SpriteRenderer>();
            componentsList[index].spriteRenderer.sprite = unSproutedSprite;

            componentsList[index].realGameobject.transform.position = new Vector3(index, 0, 0);
            index++;
            //10초가 지나면 sproutedSprite로 sprite가 바뀐다
            
        }

        
    }
}
