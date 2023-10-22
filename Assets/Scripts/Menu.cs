using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class Menu : MonoBehaviour {
    int menuItemSelected = -1;
    int totalItems;

    private Coroutine introSoundCoroutine;

    public EventReference menuNavigationTutorial;

    public EventReference[] menuItemEvents;
    private EventInstance[] menuItemEventInstances;  // Store EventInstances for menu items

    private EventInstance currentPlayingEventInstance; // Store the currently playing event

    public EventReference howToPlayExplanation;
    public EventReference controlsExplanation;
    public EventReference audioCuesExplanation;

    public int sceneIndexToLoad = 1;

    private bool hasPlayedCurrentItem = false;

    private void Start() {
        totalItems = menuItemEvents.Length;

        // Initialize EventInstances for menu items
        menuItemEventInstances = new EventInstance[totalItems];
        for (int i = 0; i < totalItems; i++) {
            menuItemEventInstances[i] = RuntimeManager.CreateInstance(menuItemEvents[i]);
        }

        // Start the introductory sound coroutine
        introSoundCoroutine = StartCoroutine(PlayIntroductorySound());
    }

    void Update() {
        if (menuItemSelected == -1) {
            if (Input.anyKeyDown) {
                if (introSoundCoroutine != null) {
                    StopCoroutine(introSoundCoroutine);
                    currentPlayingEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    menuItemSelected = 1;
                }
            }
            return;
        }

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
            if (currentPlayingEventInstance.isValid()) {
                currentPlayingEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            switch (menuItemSelected) {
                case 0:
                    SceneManager.LoadScene(sceneIndexToLoad);
                    break;

                case 1:
                    currentPlayingEventInstance = RuntimeManager.CreateInstance(howToPlayExplanation);
                    currentPlayingEventInstance.start();
                    break;

                case 2:
                    currentPlayingEventInstance = RuntimeManager.CreateInstance(controlsExplanation);
                    currentPlayingEventInstance.start();
                    break;

                case 3:
                    currentPlayingEventInstance = RuntimeManager.CreateInstance(audioCuesExplanation);
                    currentPlayingEventInstance.start();
                    break;

                case 4:
                    Application.Quit();
                    break;
            }
        }
    }

    private IEnumerator PlayIntroductorySound() {
        while (!Input.anyKeyDown) {
            // Play the introductory sound here at regular intervals
            currentPlayingEventInstance = RuntimeManager.CreateInstance(menuNavigationTutorial);
            currentPlayingEventInstance.start();
            yield return new WaitForSeconds(15f); // Adjust the interval as needed
        }
        
    }
}
