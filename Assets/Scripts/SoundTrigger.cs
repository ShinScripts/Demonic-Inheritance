using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private EventReference TriggerEvent;
    [SerializeField] private bool isPersistent = false;

    private EventInstance soundTriggerIstance;

    void Start()
    {
        soundTriggerIstance = RuntimeManager.CreateInstance(TriggerEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("block reached");
            soundTriggerIstance.start();         
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player") && !isPersistent)
        {
            soundTriggerIstance.release();
        }
    }
}
