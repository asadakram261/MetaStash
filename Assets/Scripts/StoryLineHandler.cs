using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class StoryLineHandler : MonoBehaviour
{
    public GameObject WelcomeSection;
    public GameObject StoryLine;
    public TMP_Text showText;
    public string[] instructions;
    public GameObject Character;
    public GameObject Backbutton;
    public GameObject NextButton;
    public int current = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"current {current}");
        WelcomeSection.SetActive(true);
        StartCoroutine(WelcomeScreen());
        Character.SetActive(true);
        Debug.Log($"Current value after coroutine is {current}");
    }

    public void NextInstruction()   
    {
        current++;
        if (current > 9)
        {
            SceneManager.LoadScene("Character Selection Menu");   
        }
        else
        {
            showText.text = instructions[current];
            Backbutton.SetActive(true);
        }       
    }
    public void BackInstruction()
     {
        current--;
        if (current == 0)
        {   
            Backbutton.SetActive(false);
        }
        else
        {
            
            showText.text = instructions[current];
        }  }
    IEnumerator WelcomeScreen()
    {
        yield return new WaitForSeconds(1);
        WelcomeSection.SetActive(false);
        StoryLine.SetActive(true);
        showText.text = instructions[0];
        Backbutton.SetActive(false);
    }
}
