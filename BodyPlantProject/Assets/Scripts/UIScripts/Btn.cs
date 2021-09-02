using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    public static GameObject scrollView;
    [SerializeField] GameObject PopUp;  // 열고 닫을 팝업창
    [SerializeField] bool isOpenBtn;  // 팝업창을 여는 버튼일 경우 true로, 아닌 경우 false로

    public void OnClick_Btn()
    {
        // 만약 팝업창을 여는 버튼일 경우 꺼져있는 팝업창을 켜준다.
        if(isOpenBtn)
        {
            PopUp.SetActive(true);
            
        }
        // 만약 팝업창을 닫는 버튼일 경우 켜져있는 팝업창을 닫아준다.
        else
        {
            PopUp.SetActive(false);
            scrollView.SetActive(true);
        }
    }
}
