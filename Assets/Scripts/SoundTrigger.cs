using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using static UnityEngine.ParticleSystem;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private EventReference TriggerEvent;
    private EventInstance soundTriggerInstance;
    private bool listened = false;

    void Start()
    {
        soundTriggerInstance = RuntimeManager.CreateInstance(TriggerEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!listened)
            {
                PLAYBACK_STATE state;
                other.GetComponent<PlayerMovement>().lastSoundTriggerReference.getPlaybackState(out state);

                if(state == PLAYBACK_STATE.PLAYING)
                {
                    other.GetComponent<PlayerMovement>().lastSoundTriggerReference.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
                soundTriggerInstance.start();
                other.GetComponent<PlayerMovement>().lastSoundTriggerReference = soundTriggerInstance;
                listened = true;

            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            soundTriggerInstance.release();
        }

    }

    private void FixedUpdate()
    {
        if (listened)
        {
            PLAYBACK_STATE state;
            soundTriggerInstance.getPlaybackState(out state);
            if (state == PLAYBACK_STATE.STOPPED)
            {
                Destroy(this);
            }

        }
    }
}
