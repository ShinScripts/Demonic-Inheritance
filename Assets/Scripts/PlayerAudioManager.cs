using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAudioManager : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform enemyPosition;
    [Header("FMOD Settings")]
    [SerializeField] private EventReference FootStepsEvent;
    [SerializeField] private EventReference BreathingEvent;
    [SerializeField] private EventReference HeartbeatEvent;
    [SerializeField] private EventReference WallhitEvent;

    [SerializeField]
    private float minDistanceThreshold; // Minimum distance threshold
    [SerializeField]
    private float maxDistanceThreshold; // Maximum distance threshold

    private EventInstance breathingAudioInstance;
    //private EventInstance heartbeatAudioInstance;
    private EventInstance wallhitAudioInstance;
    private EventInstance footStepsAudioInstance;



    [SerializeField] private float distanceParameter = 0;

    private float distanceToEnemy;

    private const string DIST_TO_ENEMY_B = "DistanceToEnemy_B";
    private const string DIST_TO_ENEMY_H = "DistanceToEnemy_H";

    private bool shouldPlay = true;

    private int furnitureCounter = 0;

    private bool playEnemyAudio = true;


    // Start is called before the first frame update
    void Start()
    {
        // Check if we need to skip enemy-related audio
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        playEnemyAudio = currentSceneIndex > 1;

        print("skip enemy audio: " + playEnemyAudio);

        if (playEnemyAudio)
        {
            breathingAudioInstance = RuntimeManager.CreateInstance(BreathingEvent);
            //heartbeatAudioInstance = RuntimeManager.CreateInstance(HeartbeatEvent);
            breathingAudioInstance.start();
            //heartbeatAudioInstance.start();

        }

        wallhitAudioInstance = RuntimeManager.CreateInstance(WallhitEvent);
        footStepsAudioInstance = RuntimeManager.CreateInstance(FootStepsEvent);
        wallhitAudioInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerMovement.transform.position));

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

        if (playEnemyAudio)
        {
            ParseDistance();

            //heartbeatAudioInstance.setParameterByName(DIST_TO_ENEMY_H, distanceParameter);
            breathingAudioInstance.setParameterByName(DIST_TO_ENEMY_B, distanceParameter);
        }

        //  Debug.Log("distance to enemy:" + distanceToEnemy);
        //  Debug.Log("distance parameter:" + distanceParameter);

    }

    private void PlayFootstep()
    {
        footStepsAudioInstance.start();
    }

    private void ParseDistance()
    {
        distanceToEnemy = (transform.position - enemyPosition.position).magnitude;

        // Clamp the distance to stay within the min and max thresholds
        float clampedDistance = Mathf.Clamp(distanceToEnemy, minDistanceThreshold, maxDistanceThreshold);

        // Calculate the parameter as a linear interpolation from 1 to 0 based on clampedDistance
        distanceParameter = 1 - Mathf.InverseLerp(minDistanceThreshold, maxDistanceThreshold, clampedDistance);
    }

    public void PlayWallHitSound(string obstacle, Transform transform, string side)
    {
        wallhitAudioInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        wallhitAudioInstance.setParameterByNameWithLabel("Panner", side);

        if (obstacle.Equals("Furniture") && furnitureCounter <= 0)
        {
            wallhitAudioInstance.setParameterByNameWithLabel("Obstacle", "Furniture_First");
            furnitureCounter++;
        }

        else
        {
            wallhitAudioInstance.setParameterByNameWithLabel("Obstacle", obstacle);
        }

        wallhitAudioInstance.start();

    }
}
