using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstaniateObjects : MonoBehaviour
{
     List<Button> NgoList = new List<Button>();
    public Button ngo;
   
    // Start is called before the first frame update
    void Start()
    {
       
       
        
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(NgoList.Count);
    }
}
