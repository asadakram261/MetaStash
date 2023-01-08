using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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


public class SignIn : MonoBehaviour
{
    public InputField Email;
    public InputField Password;
    public GameObject Signin;
    public Text Message;
    public GameObject MessageBox;
    string ExistingEmail;
    string ExistingPassword;


    string URL = "https://meta-stash.herokuapp.com/login";
    string url = "https://meta-stash.herokuapp.com/users/getUserInformation";
    // Start is called before the first frame update
    private void OnEnable()
    { 
        ExistingEmail = PlayerPrefs.GetString("Email");
        ExistingPassword = PlayerPrefs.GetString("Password");
        Debug.Log(ExistingEmail.Length);
        Debug.Log(ExistingPassword.Length);
        
        if(ExistingEmail.Length != 0)
        {
           
            StartCoroutine(Uploads());
        }
    
        //UserExist = PlayerPrefs.GetString("Email");
        Button btn = Signin.GetComponent<Button>();
        btn.onClick.AddListener(signin);


    }
   
    void signin()
    {       
        if(Regex.IsMatch(Email.text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") != true && !Email.text.EndsWith("@gmail.com"))
        {
            StartCoroutine(DisplayMessage("Invalid email"));
        }
        else if (Password.text.Length == 0)
        {
            StartCoroutine(DisplayMessage("Password required"));
        }

        else if (Email.text.Length == 0 && Password.text.Length == 0)
        {

            //Debug.Log("Email and password required"); 
        }
        else if (!Email.text.EndsWith("@gmail.com"))
        {
            StartCoroutine(DisplayMessage("Incomplete Email")); 
        }
        else if (Email.text.Length != 0 && Password.text.Length != 0 && Email.text.EndsWith("@gmail.com"))
        {
            StartCoroutine(Uploads());
        }
        

        
    }
    IEnumerator Uploads()
    {
        
        userSignIN sign_in = new userSignIN();



        if (ExistingEmail.Length == 0)
        {
            sign_in.email = Email.text;
            sign_in.password = Password.text;
        }
        else if (ExistingEmail.Length != 0)
        {

            sign_in.email = ExistingEmail;
            sign_in.password = ExistingPassword;
        }


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
            Debug.Log(response);
            JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
            SignInResponse deserialized = JsonConvert.DeserializeObject<SignInResponse>(response);
            
          
          if (deserialized.data.status == 200)
            {
                int id = deserialized.data.user.id;
                PlayerPrefs.SetInt("playerID", id);//player ID
                string role = deserialized.data.user.role;
                PlayerPrefs.SetString("role", role);
                //loader.SetActive(false);
                StartCoroutine(check(deserialized.data.user.id));
                PlayerPrefs.SetString("Email", Email.text);
                PlayerPrefs.SetString("Password", Password.text);
                PlayerPrefs.Save();
            }
            if (deserialized.data.status == 404)
            {
                StartCoroutine(DisplayMessage("No User Exist with this Email!"));
            }
            else if (deserialized.data.status == 401)
            {
                StartCoroutine(DisplayMessage("Invalid Password!"));
            }

        }
    }
    
    IEnumerator check(int a)
    {
        var req = new UnityWebRequest(url + "/?id=" + a, "GET");
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

            if(deserializedProduct.user.avatar == -1)
            {
                SceneManager.LoadScene("StoryLine");
            }
            else
            {
                PlayerPrefs.SetInt("selectedCharacter", deserializedProduct.user.avatar);
                Debug.Log("Avatar Value: "+ deserializedProduct.user.avatar);
                SceneManager.LoadScene("MainMenu");

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
