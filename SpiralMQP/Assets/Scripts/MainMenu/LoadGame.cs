using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadGame : MonoBehaviour
{
    void Start () {
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Load);
	}

    // Load Game Scene
    void Load()
    {
        SceneManager.LoadScene("Game");
    }
}
