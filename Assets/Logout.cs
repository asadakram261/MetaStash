using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;

public class Logout : MonoBehaviour
{
   public void UserLogout()
    {
        SceneManager.LoadScene("SignUp-LogIn");
    }
}
