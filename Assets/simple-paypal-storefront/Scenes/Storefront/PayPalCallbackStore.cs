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




public class PayPalCallbackStore : PayPallCallbackBase_V1
{

    public static PayPalCallbackStore INSTANCE;
    public bool payment = true;
    string URL = "https://meta-stash.herokuapp.com/users/updateUserPaymentStatus";

    void Awake()
    {
        INSTANCE = this;
    }

    public string payId;

    public override void createPaymentSuccess_CallBack(PayPalCreatePaymentJsonResponse payPalCreatePaymentJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore createPaymentSuccess_CallBack()...");
        payId = payPalCreatePaymentJsonResponse.id;
        StoreActions.INSTANCE.changePurchaseStatus(StoreActions.PurchaseStatus.CHECKOUT_READY);
    }

    public override void executePaymentSuccess_CallBack(PayPalExecutePaymentJsonResponse payPalExecutePaymentJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore executePaymentSuccess_CallBack()...");

        if (payPalExecutePaymentJsonResponse.state == "approved")
        {
            StartCoroutine(Uploads());

            StoreActions.INSTANCE.changePurchaseStatus(StoreActions.PurchaseStatus.COMPLETE);
        }
        else
        {
            StoreActions.INSTANCE.changePurchaseStatus(StoreActions.PurchaseStatus.INCOMPLETE);
        }

    }

    public override void getAccessTokenSuccess_CallBack(PayPalGetAccessTokenJsonResponse payPalGetAccessTokenJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore getAccessTokenSuccess_CallBack()...");

        StoreActions.INSTANCE.OpenStore();
        DialogScreenActions.INSTANCE.HideDialogScreen();
        StoreStartupBehaviour.INSTANCE.accessToken = payPalGetAccessTokenJsonResponse.access_token;
    }

    public override void payPalFailure_Callback(PayPalErrorJsonResponse payPalErrorJsonResponse)
    {
        Debug.LogWarning("entered PayPallCallbackStore payPalFailure_Callback()...");
        DialogScreenActions.INSTANCE.setContextStoreLoadFailure();
    }

    public override void showPaymentSuccess_CallBack(PayPalShowPaymentJsonResponse payPalShowPaymentJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore showPaymentSuccess_CallBack()...");

        if (payPalShowPaymentJsonResponse.payer.status == "VERIFIED")
        {
            ExecutePaymentAPI_Call apiCall = this.gameObject.AddComponent<ExecutePaymentAPI_Call>();
            apiCall.payPallCallbackBase = INSTANCE;

            apiCall.accessToken = StoreStartupBehaviour.INSTANCE.accessToken;
            apiCall.paymentID = payId;
            apiCall.payerID = payPalShowPaymentJsonResponse.payer.payer_info.payer_id;
        }
        else
        {
            StartCoroutine("retryCallShowPayment_API", payPalShowPaymentJsonResponse);
        }
    }

    IEnumerator retryCallShowPayment_API(PayPalShowPaymentJsonResponse payPalShowPaymentJsonResponse)
    {
        Debug.Log("Waiting 5 seconds before retrying call to ShowPaymentAPI...");
        yield return new WaitForSeconds(5);

        if (StoreActions.INSTANCE.currentPurchaseStatus == StoreActions.PurchaseStatus.WAITING)
        {
            ShowPaymentAPI_Call lastAPIcall = this.GetComponent<ShowPaymentAPI_Call>();

            if (lastAPIcall != null)
            {
                Destroy(lastAPIcall);
            }

            Debug.Log("creating new api call");
            ShowPaymentAPI_Call apiCall = this.gameObject.AddComponent<ShowPaymentAPI_Call>();
            apiCall.payPallCallbackBase = INSTANCE;

            apiCall.accessToken = StoreStartupBehaviour.INSTANCE.accessToken;
            apiCall.payID = payId;
        }

    }
    IEnumerator Uploads()
    {

        PaymentData pay = new PaymentData();
        pay.id = PlayerPrefs.GetInt("playerID");

        pay.payment_status = true;

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
            string response = req.downloadHandler.text;
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            Payment_status deserialized = JsonConvert.DeserializeObject<Payment_status>(response);
            if (deserialized.status == 200)
            {
                PlayerPrefs.SetString("role", "paid");
            }
        }


    }

}
