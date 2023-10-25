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
    private EventInstance soundTriggerIstance;

    void Start()
    {
        soundTriggerIstance = RuntimeManager.CreateInstance(TriggerEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PLAYBACK_STATE state;
            other.GetComponent<PlayerMovement>().lastSoundTriggerReference.getPlaybackState(out state);

            if(state == PLAYBACK_STATE.PLAYING)
            {
                other.GetComponent<PlayerMovement>().lastSoundTriggerReference.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            soundTriggerIstance.start();
            other.GetComponent<PlayerMovement>().lastSoundTriggerReference = soundTriggerIstance;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            soundTriggerIstance.release();
        }

    }
}
