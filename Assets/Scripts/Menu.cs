using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class Menu : MonoBehaviour {
    int menuItemSelected = 1;
    int totalItems;

    public EventReference[] menuItemEvents;
    private EventInstance[] menuItemEventInstances;  // Store EventInstances for menu items

    private EventInstance currentPlayingEventInstance; // Store the currently playing event

    public int sceneIndexToLoad = 1;

    [Space(20)]
    [Header("Tutorial Audio")]
    public EventReference[] tutorialEvents;
    private EventInstance[] tutorialEventInstances;  // Store EventInstances for tutorial events

    private bool isTutorialPlaying = false;
    private bool hasPlayedCurrentItem = false;
    private void Start() {
        totalItems = menuItemEvents.Length;

        // Initialize EventInstances for menu items
        menuItemEventInstances = new EventInstance[totalItems];
        for (int i = 0; i < totalItems; i++) {
            menuItemEventInstances[i] = RuntimeManager.CreateInstance(menuItemEvents[i]);
        }
    }

    void Update() {
        // Navigate menu
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && menuItemSelected != 0) {
            Debug.Log("up");

            menuItemSelected -= 1;
            hasPlayedCurrentItem = false;
            Debug.Log("up");

            // Stop the currently playing sound
            if (currentPlayingEventInstance.isValid()) {
                currentPlayingEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        } else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && menuItemSelected != totalItems - 1) {
            menuItemSelected += 1;
            hasPlayedCurrentItem = false;
            Debug.Log("down");

            // Stop the currently playing sound
            if (currentPlayingEventInstance.isValid()) {
                currentPlayingEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }

        if (!hasPlayedCurrentItem) {
            // Play the FMOD event and store it as the currently playing event
            menuItemEventInstances[menuItemSelected].start();
            currentPlayingEventInstance = menuItemEventInstances[menuItemSelected];
            hasPlayedCurrentItem = true;
        }

        // Select and execute
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
            switch (menuItemSelected) {
                case 0:
                    SceneManager.LoadScene(sceneIndexToLoad);
                    break;

                case 1:
                    if (!isTutorialPlaying) {
                        //play tutorial
                    }
                    break;

                case 2:
                    Application.Quit();
                    break;
            }
        }
    }
}
