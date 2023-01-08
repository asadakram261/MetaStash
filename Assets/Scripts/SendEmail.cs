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
public class SendEmail : MonoBehaviour
{
    public InputField Email;
    public GameObject SendandEmail;
    public GameObject MessageBox;
    public Text Message;
    public GameObject SetPassword;
    public GameObject ForgetPassword;
    string URL = "https://meta-stash.herokuapp.com/users/forgotpassword";
    // Start is called before the first frame update
    void Start()
    {
        Button btn = SendandEmail.GetComponent<Button>();
        btn.onClick.AddListener(Sendemail);
    }
    public void Sendemail()
    {
        if (Regex.IsMatch(Email.text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") != true && !Email.text.EndsWith("@gmail.com"))
        {
            StartCoroutine(DisplayMessage("Invalid email"));
        }
        else
        {
            StartCoroutine(Uploads());
        }

    }
    IEnumerator Uploads()
    {
        string email = Email.text;
        
        var req = new UnityWebRequest(URL+ "/?email=" + email, "POST");

       // byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

      //  req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

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
            EmailData deserializedProduct = JsonConvert.DeserializeObject<EmailData>(response);

            if(deserializedProduct.status == 200)
            {
                StartCoroutine(DisplayMessage("Verification Code sent to your Email"));
                ForgetPassword.SetActive(false);
                SetPassword.SetActive(true);
            }
            else if(deserializedProduct.status == 404)
            {
                StartCoroutine(DisplayMessage("Email not found"));
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
