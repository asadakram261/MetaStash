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


public class UserInfo : MonoBehaviour
{
    int Id;
    public TMP_Text Name;
    public TMP_Text Role;
    public TMP_Text City;
    public TMP_Text balance;
    
    string url = "https://meta-stash.herokuapp.com/users/getUserInformation";
    // Start is called before the first frame update


    private void OnEnable()
    {
        Id = PlayerPrefs.GetInt("playerID");
        getUser();
    }
    void getUser()
    {
        if(Id != 0)
        {
            StartCoroutine(Uploads());
           
        }
        else
        {
            Name.text = "Guest";
            Role.text = "Unpaid";
            City.text = "-";
            balance.text = "0";
        }
            
    }

    IEnumerator Uploads()
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

            Name.text = deserializedProduct.user.name;
            Role.text = deserializedProduct.user.role;
            City.text = deserializedProduct.user.city;
            balance.text = deserializedProduct.user.total_collection.ToString();

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

