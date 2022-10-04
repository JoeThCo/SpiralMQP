using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private bool isPaused = false;

    MenuController menuController;
    DungeonGeneration dungeonGeneration;

    private void Start()
    {
        menuController = GetComponent<MenuController>();
        dungeonGeneration = FindObjectOfType<DungeonGeneration>();
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
        if (dungeonGeneration.getIsPlaying())
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
    }

    public void OnGameOver()
    {
        dungeonGeneration.setIsPlaying(false);
        menuController.ShowMenu("GameOver");
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
}
