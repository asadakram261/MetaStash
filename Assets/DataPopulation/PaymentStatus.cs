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

public class PaymentStatus : MonoBehaviour
{
    public bool payment = true;
    string URL = "https://dashcache.herokuapp.com/users/updateUserPaymentStatus";
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(updatePayment);

    }

    void updatePayment()
    {
        StartCoroutine(Uploads());
    }
    IEnumerator Uploads()
    {

        PaymentData pay = new PaymentData();
        pay.id = PlayerPrefs.GetInt("playerID");

        pay.payment_status = payment;

        PaymentData paid = new PaymentData();
        paid = pay;


        var json = Newtonsoft.Json.JsonConvert.SerializeObject(pay);
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

            Debug.Log(req.downloadHandler.text);
        }


    }
}
