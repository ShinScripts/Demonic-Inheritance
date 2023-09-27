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
    [SerializeField] private float minHeartbeatDelay = 1.0f;
    [SerializeField] private float maxHeartbeatDelay = 2.5f;
    [SerializeField] private float heartbeatDelay = 2.5f;
    public bool isPlayerSpotted;

    [SerializeField] private EventReference HeartbeatEvent;
    private EventInstance heartbeat;

    [Header("Audio Settings")]
    private AudioSource source;
    public AudioClip[] clips;
    public float minPitch = 0.875f;
    public float maxPitch = 1.065f;
    //[Space(10)]
    //[Range(0, 100)]
    //[Tooltip("To what percent should distance to enemy affect heartbeatDelay compared to enemy state")]

    //private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        heartbeat = RuntimeManager.CreateInstance(HeartbeatEvent);
        heartbeat.start();

        source = GetComponent<AudioSource>();

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

        //time += Time.deltaTime;

        //UpdateHeartbeatDelay();

        //Play sound when time has hit delay.
        /*
        if (time > heartbeatDelay) {
            PlayHeartbeat();
            time = 0f;
        }
        */
        /*
        if (normDistanceEnemy <= 0.3) {
            StartCoroutine(FadeVolume(2.0f, 1.0f));
        } else if (normDistanceEnemy <= 0.5) {
            StartCoroutine(FadeVolume(2.0f, 0.65f));
        } else if (normDistanceEnemy <= 0.7) {
            StartCoroutine(FadeVolume(2.0f, 0.35f));
        } else if (normDistanceEnemy <= 1) {
            StartCoroutine(FadeVolume(2.0f, 0.2f));
        }
        */
    }

    /*
    private IEnumerator FadeVolume(float fadeDuration, float targetVolume) {
        /*
        float startVolume = source.volume;
        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        while (Time.time < endTime) {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }
        source.volume = targetVolume;
        
        heartbeat.setParameterByName("Occlusion", normDistanceEnemy);
    }
    */

    //TODO: This method should play every time the enemy makes a sound.
    public void UpdateEnemyDistance() {
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = enemy.transform.position;
        float distance = Vector3.Distance(playerPosition, enemyPosition);

        normDistanceEnemy = Mathf.Clamp01(distance / maxDistance);
        print(distance);
        print("Distance to enemy: " + normDistanceEnemy);

        UpdateHeartbeatDelay();
    }

    
    private void UpdateHeartbeatDelay() {
        heartbeatDelay = 1 - normDistanceEnemy;
        heartbeatDelay = heartbeatDelay * (isPlayerSpotted ? 1.65f : 1);
        heartbeat.setParameterByName("DistanceToEnemy", heartbeatDelay);
    }
    
    
    void PlayHeartbeat() {

        heartbeat.start();

    }
}
