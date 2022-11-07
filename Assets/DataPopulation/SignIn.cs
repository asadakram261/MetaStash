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

public class SignIn : MonoBehaviour
{
    public InputField Email;
    public InputField Password;
    public GameObject Signin;
    public Text message;
    public GameObject messageBox;
    string URL = "https://dashcache.herokuapp.com/login";
    // Start is called before the first frame update
    void Start()
    {
        Button btn = Signin.GetComponent<Button>();
        btn.onClick.AddListener(signin);
    }
   
    void signin()
    {
        if (Email.text.Length == 0)
        {
            messageBox.SetActive(true);
            
            message.text = "Email required";

          //  Debug.Log("Email required");
        }
        else if (Password.text.Length == 0)
        {
            
            messageBox.SetActive(true);
            message.text = "Password required";
           // Debug.Log("Password required");
        }
        else if (Email.text.Length == 0 && Password.text.Length == 0)
        {
            messageBox.SetActive(true);
            message.text = "Email and Password required";
           //Debug.Log("Email and password required");
        }
        else if(!Email.text.EndsWith("@gmail.com"))
        {
            messageBox.SetActive(true);
            message.text = "Incomplete Email";
            //Debug.Log("Incomplete Email");
        }
        else if(Email.text.Length != 0 && Password.text.Length != 0 && Email.text.EndsWith("@gmail.com"))
        {
            StartCoroutine(Uploads());
        }

        
    }
    IEnumerator Uploads()
    {
        userSignIN sign_in = new userSignIN();
        sign_in.email = Email.text;
        sign_in.password = Password.text;
        

        Sign_In root1 = new Sign_In();
        root1.user = sign_in;
        

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(root1);
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
            //Debug.Log("response" + response);
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            PlayerData deserialized = JsonConvert.DeserializeObject<PlayerData>(response);
            // Debug.Log("Line 64 !!!!!!!!"+response);
            // Debug.Log("Id: " + deserialized.data.user.id);
            messageBox.SetActive(true);
            message.text = deserialized.data.message;
            int id = deserialized.data.user.id;
            PlayerPrefs.SetInt("playerID", id);
            string role = deserialized.data.user.role;
            PlayerPrefs.SetString("role", role); //role;
        }
    }
}
