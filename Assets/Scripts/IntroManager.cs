using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private EventReference IntroEvent;
    private EventInstance introAudioInstance;
    private float tresholdTime = 0.5f;
    private float startHoldingTime = 0f;
    private int sceneIndexToLoad;

    // Start is called before the first frame update
    void Start()
    {
        introAudioInstance = RuntimeManager.CreateInstance(IntroEvent);
        introAudioInstance.start();
        sceneIndexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startHoldingTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            float holdTime = Time.time - startHoldingTime;

            if (holdTime >= tresholdTime)
            {
                introAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                StartCoroutine(LoadNewScene(2f));
            }     
        }
    }

    private void FixedUpdate()
    {
        PLAYBACK_STATE state;
        introAudioInstance.getPlaybackState(out state);
            
        if(state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            StartCoroutine(LoadNewScene(0f));
        }

       
    }

    IEnumerator LoadNewScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        introAudioInstance.release();
        SceneManager.LoadScene(sceneIndexToLoad);
        yield return null;
    }
}
