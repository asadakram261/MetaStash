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

public class GetNGOs : MonoBehaviour
{
    public Toggle SelectNGO;
    private TMP_Text ngo_name;
    List<Toggle> NGO_List = new List<Toggle>();
    List<int> NGO_ID = new List<int>();
    List<int> settings = new List<int>();
    List<int> uniqueList = new List<int>();
    int ngo1, ngo2, ngo3, ngo4;
    public GameObject Control;
    int[] ngo;
    public TMP_Text Message;
    public GameObject MessageBox;
    public GameObject Confirmation;
    public Button CreateButton;
    int Id;

    public string URL = "https://meta-stash.herokuapp.com/users/getNGOs";
    private string URL1 = "https://meta-stash.herokuapp.com/users/registerPoll";
    string url = "https://meta-stash.herokuapp.com/users/getUserInformation";
    // Start is called before the first frame update

    private void OnEnable()
    {
        Id = PlayerPrefs.GetInt("playerID");
        getNgo();
    }
    void getNgo()
    {
        StartCoroutine(Uploads());
        StartCoroutine(GetInfo());
    }
    IEnumerator Uploads()
    {
        GameObject pt = GameObject.Find("Content");
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
                foreach (NGO_Data p in Data)
                {
                    int x = -20;
                    Toggle ToggleNgo = Toggle.Instantiate(SelectNGO, new Vector3(580, x - 100, 0), Quaternion.identity);
                    ToggleNgo.transform.SetParent(pt.transform, false);
                    ToggleNgo.name = "button" + i;
                    NGO_List.Add(ToggleNgo);
                    ngo_name = NGO_List[i].transform.Find("Name").GetComponent<TMP_Text>();
                    ngo_name.text = p.name;
                    NGO_ID.Add(p.id);
                }
                for (int a = 0; a < NGO_List.Count; a++)
                {
                    int index = a;
                    Toggle t = NGO_List[a];
                    t.onValueChanged.AddListener(
                        (bool check) =>
                          {
                              Debug.Log($"Index: {index} and check: {check}");
                              if (check == true)
                              {
                                  checkBox(index + 1);
                              }
                              else if (check == false)
                              {
                                  RemoveNgo(index + 1);
                              }

                          });
                }
            }
        }
    }
    IEnumerator GetInfo()
    {

        var req = new UnityWebRequest(url + "/?id=" + Id, "GET");
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        //Check for errors
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(req.error);
        }
        else
        {
            string response = req.downloadHandler.text;
            Debug.Log("response" + response);
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            RootUser deserializedProduct = JsonConvert.DeserializeObject<RootUser>(response);

            Debug.Log(deserializedProduct.user.role);
            PlayerPrefs.SetString("role", deserializedProduct.user.role);


        }
    }
    public void CheckTrueStatus()
    {
        for (int i = 0; i < NGO_List.Count; i++)
        {
            if (NGO_List[i] == true)
            {
                settings.Add(i + 1);
            }

        }

    }
    public void RemoveNgo(int index)
    {
        settings.Remove(index);
        Debug.Log("Element is removed");
    }


    public void checkBox(int index)
    {
        Debug.Log($"Index is {index}");
        settings.Add(index);
    }
    public void showList()
    {



        for (int i = 0; i < uniqueList.Count; i++)
        {
            Debug.Log($"ID is : {uniqueList[i]}");
        }

    }
    public void CreatePoll()
    {
        Control.SetActive(false);
        uniqueList = settings.Distinct().ToList();
        if (uniqueList.Count != 4)
        {
            StartCoroutine(DisplayMessage("Please select 4 NGOs"));

        }
        else if (PlayerPrefs.GetInt("playerID") == -1 || PlayerPrefs.GetString("role") != "admin")
        {
            StartCoroutine(DisplayMessage("Only admin user can create a Poll"));
        }
        else
        {
            StartCoroutine(createPoll());
        }

        /* 
          if (PlayerPrefs.GetInt("playerID") == -1)
         {
             StartCoroutine(DisplayMessage("You are in guest mode "));
         }
         else if (PlayerPrefs.GetString("role") != "admin")
         {
             StartCoroutine(DisplayMessage("Only admin user can create a poll"));
         }
         else if (PlayerPrefs.GetString("role") == "admin")
         {*/

        // }
    }
    IEnumerator createPoll()
    {
        uniqueList = settings.Distinct().ToList();
        PollData poll = new PollData();
        poll.user_id = PlayerPrefs.GetInt("playerID");
        poll.ngo_id_1 = uniqueList[0];
        poll.ngo_id_2 = uniqueList[1];
        poll.ngo_id_3 = uniqueList[2];
        poll.ngo_id_4 = uniqueList[3];

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(poll);
        Debug.Log("json" + json);
        var req = new UnityWebRequest(URL1, "POST");
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
            Debug.Log("response" + response);
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            PollMessage deserializedProduct = JsonConvert.DeserializeObject<PollMessage>(response);
            if (deserializedProduct.status == 200)
            {
                StartCoroutine(DisplayMessage("Poll successfully created"));
            }
            else if(deserializedProduct.status == 500)
            {
                StartCoroutine(DisplayMessage("Poll already exists"));
            }
        }
    }
    IEnumerator DisplayMessage(string msg)
    {
        CreateButton.enabled = false;
        Confirmation.SetActive(false);
        Message.text = msg;
        MessageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        MessageBox.SetActive(false);
        Control.SetActive(true);
        CreateButton.enabled = true;

    }
}

