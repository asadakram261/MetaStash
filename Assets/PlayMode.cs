using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
