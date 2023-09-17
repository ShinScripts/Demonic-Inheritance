using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;


    //private FMOD.Studio.EventInstance playerState;
    [Header("FMOD Settings")]
    [SerializeField] private EventReference FootStepsEvent;

    private bool shouldPlay = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.IsBusy)
        {
            if(shouldPlay) {
                shouldPlay = false;
                PlayFootstep();
            }
        }

        else
        {
            shouldPlay = true;
        }
      
    }

    public void PlayFootstep() //Footsteps event in FMOD
    {
        RuntimeManager.PlayOneShot(FootStepsEvent, transform.position);                                                                                    // We also set our event instance to release straight after we tell it to play, so that the instance is released once the event had finished playing.
    }
}
