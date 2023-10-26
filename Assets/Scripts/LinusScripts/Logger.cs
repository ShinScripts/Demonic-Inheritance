using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour {
    private string logFileName = "LogFile.txt";

    private void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }
    private void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    private void Start() {
        logFileName = Application.streamingAssetsPath + "/LogFile.txt";
    }

    private void HandleLog(string logString, string stackTrace, LogType type) {
        TextWriter tw = new StreamWriter(logFileName, true);
        
        tw.WriteLine("[" + System.DateTime.Now + "]" + logString);

        tw.Close();
    }
}
