using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SM : MonoBehaviour

{
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Script worked");
    }

    // Update is called once per frame
    public void sceneshift()
    {
        Debug.Log("Worked");
        SceneManager.LoadScene("StorefrontDemoScene");
    }
}
