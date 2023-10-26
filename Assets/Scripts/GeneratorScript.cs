using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class GeneratorScript : MonoBehaviour
{
    public bool beenTaken;
    public int generatorNumber;

    [SerializeField] private EventReference GeneratorCollectedEvent;
    private EventInstance generatorCollectedAudio;

    private void Start()
    {
        generatorCollectedAudio = RuntimeManager.CreateInstance(GeneratorCollectedEvent);
        generatorCollectedAudio.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }

    void OnTriggerEnter (Collider collision)
    {
        if (!beenTaken && collision.CompareTag("Player"))
        {
            beenTaken = true;
            collision.gameObject.GetComponent<PlayerMovement>().SetCheckPoint(transform.position);
            generatorCollectedAudio.start();
            generatorCollectedAudio.release();
            Debug.Log("Generator " + generatorNumber + " has been taken!");

        }
    }


}
