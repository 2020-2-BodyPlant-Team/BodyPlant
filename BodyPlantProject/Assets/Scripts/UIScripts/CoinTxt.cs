using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTxt : MonoBehaviour
{

    [SerializeField] Text CoinText;
    GameManager gameManager;        //게임매니저를 통해 세이브데이터를 참조해야 한다.
    SaveDataClass saveData;         //게임매니저를 통해 가져올 세이브 데이터
    // Start is called before the first frame update
    void Start()
    {
        CoinText.text = saveData.coin.ToString() + "c";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void substractionToCoin()
    {
        
    }
}
