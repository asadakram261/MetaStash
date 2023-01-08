using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;
using GoShared;
using System.Linq;
using MiniJSON;
using SimpleJSON;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class DropInfo : MonoBehaviour
{
    private int dropID = 1;
    private int playerID = 1;
    string URL = "https://dashcache.herokuapp.com/users/collectDrop";
    private DropDetection detection;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(collectDrop);

    }

    void collectDrop()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {

        collectDrop drop = new collectDrop();
        drop.drop_id = dropID;
        drop.player_id = playerID;

        collectDrop myDrop = new collectDrop();
        myDrop = drop;




        var json = Newtonsoft.Json.JsonConvert.SerializeObject(myDrop);
        Debug.Log("json" + json);
        var req = new UnityWebRequest(URL, "POST");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("error: " + req.error);
        }
        else
        {
            Debug.Log("response" + req.downloadHandler.text);

            string response = req.downloadHandler.text;
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            DropData deserializedProduct = JsonConvert.DeserializeObject<DropData>(response);
            Debug.Log("Balance: " + deserializedProduct.balance);



        }


    }
}
