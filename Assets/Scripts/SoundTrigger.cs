using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

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
            soundTriggerIstance.start();            
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
