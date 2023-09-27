using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform enemyPosition;

    //private FMOD.Studio.EventInstance playerState;
    [Header("FMOD Settings")]
    [SerializeField] private EventReference FootStepsEvent;
    [SerializeField] private EventReference BreathingEvent;
    [SerializeField] private EventReference HeartbeatEvent;
    [SerializeField] private EventReference WallhitEvent;


    [SerializeField]
    private float minDistanceThreshold; // Minimum distance threshold
    [SerializeField]
    private float maxDistanceThreshold; // Maximum distance threshold

    private EventInstance breathingAudio;
    private EventInstance heartbeatAudio;
    private EventInstance wallhitAudio;

    [SerializeField] private float distanceParameter = 0;

    private float distanceToEnemy;

    private const string DIST_TO_ENEMY_B = "DistanceToEnemy_B";
    private const string DIST_TO_ENEMY_H = "DistanceToEnemy_H";

    private bool shouldPlay = true;

    private Transform audioSourceFrontPosition;
    private Transform audioSourceBackPosition;

    private int furnitureCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        breathingAudio = RuntimeManager.CreateInstance(BreathingEvent);
        heartbeatAudio = RuntimeManager.CreateInstance(HeartbeatEvent);
        wallhitAudio = RuntimeManager.CreateInstance(WallhitEvent);

        breathingAudio.start();
        heartbeatAudio.start();

        audioSourceBackPosition = playerMovement.AudioFrontPosition;
        audioSourceFrontPosition = playerMovement.AudioBehindPosition;

        wallhitAudio.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerMovement.transform.position));

    }

    // Update is called once per frame
    void FixedUpdate()
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

        ParseDistance();

        heartbeatAudio.setParameterByName(DIST_TO_ENEMY_H, distanceParameter);
        breathingAudio.setParameterByName(DIST_TO_ENEMY_B, distanceParameter);



        //  Debug.Log("distance to enemy:" + distanceToEnemy);
        //  Debug.Log("distance parameter:" + distanceParameter);

    }

    private void PlayFootstep() //Footsteps event in FMOD
    {
        RuntimeManager.PlayOneShot(FootStepsEvent, transform.position);                                                                                    // We also set our event instance to release straight after we tell it to play, so that the instance is released once the event had finished playing.
    }

    private void ParseDistance()
    {
        distanceToEnemy = (transform.position - enemyPosition.position).magnitude;

        // Clamp the distance to stay within the min and max thresholds
        float clampedDistance = Mathf.Clamp(distanceToEnemy, minDistanceThreshold, maxDistanceThreshold);

        // Calculate the parameter as a linear interpolation from 1 to 0 based on clampedDistance
        distanceParameter = 1 - Mathf.InverseLerp(minDistanceThreshold, maxDistanceThreshold, clampedDistance);
    }

    public void PlayWallHitSoundFront(string obstacle)
    {
        wallhitAudio.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(audioSourceFrontPosition.position));

        wallhitAudio.setParameterByNameWithLabel("Panner", "Front");

        if (obstacle.Equals("Wall"))
        {
            wallhitAudio.setParameterByNameWithLabel("Obstacle", "Wall");
        }

        else if (obstacle.Equals("Furniture") && furnitureCounter <= 0)
        {
            wallhitAudio.setParameterByNameWithLabel("Obstacle", "Furniture_First");
            furnitureCounter++;  
        }
        
        else
        {
            wallhitAudio.setParameterByNameWithLabel("Obstacle", "Furniture");
        }

        wallhitAudio.start();
        //wallhitAudio.release();

    }

    public void PlayWallHitSoundBack(string obstacle)
    {

        wallhitAudio.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(audioSourceFrontPosition.position));

        wallhitAudio.setParameterByNameWithLabel("Panner", "Back");

        if (obstacle.Equals("Wall"))
        {
            wallhitAudio.setParameterByNameWithLabel("Obstacle", "Wall");
        }

        else if (obstacle.Equals("Furniture") && furnitureCounter <= 0)
        {
            wallhitAudio.setParameterByNameWithLabel("Obstacle", "Furniture_First");
            furnitureCounter++;
        }

        else
        {
            wallhitAudio.setParameterByNameWithLabel("Obstacle", "Furniture");
        }

        wallhitAudio.start();

        //wallhitAudio.release();

    }
}
