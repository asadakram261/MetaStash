using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Subscribe : MonoBehaviour
{
    private string pay_status;

    private void Start()
    {
        pay_status = PlayerPrefs.GetString("PaymentStatus");

    }
    public void SubscribeScene()
    {
        if(pay_status == "Paid")
        {
            Debug.Log("you already paid");
        }
        else if(pay_status == "unpaid")
        {
            SceneManager.LoadScene("PaypalScreen");
        }
    }
}
