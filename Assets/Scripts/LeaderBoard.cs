using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.Linq;
using MiniJSON;
using System.Collections.Generic;
using SimpleJSON;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;
using GoShared;
using TMPro;



public class LeaderBoard : MonoBehaviour
{
    private List<string> Name = new List<string>();
    private List<float> Collection = new List<float>();
    private List<int> playerID = new List<int>();
    private List<int> Rank = new List<int>();
    private List<Image> PlayersRank = new List<Image>();
    public GameObject myRank;
    public Image player;
   
    private TMP_Text playername;
    private TMP_Text playerCollection;
    private TMP_Text rank;
    private int myID ;
    string URL = "https://meta-stash.herokuapp.com/users/leaderboard";
    public void OnEnable()
    {
        
       myID = PlayerPrefs.GetInt("playerID");
        StartCoroutine(Uploads());
    }

    IEnumerator Uploads()
    {
        UnityWebRequest req = new UnityWebRequest(URL, "GET");
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
            GameObject pt = GameObject.Find("Content");
            string response = req.downloadHandler.text;
            Debug.Log(response);
            JSONNode itemsData = JSON.Parse(response);
            for (int i = 0; i < itemsData["data"].Count; i++)
            {
                var Data = new List<LeaderBoardData>
                    {
                        new LeaderBoardData
                        {
                            id = itemsData["data"][i]["id"] , name = itemsData["data"][i]["name"] , total_collection = itemsData["data"][i]["total_collection"]
                        }

                    };

                Image playerData = Instantiate(player);
                playerData.transform.SetParent(pt.transform, false);
                playerData.name = "Player" + i;
                PlayersRank.Add(playerData);
                Rank.Add(i + 1);
                foreach (LeaderBoardData info in Data)
                {
                   
                    Name.Add(info.name);
                    Collection.Add(info.total_collection);
                    playerID.Add(info.id);
                    
                    
                }

            }

            for (int j = 0; j < playerID.Count; j++)
            {
                
                playername = PlayersRank[j].transform.Find("Name").GetComponent<TMP_Text>();
                playerCollection = PlayersRank[j].transform.Find("Collection").GetComponent<TMP_Text>();
                rank = PlayersRank[j].transform.Find("Rank").GetComponent<TMP_Text>();


                playername.text = Name[j];
                playerCollection.text = Collection[j].ToString();
                rank.text = Rank[j].ToString();
               
            }
           if(PlayerPrefs.GetString("Guest") == "Guest")
            {
                playername = myRank.transform.Find("Name").GetComponent<TMP_Text>();
                playerCollection = myRank.transform.Find("Collection").GetComponent<TMP_Text>();
                rank = myRank.transform.Find("Rank").GetComponent<TMP_Text>();

                playername.text = "Guest";
                playerCollection.text = 0.ToString();
                rank.text = 0.ToString();
            }
           else
            {
                for(int c = 0; c< playerID.Count; c++)
                            {
                
                                if(myID == playerID[c])
                                {
               
                                    playername = myRank.transform.Find("Name").GetComponent<TMP_Text>();
                                    playerCollection = myRank.transform.Find("Collection").GetComponent<TMP_Text>();
                                    rank = myRank.transform.Find("Rank").GetComponent<TMP_Text>();

                                    playername.text = Name[c];
                                    playerCollection.text = Collection[c].ToString();
                                    rank.text = Rank[c].ToString();

                                }
                            }
            }
           
           

        }
    }
 }

