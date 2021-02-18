using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleDeer : MonoBehaviour
{
    Vector3 pos;
    float delta = 8.0f;
    float speed = 3.0f;

    Animator fdAni;

    void Start()
    {
        fdAni = GetComponent<Animator>();
        pos = transform.position;
    }
    
    void Update()
    {
        fdAni.SetBool("isRunnig",true);
        Vector3 v = pos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
