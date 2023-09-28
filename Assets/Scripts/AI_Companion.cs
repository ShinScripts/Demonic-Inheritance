using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Companion : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GeneratorManager generatorManager;
    [SerializeField] private EventReference NestedEvent;
    private EventInstance ai_assitance;

    int totalGenerators;

    public EventInstance Ai_assitance { get => ai_assitance; set => ai_assitance = value; }

    // Start is called before the first frame update
    void Start()
    {
        ai_assitance = RuntimeManager.CreateInstance(NestedEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.IsBusy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ai_assitance.setParameterByNameWithLabel("RoomZone", playerMovement.CurrentSoundZoneName);
                ai_assitance.start();
                totalGenerators = generatorManager.GeneratorsLeft();
                //Debug.Log("Asking for help" + " generators left: " + totalGenerators);
            }
        }

        float voiceActing;
        float outValue;
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("VoiceActingOngoing", out outValue, out voiceActing); ;
        Debug.Log("voice acting is: " + voiceActing);
    }

    public void SetGeneratorIndex(int index)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PiecesCollected", index);
    }
}
