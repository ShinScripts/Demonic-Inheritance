using System;
using System.IO;
using UnityEngine;

public class ExternalParameters : MonoBehaviour {
    // Define variables to store rules
    public bool doEnemiesSpawn;
    public bool doHeartRaise;
    public bool enableCheats;

    private void Start() {
        string rulesFilePath = Path.Combine(Application.dataPath, "rules.txt");

        if (File.Exists(rulesFilePath)) {
            string[] lines = File.ReadAllLines(rulesFilePath);

            foreach (string line in lines) {
                string[] parts = line.Split('=');

                if (parts.Length == 2) {
                    string variableName = parts[0].Trim();
                    string variableValue = parts[1].Trim();

                    // Parse and set the variable value
                    if (variableName == "doEnemiesSpawn")
                        doEnemiesSpawn = bool.Parse(variableValue);
                    else if (variableName == "doHeartRaise")
                        doHeartRaise = bool.Parse(variableValue);
                    else if (variableName == "enableCheats")
                        enableCheats = bool.Parse(variableValue);
                }
            }
        } else {
            Debug.LogError("rules.txt not found.");
        }

        // Use the rules to adjust the game behavior as needed
        if (doEnemiesSpawn) {
            
        } else {
            
        }

        if (doHeartRaise) {
            // Enable heart raise logic
        } else {
            
        }

        if (enableCheats) {
            
        }
    }
}
