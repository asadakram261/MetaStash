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
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionMenu : MonoBehaviour
{
    int ID;
    public GameObject[] playerObjects;
    public string[] characterName;
    public int selectedCharacter = 0;
    public TMP_Text Name;
    public string gameScene = "Character Selection Scene";
    public string CharacterName;
    public GameObject BackButton;
    string URL = "https://meta-stash.herokuapp.com/users/changeAvatar";
    

    void Start()
    {
        ID = PlayerPrefs.GetInt("playerID");
        if (ID != 0)
        {
            BackButton.SetActive(true);
        }
        else
        {
            BackButton.SetActive(false);
        }
        HideAllCharacters();
        
        selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
        Name.text = characterName[selectedCharacter];
        playerObjects[selectedCharacter].SetActive(true);
    }


    private void HideAllCharacters()
    {
        foreach (GameObject g in playerObjects)
        {
            g.SetActive(false);
        }
    }

    public void NextCharacter()
    {
        playerObjects[selectedCharacter].SetActive(false);
       
        selectedCharacter++;
        if (selectedCharacter >= playerObjects.Length)
        {
            selectedCharacter = 0;
        }
        Name.text = characterName[selectedCharacter];
        playerObjects[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        playerObjects[selectedCharacter].SetActive(false);
        
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter = playerObjects.Length-1;
        }
        Name.text = characterName[selectedCharacter];
        playerObjects[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SelectAvatar();
        
    }
    void SelectAvatar()
    {
        if(ID != 0)
        {
            StartCoroutine(Uploads());
        }
        else
        {
            PlayerPrefs.SetInt("GuestCharacter",selectedCharacter);
            SceneManager.LoadScene("MainMenu");
        }
        
    }
    IEnumerator Uploads()
    {
        Setavatar character = new Setavatar();
        character.id = ID;
        character.avatar = selectedCharacter;
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);


        var json = Newtonsoft.Json.JsonConvert.SerializeObject(character);
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
            AvatarStatus deserialized = JsonConvert.DeserializeObject<AvatarStatus>(response);

            if (deserialized.status == 200)
            {
                SceneManager.LoadScene("MainMenu");

            }
            
        }


    }

}
