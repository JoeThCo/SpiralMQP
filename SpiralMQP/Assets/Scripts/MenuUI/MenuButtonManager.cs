using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonManager : MonoBehaviour
{
    public void onClick(string ButtonName){
        switch(ButtonName){
            case "NewGame":
                SceneLoader.LoadScene(SceneLoader.Scene.Game);
                break;
            case "Credits":
                SceneLoader.LoadScene(SceneLoader.Scene.Credits);
                break;
            case "Settings":
                SceneLoader.LoadScene(SceneLoader.Scene.Settings);
                break;
            case "QuitGame":
                Application.Quit();
                break;
        }
    }
}
