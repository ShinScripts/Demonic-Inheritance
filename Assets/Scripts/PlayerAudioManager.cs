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

    [Space(20)]
    [Header("Layers")]
    [SerializeField] private LayerMask wood;
    [SerializeField] private LayerMask tiles;
    [SerializeField] private LayerMask stone;


    [SerializeField]
    private float minDistanceThreshold; // Minimum distance threshold
    [SerializeField]
    private float maxDistanceThreshold; // Maximum distance threshold

    private EventInstance wallhitAudio;

    [SerializeField] private float distanceParameter = 0;

    private float distanceToEnemy;

    private const string DIST_TO_ENEMY_B = "DistanceToEnemy_B";
    private const string DIST_TO_ENEMY_H = "DistanceToEnemy_H";

    private bool shouldPlay = true;


    // Start is called before the first frame update
    void Start()
    {
        wallhitAudio = RuntimeManager.CreateInstance(WallhitEvent);

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

    }

    private void PlayFootstep() //Footsteps event in FMOD
    {
        if(Physics.Raycast(transform.position, Vector3.down, 20f, wood)) {
            print("wood");

        } else if(Physics.Raycast(transform.position, Vector3.down, 20f, tiles)) {
            print("tiles");

        } else if (Physics.Raycast(transform.position, Vector3.down, 20f, stone)) {
            print("stone");

        } else
        {
            print("generic");
        }

        RuntimeManager.PlayOneShot(FootStepsEvent, transform.position);                                                                                 
    }

    private void ParseDistance()
    {
        distanceToEnemy = (transform.position - enemyPosition.position).magnitude;

        // Clamp the distance to stay within the min and max thresholds
        float clampedDistance = Mathf.Clamp(distanceToEnemy, minDistanceThreshold, maxDistanceThreshold);

        // Calculate the parameter as a linear interpolation from 1 to 0 based on clampedDistance
        distanceParameter = 1 - Mathf.InverseLerp(minDistanceThreshold, maxDistanceThreshold, clampedDistance);
    }

    public void PlayWallHitSoundFront()
    {
        wallhitAudio.setParameterByNameWithLabel("FrontBack", "Front");
        wallhitAudio.start();
       // wallhitAudioFront.release();

    }

    public void PlayWallHitSoundBack()
    {
        wallhitAudio.setParameterByNameWithLabel("FrontBack", "Back");
        wallhitAudio.start();
       // wallhitAudioBack.release();

    }
}
