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


public class ActivePolls : MonoBehaviour
{
    string URL = "https://dashcache.herokuapp.com/users/getActivePolls";
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(getPolls);
    }

    void getPolls()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {

        var req = new UnityWebRequest(URL, "GET");

        //byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        //req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

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
            JSONNode itemsData = JSON.Parse(response);
            for (int i = 0; i < itemsData["data"].Count; i++)
            {
                var Data = new List<ActivePoll>
                    {
                        new ActivePoll
                        {
                            Ngo_id = itemsData["data"][i]["Ngo_id"] , Name = itemsData["data"][i]["Name"] , Votes = itemsData["data"][i]["Votes"]
                        }

                    };

                foreach (ActivePoll info in Data)
                {
                    Debug.Log("Info" + info.Ngo_id + "Name" + info.Name + "Votes" + info.Votes);
                }

            }



        }



    }
}
