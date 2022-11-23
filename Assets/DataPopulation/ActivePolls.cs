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
using TMPro;


public class ActivePolls : MonoBehaviour
{
    private List<int> NGO_ID = new List<int>();
    private List<int> Votes = new List<int>();

    public List<GameObject> VoteList = new List<GameObject>();

   string URL = "https://dashcache.herokuapp.com/users/getActivePolls";
    // Start is called before the first frame update
    private void OnEnable()
    {
        getPolls();
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

            foreach(ActivePoll info in Data)
                {
                    
                        NGO_ID.Add(info.Ngo_id);
                        Votes.Add(info.Votes);
                    
                }

            for(i = 0; i<4;i++)
                {
                    
                }

                
            }




        }



    }
    public void Display()
    {
        /*id1.text = NGO_ID[0].ToString();
        vote1.text = Votes[0].ToString();
        Debug.Log("ID:  " + NGO_ID[1] + "  Votes:" + Votes[1]);
        Debug.Log("ID:  " + NGO_ID[2].ToString() + "  Votes:" + Votes[2].ToString());
        id2.text = NGO_ID[2].ToString();
        vote2.text = Votes[2].ToString();
        Debug.Log("ID:  " + NGO_ID[1].ToString() + "  Votes:" + Votes[1].ToString());
        id3.text = NGO_ID[3].ToString();
        vote3.text = Votes[3].ToString();
        Debug.Log("ID:  " + NGO_ID[2].ToString() + "  Votes:" + Votes[2].ToString());
        id4.text = NGO_ID[4].ToString();
        vote4.text = Votes[4].ToString();
        Debug.Log("ID:  " + NGO_ID[3].ToString() + "  Votes:" + Votes[3].ToString());*/

    }
}
