using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string startMenu;
    public List<Menu> allMenus;

    public static MenuController Instance;

    private void Start()
    {
        Instance = this;
        ShowMenu(startMenu);
    }

    public void ShowMenu(string searchName)
    {
        foreach (Menu m in allMenus)
        {
            m.gameObject.SetActive(m.MenuName.Equals(searchName));
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.LogError("Quitting...");
        Application.Quit();
    }
}