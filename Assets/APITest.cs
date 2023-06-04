using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;



public class APITest : MonoBehaviour
{
    public string URL;
    public JSONTest jsonTest = new JSONTest();


    // Start is called before the first frame update
    void Start()
    {
        URL = "https://646c39fa7b42c06c3b2ad805.mockapi.io/user";
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;

                // jsonTest = JsonConvert.DeserializeObject<JSONTest>(output);
                Debug.Log(json);
                RootObject obj = JsonUtility.FromJson<RootObject>("{\"jsonTests\":" + json + "}");
                foreach(JSONTest test in obj.jsonTests)
                {
                    Debug.Log(test.message);
                }
            }
        }
    }
}

[Serializable]
public class RootObject
{
    public JSONTest[] jsonTests;
}

[Serializable]
public class JSONTest
    {
        public string message;
        public string happinessLevel;
        public string id;
    }

