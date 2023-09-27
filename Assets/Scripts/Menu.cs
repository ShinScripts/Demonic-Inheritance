using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    int menuItemSelected = 1;
    int totalItems;

    public AudioClip[] menuItemCips;
    public AudioSource source;

    bool hasPlayedCurrentItem = true;

    public int sceneIndexToLoad = 1;

    [Space(20)]
    [Header("Tutorial Audio")]
    public AudioClip[] tutorialClips;

    private bool isTutorialPlaying = false;

    // Start is called before the first frame update
    private void Start() {
        totalItems = menuItemCips.Length;
    }

    // Update is called once per frame
    void Update() {
        /*
        if(Input.GetKeyDown(KeyCode.Escape)) {
            source.Stop();
        }
        */

        //Navigate menu
        if(!source.isPlaying) {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && menuItemSelected != 0) {
                menuItemSelected -= 1;
                hasPlayedCurrentItem = false;

            } else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && menuItemSelected != totalItems - 1) {
                menuItemSelected += 1;
                hasPlayedCurrentItem = false;
            }

            if (!hasPlayedCurrentItem) {
                source.clip = menuItemCips[menuItemSelected];
                source.PlayOneShot(source.clip);
                hasPlayedCurrentItem = true;
            }
        }
      
        //Select and execute
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
            switch (menuItemSelected) {
                case 0:
                    SceneManager.LoadScene(sceneIndexToLoad);
                    break;

                case 1:
                    if (!isTutorialPlaying) 
                    {
                        StartCoroutine(PlayTutorial());
                    }
                    break;

                case 2:
                    Application.Quit();
                    break;
            }
        }
    }

    private IEnumerator PlayTutorial() {
        isTutorialPlaying = true;

        foreach (var clip in tutorialClips) {
            source.clip = clip;
            source.PlayOneShot(source.clip);
            yield return new WaitForSeconds(source.clip.length + 1f);
        }

        isTutorialPlaying = false; 
    }
}
