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

public class CreatePoll : MonoBehaviour
{

    public int ngo1;
    public int ngo2;
    public int ngo3;
    public int ngo4;
    private string URL = "https://meta-stash.herokuapp.com/users/registerPoll";
    public GameObject create;
    // Start is called before the first frame update
    void Start()
    {
        RegisterPoll();
        Button btn = create.GetComponent<Button>();
        btn.onClick.AddListener(RegisterPoll);

    }
    void RegisterPoll()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {
        PollData poll = new PollData();
        poll.user_id = 2;// PlayerPrefs.GetInt("playerID");
        poll.ngo_id_1 = ngo1;
        poll.ngo_id_2 = ngo2;
        poll.ngo_id_3 = ngo3;
        poll.ngo_id_4 = ngo4;



        var json = Newtonsoft.Json.JsonConvert.SerializeObject(poll);
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
