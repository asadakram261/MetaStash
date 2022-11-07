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

public class Vote : MonoBehaviour
{
    public int NGO_ID;
    string URL = "https://dashcache.herokuapp.com/users/castVote";
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(VoteCast);
    }
    void VoteCast()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {
        CastVote vote = new CastVote();
        vote.player_id = PlayerPrefs.GetInt("playerID");
        vote.ngo_id = NGO_ID;



        var json = Newtonsoft.Json.JsonConvert.SerializeObject(vote);
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


            Debug.Log(req.downloadHandler.text);
        }


    }
}
