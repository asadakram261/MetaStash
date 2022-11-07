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


public class UserInfo : MonoBehaviour
{
    int Id;
    public Text balance;
    string url = "https://dashcache.herokuapp.com/users/getUserInformation";
    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponent<Button>().onClick.AddListener(getUser);
    }

    void getUser()
    {
        StartCoroutine(Uploads());
    }

    IEnumerator Uploads()
    {
        Id = PlayerPrefs.GetInt("playerID");
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



            Debug.Log("Email: " + deserializedProduct.user.email);
            Debug.Log("Name: " + deserializedProduct.user.name);
            Debug.Log("Role: " + deserializedProduct.user.role);
            Debug.Log("City: " + deserializedProduct.user.city);
            Debug.Log("Country: " + deserializedProduct.user.country);
            Debug.Log("Payment Status: " + deserializedProduct.user.payment_status);
            Debug.Log("Vote casted: " + deserializedProduct.user.vote_casted);
            Debug.Log("Total Collection: " + deserializedProduct.user.total_collection);
           // balance.text = "Balance" + deserializedProduct.user.total_collection.ToString();


        }
    }
}

