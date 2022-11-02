using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadGame : MonoBehaviour
{
    // void Start () {
	// 	Button btn = gameObject.GetComponent<Button>();
	// 	btn.onClick.AddListener(LoadMain);
	// }

    // Load Game Scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }


     // Load Game Scene
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

     // Load Game Scene
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }
}
