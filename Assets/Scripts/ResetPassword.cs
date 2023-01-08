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
using System.Text.RegularExpressions;
public class ResetPassword : MonoBehaviour
{
    public InputField Password;
    public InputField ConfirmPassword;
    public InputField Code;
    public GameObject Reset_Password;
    public GameObject MessageBox;
    public Text Message;
    public GameObject Signin;
    public GameObject setPassword;
    public GameObject ForgetPassword;
    string URL = "http://meta-stash.herokuapp.com/users/resetpassword";
    // Start is called before the first frame update
    void Start()
    {
         Button btn = Reset_Password.GetComponent<Button>();
         btn.onClick.AddListener(Reset);
    }
    public void Reset()
    {
        Regex validateNumberRegex = new Regex("^(?:-(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))|(?:0|(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))))(?:.\\d+|)$");

        if (!Regex.Match(Password.text, ConfirmPassword.text).Success)
        {
            StartCoroutine(DisplayMessage("Password must be same"));
        }
        else if (Code.text.Length == 0)
        {
            StartCoroutine(DisplayMessage("Enter the code"));
        }
        else
        {
            StartCoroutine(Uploads());
        }
    }
    IEnumerator Uploads()
    {
        ResetData reset = new ResetData();
        reset.password = Password.text;
        reset.token = Code.text;


        var json = Newtonsoft.Json.JsonConvert.SerializeObject(reset);
        Debug.Log("json" + json);
        var req = new UnityWebRequest(URL, "POST");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        Debug.Log("Before if");
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Result");
            Debug.Log("error: " + req.error);
        }
        else
        {
            Debug.Log("if else if");
            string response = req.downloadHandler.text;
            Debug.Log("response" + response);
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            EmailData deserializedProduct = JsonConvert.DeserializeObject<EmailData>(response);

            if (deserializedProduct.status == 200)
            {
              
                Signin.SetActive(true);
            }
            else if (deserializedProduct.status == 401)
            {
                StartCoroutine(DisplayMessage("Token expired or Wrong"));
            }
        }
    }

    IEnumerator DisplayMessage(string msg)
    {
        Message.text = msg;
        MessageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        MessageBox.SetActive(false);
    }
}
