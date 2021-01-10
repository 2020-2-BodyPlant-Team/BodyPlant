using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    List<ComponentClass> componentList;
    public GameObject imageObject;
    int NumberOfPlants = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if(Input.GetKeyDown("a"))
        {            
            Planting();           
            NumberOfPlants++;            
        }        
    }
    
    void Planting()
    {
        componentList.Add(new ComponentClass());
        GameObject obj = Instantiate(imageObject);
        componentList[NumberOfPlants].realGameObject = obj;
        componentList[NumberOfPlants].spriteRenderer = obj.GetComponent<SpriteRenderer>();
        componentList[NumberOfPlants].spriteRenderer.sprite = componentList[NumberOfPlants].unSproutedSprite;    
    }
    
}
