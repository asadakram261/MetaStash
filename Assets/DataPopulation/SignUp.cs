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
    public Text message;
    
    

    string URL = "https://dashcache.herokuapp.com/signup";
    // Start is called before the first frame update
    void Start()
    {
        Button btn = Sign_UP.GetComponent<Button>();
        btn.onClick.AddListener(createuser);
        
    }
    void createuser()
    {
        if (Name.text.Length == 0)
        {
            Debug.Log("1");
            message.text = "User field required";
        }
        else if (Email.text.Length == 0)
        {
            Debug.Log("2");
            message.text = "Email field required";
        }
        else if (!Email.text.EndsWith("@gmail.com"))
        {
            Debug.Log("3");
            message.text = "Incomplete email";
        }
        else if (Password.text.Length == 0)
        {
            Debug.Log("4");
            message.text = "Password field required";
        }
        else if (ConfirmPassword.text.Length == 0)
        {
            Debug.Log("5");
            message.text = "Confirm password field required";
        }

        else if (Password.text != ConfirmPassword.text)
        {
            Debug.Log("6");
            message.text = "Password doesn't match";
        }
        else if (City.text.Length == 0)
        {
            Debug.Log("7");
            message.text = "City field  required";
        }

        else if (Country.text.Length == 0)
        {
            Debug.Log("8");
            message.text = "Country field is required";
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
                message.text = "User already exists";
            }
            else if (deserialized.status == 200)
            {
                
                SignInScreen.SetActive(true);
                SignUpScreen.SetActive(false);
            }
            
        }



    }
}
