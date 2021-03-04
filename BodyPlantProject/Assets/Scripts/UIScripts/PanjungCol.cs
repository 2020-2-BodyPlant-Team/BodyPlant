using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanjungCol : MonoBehaviour
{
    Rigidbody2D rigid;
    WorkFishingManager workFishingManager;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        workFishingManager = GameObject.Find("WorkFishingManager").GetComponent<WorkFishingManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Panjung")
        {
            workFishingManager.touchforfish = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Panjung")
        {
            workFishingManager.touchforfish = false;
        }
    }
}
