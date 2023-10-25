using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AI_Companion : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GeneratorManager generatorManager;
    [SerializeField] private EventReference NestedEvent;
    private EventInstance ai_assitance;
    private float tresholdTime = 0.25f;
    private float startHoldingTime = 0f;

    private bool isPlayerInTutorial;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerInTutorial = SceneManager.GetActiveScene().buildIndex == 2 ? true : false;

        ai_assitance = RuntimeManager.CreateInstance(NestedEvent);

        string location = isPlayerInTutorial ? "Outside" : "Inside";

        ai_assitance.setParameterByNameWithLabel("Location", location);

        print(location);

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.IsBusy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //GetAIAssistence();
                //Debug.Log("Space down at: " +  startHoldingTime);
                startHoldingTime = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                float holdTime = Time.time - startHoldingTime;

                if(holdTime <= tresholdTime)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("AssistenceLength", "Short");
                    Debug.Log("Short assistence");
                }
                else
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("AssistenceLength", "Long");
                    Debug.Log("Long assistence");
                }

                GetAIAssistence();

                //Debug.Log("Space up with difference: " + holdTime);

            }
        }
    }

    private void GetAIAssistence()
    {
        if (isPlayerInTutorial)
        {
            ai_assitance.setParameterByNameWithLabel("DirectionGoal", playerMovement.GetCorrentDirection());
            ai_assitance.setParameterByNameWithLabel("DistanceToGoal", playerMovement.CurrentSoundZoneName);

        }
        else
        {
            ai_assitance.setParameterByNameWithLabel("RoomZone", playerMovement.CurrentSoundZoneName);

        }
        ai_assitance.start();
    }
}
