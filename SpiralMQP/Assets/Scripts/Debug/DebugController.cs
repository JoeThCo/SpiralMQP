using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugController : MonoBehaviour
{
    public bool isDebugMode = false;
    public KeyCode ChangeDebugModeKey = KeyCode.Numlock;
    public TextMeshProUGUI DebugText;

    public static DebugController Instance;


    private void Start()
    {
        // First time run
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            SetDebugText(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetDebugText(bool state)
    {
        DebugText.gameObject.SetActive(state);
    }

    private void Update()
    {
        if (Input.GetKeyDown(ChangeDebugModeKey))
        {
            isDebugMode = !isDebugMode;

            SetDebugText(isDebugMode);
            Debug.LogWarningFormat("Debug Mode: {0}", isDebugMode);
        }
    }
}
