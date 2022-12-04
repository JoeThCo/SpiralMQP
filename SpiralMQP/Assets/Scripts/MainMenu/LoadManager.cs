using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public void LoadCredits() 
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadMainMenu() 
    {
        SceneManager.LoadScene("Main");
    }

     public void LoadSetting() 
    {
        SceneManager.LoadScene("Setting");
    }
}
