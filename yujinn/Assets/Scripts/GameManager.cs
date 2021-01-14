using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Wrapper wrapper;
    public Text nameChange;

    void Start()
    {
        wrapper.componentList = new List<ComponentClass>();
        /*
        ComponentClass component1 = new ComponentClass();
        component1.name = "정상훈"; //플레이 할 때
        ComponentClass Component2 = new ComponentClass("정상훈"); //생성할 때
        */
    }

    void Add()
    {
        int randomInt = Random.Range(0, 4);
        string name = "null";
        if(randomInt == 0)
        {
            name = "정상훈";
        }
        else if(randomInt == 1)
        {
            name = "안우진";
        }
        else if(randomInt == 2)
        {
            name = "문유진";
        }
        else if(randomInt == 3)
        {
            name = "김예현";
        }

        wrapper.componentList.Add(new ComponentClass(name));
    }

    void Save()
    {
        Debug.Log("saved");
        JsonUtility.ToJson(wrapper);
        string jsonText = JsonUtility.ToJson(wrapper, true);

        FileStream fileStream = new FileStream(Application.dataPath + "/SaveData.json", FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    Wrapper Load()
    {
        Debug.Log("loading");
        FileStream stream = new FileStream(Application.dataPath + "/SaveData.json", FileMode.Open);
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Close();

        string jsonData = Encoding.UTF8.GetString(bytes);

        Wrapper data = JsonUtility.FromJson<Wrapper>(jsonData);
        return data;
    }

    IEnumerator NameCoroutine()
    {
        for (int i = 0; i < wrapper.componentList.Count; i++)
        {
            yield return new WaitForSeconds(1);
            nameChange.text = wrapper.componentList[i].name;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            wrapper = Load();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(NameCoroutine());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Add();
        }
    }
}
