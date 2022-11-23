using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using GoShared;
using System.Linq;
using MiniJSON;
using System.Collections.Generic;
using SimpleJSON;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GetNGOs : MonoBehaviour
{
    public string URL = "https://dashcache.herokuapp.com/users/getNGOs";
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(getNgo);
    }
    void getNgo()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {

        var req = new UnityWebRequest(URL, "GET");

        //byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        //byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
      
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("error: " + req.error);
        }
        else
        {
            string response = req.downloadHandler.text;

            Debug.Log(response);
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            for (int i = 0; i < itemsData["data"].Count; i++)
            {
                var Data = new List<NGO_Data>
                    {
                        new NGO_Data
                        {
                            id = itemsData["data"][i]["id"] , name = itemsData["data"][i]["name"]
                        }

                    };

                
            }



        }


    }
}
