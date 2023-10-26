using System;
using System.IO;
using UnityEngine;

public class ExternalParameters : MonoBehaviour
{
    [HideInInspector] public float enemy_speed;
    [HideInInspector] public bool doHeartRaise;
    [HideInInspector] public bool enableCheats;
    [HideInInspector] public bool enableTriggers;

    [SerializeField] GameObject triggers;
    private HeartbeatManager heartbeatManager;

    private void Awake()
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

                    if (variableName == "enemy_speed")
                    {
                        enemy_speed = float.Parse(variableValue);
                        Debug.Log("shouldEnemyGoFast = " + enemy_speed);
                    }
                    else if (variableName == "doHeartRaise")
                    {
                        doHeartRaise = bool.Parse(variableValue);
                        Debug.Log("doHeartRaise = " + doHeartRaise);
                    }

                    else if (variableName == "enableTriggers")
                    {
                        enableTriggers = bool.Parse(variableValue);
                        Debug.Log("enable triggers = " + enableTriggers);
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

        if (!enableTriggers)
        {
            triggers.gameObject.SetActive(false);
        }
    }
}
