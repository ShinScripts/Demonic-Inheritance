using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class HeartbeatManager : MonoBehaviour
{
    public Transform enemy;
    public Transform player;

    [Range(0,1)]
    [SerializeField] private float normDistanceEnemy = 1f;
    [SerializeField] private float maxDistance = 60;

    [Header("Heartbeat")]
    [Header("INFO: When player is spotted the heartbeatdelay can go below the minHeartbeatdelay")]
    public float heartbeatDelay = 2.5f;
    public bool isPlayerSpotted;

    [SerializeField] private EventReference HeartbeatEvent;
    private EventInstance heartbeat;

    [Header("Audio Settings")]
    public float minPitch = 0.875f;
    public float maxPitch = 1.065f;

    private bool doUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        heartbeat = RuntimeManager.CreateInstance(HeartbeatEvent);
        heartbeat.start();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();

        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = enemy.transform.position;
        float distance = Vector3.Distance(playerPosition, enemyPosition);
        normDistanceEnemy = Mathf.Clamp01(distance / maxDistance);
        print(distance);
        print(normDistanceEnemy);

        
    }

    // Update is called once per frame
    void Update()
    {
        //TEMP This should only play when enemy makes sound
        UpdateEnemyDistance();

        if (doUpdate) UpdateHeartbeatDelay();
        
    }

    //TODO: This method should play every time the enemy makes a sound.
    public void UpdateEnemyDistance() {
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = enemy.transform.position;
        float distance = Vector3.Distance(playerPosition, enemyPosition);

        normDistanceEnemy = Mathf.Clamp01(distance / maxDistance);
        //print(distance);
        //print("Distance to enemy: " + normDistanceEnemy);
        
    }

    
    private void UpdateHeartbeatDelay() {
        heartbeatDelay = 1 - normDistanceEnemy;
        heartbeatDelay = heartbeatDelay * (isPlayerSpotted ? 1.65f : 1);
        heartbeat.setParameterByName("DistanceToEnemy", heartbeatDelay);
        UnityEngine.Debug.Log(heartbeatDelay);
    }

    public void DisableUpdates() {
        doUpdate = false;
    }
}
