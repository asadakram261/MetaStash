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

public class SignUp : MonoBehaviour
{
   
    public InputField Name;
    public InputField Email;
    public InputField Password;
    public InputField ConfirmPassword;
    public InputField City;
    public InputField Country;
    public GameObject Sign_UP;
    public GameObject SignInScreen;
    public GameObject SignUpScreen;
    public Text Message;
    public GameObject MessageBox;

    
    

    string URL = "https://meta-stash.herokuapp.com/signup";
    // Start is called before the first frame update
    void Start()
    {
        Button btn = Sign_UP.GetComponent<Button>();
        btn.onClick.AddListener(createuser);
        
    }
    void createuser()
    {
        if(!Regex.IsMatch(Name.text, @"^[\p{L}\p{M}' \.\-]+$") == true)
        {
            StartCoroutine(DisplayMessage("Name contains words and start with capital letter"));
        }
        else if (!Regex.IsMatch(Email.text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") == true && !Email.text.EndsWith("@gmail.com") == true  )
        {
            StartCoroutine(DisplayMessage("Invalid email"));
        }
        else if (!Regex.Match(Password.text , ConfirmPassword.text).Success)
        {
            StartCoroutine(DisplayMessage("Password doesn't match"));
        }
        else if (!Regex.IsMatch(City.text, @"^[\p{L}\p{M}' \.\-]+$") == true)
        {
            StartCoroutine(DisplayMessage("City must be a words  and start with capital letter "));
        }
        else if (!Regex.IsMatch(Country.text, @"^[\p{L}\p{M}' \.\-]+$") == true)
        {
            StartCoroutine(DisplayMessage("Country must be a words  and start with capital letter"));
        }

        else if (Name.text.Length != 0 && Email.text.Length != 0 && Password.text.Length != 0 && ConfirmPassword.text.Length != 0 && City.text.Length != 0 && Country.text.Length != 0)
        {

            StartCoroutine(Uploads());

        }

    }
    IEnumerator Uploads()
    {
        userData newuser = new userData();
        newuser.name = Name.text;
        newuser.email = Email.text;
        newuser.password = Password.text;
        newuser.password_confirmation = ConfirmPassword.text;
        newuser.city = City.text;
        newuser.country = Country.text;

        SignUP root1 = new SignUP();
        root1.user = newuser;


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
            Signup_Info deserialized = JsonConvert.DeserializeObject<Signup_Info>(response);

            if (deserialized.status == 500)
            {
                StartCoroutine(DisplayMessage("User already exists"));
            }
            else if (deserialized.status == 200)
            {
                
                SignInScreen.SetActive(true);
                SignUpScreen.SetActive(false);
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
