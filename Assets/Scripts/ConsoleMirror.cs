using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // For UI components

public class ConsoleMirror : MonoBehaviour
{
    public GameObject textPrefab;
    public ScrollRect consoleText;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog; 
    }

    // Used to show logs in the app when debugging on site
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (consoleText != null)
        {
            GameObject textObject = Instantiate(textPrefab, consoleText.content, false);
            textObject.GetComponent<TextMeshProUGUI>().text = logString;
        }
    }
}