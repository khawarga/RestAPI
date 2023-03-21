using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Proyecto26;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEditor;
using Models;

public class PostMethod : MonoBehaviour
{
    InputField outputArea;

    void Start()
    {
        outputArea = GameObject.Find("Output").GetComponent<InputField>();
        GameObject.Find("Post").GetComponent<Button>().onClick.AddListener(PostData);
    }

    void PostData() => Post();

    /*IEnumerator PostData_Coroutine()
    {
        outputArea.text = "Loading...";
        string uri = "https://my-json-server.typicode.com/typicode/demo/posts";
        WWWForm form = new WWWForm();
        form.AddField("title", "test data");
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.result.Equals(UnityWebRequest.Result.ConnectionError) || request.result.Equals(UnityWebRequest.Result.ProtocolError))
                outputArea.text = request.error;
            else
                outputArea.text = request.downloadHandler.text;
        }
    }*/

    private void LogMessage(string title, string message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
    }

    public void Post()
    {

        // We can add default query string params for all requests
        RestClient.DefaultRequestParams["param1"] = "My first param";
        RestClient.DefaultRequestParams["param3"] = "My other param";

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = "https://jsonplaceholder.typicode.com/posts",
            Params = new Dictionary<string, string> {
                { "param1", "value 1" },
                { "param2", "value 2" }
            },
            Body = new Posting
            {
                id = 999,
                userId = 55,
                title = "ea",
                body = "uy"
            }
        };
        RestClient.Post<Posting>(currentRequest)
        .Then(res => {

            // And later we can clear the default query string params for all requests
            RestClient.ClearDefaultParams();

            string ea = JsonUtility.ToJson(res);

            Posting poost = JsonUtility.FromJson<Posting>(ea);

            Image img = GameObject.Find("Image").GetComponent<Image>();
            img.sprite = null;

            outputArea.text = "";

            outputArea.text = "ID :" + poost.id + "\nUserID :" + poost.userId + "\nTitle :" + poost.title + "\nBody :" + poost.body;
        })
        .Catch(err => this.LogMessage("Error", err.Message));
    }

    [Serializable]
    public class Posting
    {
        public int id;
        public int userId;
        public string title;
        public string body;
    }
}