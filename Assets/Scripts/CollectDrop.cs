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

public class CollectDrop : MonoBehaviour
{
    int playerID;
    public GameObject Bitcoin;
    public GameObject Dollar;
    public GameObject BackToPlayMode;
    string spawnObj;
    public GameObject ConfirmationBox;
    public TMP_Text message;
    string URL = "https://meta-stash.herokuapp.com/users/collectDrop";
    [SerializeField]
    private Camera arcamera;
    public GameObject Cube;




    private Vector2 touchposition = default;
    // Start is called before the first frame update
    void Start()
    {

        playerID = PlayerPrefs.GetInt("playerID");
        spawnObj = PlayerPrefs.GetString("CollideObject");
        Debug.Log($"SpawnObject is {spawnObj}");
        // Instantiate(Cube);
        if (spawnObj == "Bitcoin")
        {
            Debug.Log("Bitcoin is instantiated");
            Instantiate(Bitcoin, new Vector3(0f, .5f, 10f), Quaternion.identity);
        }
        else if (spawnObj == "Dollar")
        {
            Debug.Log("Bitcoin is instantiated");
            Instantiate(Dollar, new Vector3(0f, .1f, 10f), Quaternion.identity);
        }

    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                string name = hit.collider.gameObject.name;
                Debug.Log("Name of object : " + name);
                if (name == "AR-Bitcoin Variant(Clone)" || name == "AR-Dollar Variant(Clone)")
                {
                    collectDrop();
                }
            }
        }
    }

    public void collectDrop()
    {
        Debug.Log("called successfullly");
        if (playerID != 0)
        {
            StartCoroutine(Uploads());
        }
        else
        {
            message.text = "You cannot collect the drop in Guest Mode";
            ConfirmationBox.SetActive(true);
        }

    }
    IEnumerator Uploads()
    {
        collectDrop drop = new collectDrop();
        drop.player_id = playerID;
        drop.drop_id = PlayerPrefs.GetInt("DropID");

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(drop);
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
            CollectDropData deserialized = JsonConvert.DeserializeObject<CollectDropData>(response);

            if (deserialized.status == 200)
            {
                StartCoroutine(DisplayMessage("Drop collected. \n Amount added to your account"));
            }
            else if (deserialized.status == 401)
            {
                StartCoroutine(DisplayMessage("Drop already collected"));
            }
            else if (deserialized.status == 402)
            {
                StartCoroutine(DisplayMessage("Please subscribe to collect the drop")); 
            }
        }
    }
    IEnumerator DisplayMessage(string msg)
    {
        ConfirmationBox.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(1);
        ConfirmationBox.SetActive(false);
    }



}