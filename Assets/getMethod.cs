using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Proyecto26;
using System;
using UnityEditor;

public class getMethod : MonoBehaviour
{
    InputField outputArea;

    private void Start()
    {
        outputArea = GameObject.Find("Output").GetComponent<InputField>();
        GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(GetData);
    }
    string fixJson(string value)
    {
        value = "{\"ListFruit\":" + value + "}";
        return value;
    }


    void GetData() => onClick();

    /*IEnumerator getData_courotine()
    {
        outputArea.text = "Loading...";
        string uri = "https://fruityvice.com/api/fruit/all";

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result.Equals(UnityWebRequest.Result.ConnectionError) || request.result.Equals(UnityWebRequest.Result.ProtocolError))
                outputArea.text = request.error;
            else
            {
                string jsonString = fixJson(request.downloadHandler.text);

                var datalist_json = JsonConvert.DeserializeObject<DataList.FruitList>(jsonString);

                outputArea.text = "";

                foreach (DataList.Fruit x in datalist_json.ListFruit)
                {
                    outputArea.text = outputArea.text + "Name : " + x.name + "\nGenus : " + x.genus + "\nFamily : " + x.family + "\nOrder : " + x.order + "\n\n";
                }
            }
        }
    }*/

    public void onClick()
    {
        var usersRoute = "https://fruityvice.com/api/fruit/all";
        RestClient.GetArray<DataList.Fruit>(usersRoute).Then(allUsers => {

            string json = JsonHelper.ArrayToJsonString(allUsers, true);

            var datalist_json = JsonConvert.DeserializeObject<DataList.FruitList>(json);

            outputArea.text = "";

            Image img = GameObject.Find("Image").GetComponent<Image>();
            img.sprite = null;

            foreach (DataList.Fruit x in datalist_json.Items)
            {
                outputArea.text = outputArea.text + "Name : " + x.name + "\nGenus : " + x.genus + "\nFamily : " + x.family + "\nOrder : " + x.order + "\n\n";
            }
        });
    }
}
