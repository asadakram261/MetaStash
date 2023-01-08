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

public class DropDetection : MonoBehaviour
{
    private Dollar info;
    string URL = "https://meta-stash.herokuapp.com/users/collectDrop";
    public GameObject ARButton;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnCollisionEnter(Collision collision)
    {

        info = collision.gameObject.GetComponent<Dollar>();

        collectDrop drop = new collectDrop();
       
        if (collision.gameObject.tag == "Bitcoin")
        {
            Debug.Log("Bitcoin is collided");
            PlayerPrefs.SetInt("DropID", info.id);
            PlayerPrefs.SetString("CollideObject", "Bitcoin");
            ARButton.SetActive(true);

        }
        else if (collision.gameObject.tag == "Dollar")
        {
            Debug.Log("Dollar is collided");
            PlayerPrefs.SetInt("DropID", info.id);
            PlayerPrefs.SetString("CollideObject", "Dollar");
            ARButton.SetActive(true);
        }


    }
    /*private IEnumerator OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Drop")
        {

            info = collision.gameObject.GetComponent<Dollar>();

            collectDrop drop = new collectDrop();
            drop.drop_id = info.id;
            drop.player_id = PlayerPrefs.GetInt("playerID");

            collectDrop myDrop = new collectDrop();
            myDrop = drop;




            var json = Newtonsoft.Json.JsonConvert.SerializeObject(myDrop);
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

                Debug.Log("response" + req.downloadHandler.text);

                string response = req.downloadHandler.text;
                JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
                DropData deserializedProduct = JsonConvert.DeserializeObject<DropData>(response);
                
                //Debug.Log("Balance: " + deserializedProduct.balance);

            }
    



}*/
}

