using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public KeyCode ChangeDebugModeKey = KeyCode.Numlock;
    public static DebugController Instance;

    public bool isDebugMode = true;

    private void Start()
    {
        // First time run
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(ChangeDebugModeKey))
        {
            isDebugMode = !isDebugMode;
            Debug.LogWarningFormat("Debug Mode: {0}", isDebugMode);
        }
    }

    private void OnGUI()
    {
        if (isDebugMode)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Debug mode enabled!");
        }
    }
}
