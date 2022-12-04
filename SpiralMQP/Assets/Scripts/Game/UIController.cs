using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private bool isPaused = false;

    MenuController menuController;

    private void Start()
    {
        menuController = GetComponent<MenuController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseController();
        }
    }

    public void PauseController()
    {
        if (GameManager.Instance.isPlaying)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

            isPaused = !isPaused;
        }
    }

    void ResumeGame()
    {
        menuController.ShowMenu("Game");
        Time.timeScale = 1;
    }

    void PauseGame()
    {
        menuController.ShowMenu("Pause");
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    public void OnGameOver()
    {
        GameManager.Instance.isPlaying = false;
        menuController.ShowMenu("GameOver");
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
}
