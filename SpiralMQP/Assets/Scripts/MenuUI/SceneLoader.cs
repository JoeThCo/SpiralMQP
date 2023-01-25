using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SceneLoader
{
    public enum Scene {
        Game,
        Main,
        Credits,
        Settings
    }

    public static void LoadScene(Scene SceneToLoad) 
    {
        SceneManager.LoadScene(SceneToLoad.ToString());
    }

    public static void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
