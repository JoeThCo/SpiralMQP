using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Play main menu music
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);
    }


    /// <summary>
    /// Called from the Play Button
    /// </summary>
    public void PlayGame()
    {
        LoadingManager.Instance.LoadAScene("Game");
    }

    public void LoadMenu()
    {
        LoadingManager.Instance.LoadAScene("MainMenu");
    }

    public void LoadCredits()
    {
        LoadingManager.Instance.LoadAScene("Credits");
    }


    /// <summary>
    /// Terminate and quit and game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
