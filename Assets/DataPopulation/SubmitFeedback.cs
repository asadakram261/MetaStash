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


public class SubmitFeedback : MonoBehaviour
{
    int ID;
    public InputField subject;
    public InputField details;
    public GameObject Submit;
    string URL = "https://dashcache.herokuapp.com/users/createFeedback";

    void Start()
    {
        Button btn = Submit.GetComponent<Button>();
        btn.onClick.AddListener(CreateFeedback);
    }
    void CreateFeedback()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {
        FeedbackData user = new FeedbackData();
        user.id = PlayerPrefs.GetInt("playerID");
        user.message_heading = subject.text;
        user.message_details = details.text;

        FeedbackData root1 = new FeedbackData();
        root1 = user;


        var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        Debug.Log("json" + json);
        ID = 1; // PlayerPrefs.GetInt("playerID");
        var req = new UnityWebRequest(URL + "/?id=" + ID, "POST");

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
