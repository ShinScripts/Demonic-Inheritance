using System;
using System.IO;
using UnityEngine;

public class ExternalParameters : MonoBehaviour
{
    [HideInInspector] public bool doEnemiesSpawn;
    [HideInInspector] public bool doHeartRaise;
    [HideInInspector] public bool enableCheats;

    private HeartbeatManager heartbeatManager;

    private void Start()
    {

        heartbeatManager = FindAnyObjectByType<HeartbeatManager>();

        string rulesFilePath = Path.Combine(Application.dataPath, "rules.txt");

        if (File.Exists(rulesFilePath))
        {
            string[] lines = File.ReadAllLines(rulesFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('=');

                if (parts.Length == 2)
                {
                    string variableName = parts[0].Trim();
                    string variableValue = parts[1].Trim();

                    if (variableName == "doEnemiesSpawn")
                    {
                        doEnemiesSpawn = bool.Parse(variableValue);
                        Debug.Log("doEnemiesSpawn = " + doEnemiesSpawn);
                    }
                    else if (variableName == "doHeartRaise")
                    {
                        doHeartRaise = bool.Parse(variableValue);
                        Debug.Log("doHeartRaise = " + doHeartRaise);
                    }
                    else if (variableName == "enableCheats")
                    {
                        enableCheats = bool.Parse(variableValue);
                        Debug.Log("enableCheats = " + enableCheats);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("rules.txt not found.");
        }

        

        if (doHeartRaise)
        {

        }
        else
        {
            heartbeatManager.DisableUpdates();
        }

        if (enableCheats)
        {

        }
    }
}
