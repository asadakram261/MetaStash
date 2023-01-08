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

public class Vote : MonoBehaviour
{
    public Button VoteButton;
    public GameObject messageBox;
    public TMP_Text message;
    public GameObject confirmationBox;
    int playerID;
    string URL = "https://meta-stash.herokuapp.com/users/castVote";
    void Start()
    {
        playerID = PlayerPrefs.GetInt("playerID");
        Button btn = VoteButton.GetComponent<Button>();
        btn.onClick.AddListener(VoteCast);
    }


    void VoteCast()
    {
        if(playerID != 0)
        {
            StartCoroutine(Uploads());
        }
        else
        {
            confirmationBox.SetActive(false);
            messageBox.SetActive(true);
            message.text = "You are in guest mode";
        }
            
    }
    IEnumerator Uploads()
    {
        CastVote vote = new CastVote();
        vote.player_id =  PlayerPrefs.GetInt("playerID");
        vote.ngo_id = PlayerPrefs.GetInt("NGO-ID");



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

            string response = req.downloadHandler.text;
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            VoteResponse deserialized = JsonConvert.DeserializeObject<VoteResponse>(response);
            if(deserialized.status == 200)
            {
                confirmationBox.SetActive(false);
                messageBox.SetActive(true);
                message.text = "Voted successfully!!!";
            }
            else if (deserialized.status == 401)
            {
                confirmationBox.SetActive(false);
                messageBox.SetActive(true);
                message.text = "Please Subscribe to cast vote";
            }
            else if (deserialized.status == 402)
            {
                confirmationBox.SetActive(false);
                messageBox.SetActive(true);
                message.text = "You already voted";
            }
        }


    }
}
