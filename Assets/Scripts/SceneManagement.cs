using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagement : MonoBehaviour
{
    private string pay_status;
    public TMP_Text message;
    public GameObject messageBox;
    // Start is called before the first frame update
    void Start()
    {
        pay_status = PlayerPrefs.GetString("role");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void StartPlayMode()
    {
        SceneManager.LoadScene("Playmode");
    }
    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LaunchARMode()
    {
        SceneManager.LoadScene("AR-Scene");
    }
    public void CharacterSelection()
    {
        SceneManager.LoadScene("Character Selection Menu");

    }

    public void SignUpLogin()
    {
        Debug.Log("No button called");
        SceneManager.LoadScene("SignUp-LogIn");
    }
    public void GuestMode()
    {
        PlayerPrefs.SetString("Guest", "Guest");
        SceneManager.LoadScene("StoryLine");
    }
   public void Subscription()
    {
        if (pay_status == "paid" )
        {
            StartCoroutine(ShowMessage());
        }
        else if (pay_status == "unpaid")
        {
            SceneManager.LoadScene("PaypalScreen");
        }
    }

   

    IEnumerator ShowMessage()
    {
        messageBox.SetActive(true);
        message.text = "You are already a paid user";
        messageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        messageBox.SetActive(false);
    }

}
