using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public GameObject Bitcoin;
    public GameObject Dollar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Bitcoin.transform.RotateAround(Bitcoin.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
