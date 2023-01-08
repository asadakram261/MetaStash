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
    private List<int> Votes = new List<int>();
    private List<string> Name = new List<string>();
    private List<int> NGO_ID = new List<int>();
    public List<Button> VoteList = new List<Button>();
    private TMP_Text ngo_name;
    private TMP_Text ngo_votes;
    public GameObject ConfirmationBox;
    int ngoid;
    public GameObject MessageBox;
    public TMP_Text Message;
    public GameObject PollData;

    string URL = "https://meta-stash.herokuapp.com/users/getActivePolls";
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
            VoteResponse deserializedProduct = JsonConvert.DeserializeObject<VoteResponse>(response);
            if(deserializedProduct.status == 200)
            {
                PollData.SetActive(true);
            }
            if (deserializedProduct.status == 404)
            {
                PollData.SetActive(false);
                StartCoroutine(DisplayMessage("No Poll exists"));
            }


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

                    Name.Add(info.Name);
                    Votes.Add(info.Votes);
                    NGO_ID.Add(info.Ngo_id);
                }

            }

            for (int j = 0; j < itemsData["data"].Count; j++)
            {
                ngo_name = VoteList[j].transform.Find("Name").GetComponent<TMP_Text>();
                ngo_votes = VoteList[j].transform.Find("Votes").GetComponent<TMP_Text>();



                ngo_name.text = Name[j];
                ngo_votes.text = Votes[j].ToString();
            }
            for (int m = 0; m < itemsData["data"].Count; m++)
            {
                Debug.Log("id:" + NGO_ID[m]);
                Debug.Log("name:" + Votes[m]);
                Debug.Log("votes:" + Name[m]);
            }


            VoteList[0].onClick.AddListener(delegate { ButtonLanguageSelected(NGO_ID[0]); });
            VoteList[1].onClick.AddListener(delegate { ButtonLanguageSelected(NGO_ID[1]); });
            VoteList[2].onClick.AddListener(delegate { ButtonLanguageSelected(NGO_ID[2]); });
            VoteList[3].onClick.AddListener(delegate { ButtonLanguageSelected(NGO_ID[3]); });





        }
    }
    public void ButtonLanguageSelected(int a)
    {
        ngoid = a;
        Debug.Log("ngoid:  " + a);
        PlayerPrefs.SetInt("NGO-ID", a);
        ConfirmationBox.SetActive(true);

    }
    IEnumerator DisplayMessage(string msg)
    {
        MessageBox.SetActive(true);
        Message.text = msg;
        yield return new WaitForSeconds(1);
        MessageBox.SetActive(false);
   }




}
