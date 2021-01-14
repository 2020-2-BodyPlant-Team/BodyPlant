using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Wrapper wrapper;
    [SerializeField] Text Text;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        wrapper.componentList = new List<ComponentClass>();
    }

    // Update is called once per frame
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Add();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {

            Print();
        }
    }

    void Add()
    {
        int randomInt = Random.Range(0, 3);
        string name = "null";
        if (randomInt == 0)
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
        else if (randomInt == 3)
        {
            name = "김예현";
        }
        wrapper.componentList.Add(new ComponentClass(name));
        Debug.Log("added");
        index++;
    }

    void Save()
    {
        Debug.Log("saved");
        JsonUtility.ToJson(wrapper);
        string jsonText = JsonUtility.ToJson(wrapper,true);

        FileStream fileStream = new FileStream(Application.dataPath + "/SaveData.json", FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    Wrapper Load()
    {
        Debug.Log("loaded");
        FileStream stream = new FileStream(Application.dataPath + "/SaveData.json", FileMode.Open);
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Close();

        string jsonData = Encoding.UTF8.GetString(bytes);

        Wrapper data = JsonUtility.FromJson<Wrapper>(jsonData);
        return data;
    }

    void Print()
    {

        StartCoroutine(WaitCoroutine());

    }

    IEnumerator WaitCoroutine()
    {

        for (int i = 0; i < wrapper.componentList.Count; i++)
        {
            Text.text = wrapper.componentList[i].name;

            yield return new WaitForSeconds(1);
        }


    }

}
