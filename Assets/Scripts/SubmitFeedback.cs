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
using TMPro;


public class SubmitFeedback : MonoBehaviour
{
    int ID;
    public InputField subject;
    public InputField details;
    public GameObject Submit;
    public TMP_Text message;
    public GameObject messageBox;
    string URL = "https://meta-stash.herokuapp.com/users/createFeedback";

    void Start()
    {
        ID = PlayerPrefs.GetInt("playerID");
        Button btn = Submit.GetComponent<Button>();
        btn.onClick.AddListener(CreateFeedback);
    }
    void CreateFeedback()
    {   if (ID != 0)
        {
            if (subject.text.Length == 0)
            {
                StartCoroutine(DisplayMessage("Subject Field required"));
            }
            else if (details.text.Length == 0)
            {
                StartCoroutine(DisplayMessage("Description Field required"));
            }
            else if (details.text.Length != 0 && subject.text.Length != 0)

            {
                StartCoroutine(Uploads());
            }
        }
        else
        {
            StartCoroutine(DisplayMessage("You can't give feedback in guest mode"));
        }
    }
    IEnumerator Uploads()
    {
        FeedbackData user = new FeedbackData();
        user.id = ID;
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
            string response = req.downloadHandler.text;
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            Feedback deserialized = JsonConvert.DeserializeObject<Feedback>(response);

            if(deserialized.status == 200)
            {
               

                StartCoroutine(DisplayMessage("Feedback Submitted"));
            }
        } 
    }
    IEnumerator DisplayMessage(string msg)
    {
        
        message.text = msg;
        messageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        messageBox.SetActive(false);
    }
}
