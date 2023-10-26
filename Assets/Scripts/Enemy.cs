using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class Enemy : MonoBehaviour
{
    public GameObject[] patrol;
    private int patrol_index = 0;
    private GameObject target;
    private GameObject player;
    [SerializeField] private EventReference EnemyDetection;
    private NavMeshAgent agent;

    private bool follow_player = false;
    private bool detected = false;

    private EventInstance enemyDetectionSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        print(GameObject.Find("Rules").GetComponent<ExternalParameters>().enemy_speed);

        agent.speed = GameObject.Find("Rules").GetComponent<ExternalParameters>().enemy_speed; 

        transform.position = patrol[patrol_index].transform.position;
        target = patrol[++patrol_index];

        enemyDetectionSound = RuntimeManager.CreateInstance(EnemyDetection);
        enemyDetectionSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }

    void Update()
    {
        if (detected && follow_player && !player.GetComponent<PlayerMovement>().is_dead && Vector3.Distance(transform.position, player.transform.position) < 1.5f)
        {
            player.GetComponent<PlayerMovement>().is_dead = true;
            print("dead");
        }

        if (player.GetComponent<PlayerMovement>().IsBusy && !follow_player)
        {
            StartCoroutine(StartFollowing(2));
        }

        if (CanSeePlayer() && follow_player)
        {
            agent.SetDestination(player.transform.position);
            return;
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            ChangeTargets();
        }
    }

    void ChangeTargets()
    {
        if (patrol.Length == patrol_index)
        {
            patrol_index = 0;
            target = patrol[patrol_index];
        }
        else
        {
            target = patrol[patrol_index++];
        }
    }

    private IEnumerator StartFollowing(float duration)
    {
        follow_player = true;

        yield return new WaitForSeconds(duration);

        follow_player = false;

        yield return null;
    }

    bool CanSeePlayer()
    {
        transform.LookAt(player.transform);

        bool has_hit = Physics.Raycast(transform.position + transform.forward, transform.forward, out RaycastHit hit, 200f);

        return has_hit && hit.transform.CompareTag("Player");
    }

    // Detection and stuff
    private void OnTriggerStay(Collider collider)
    {
        if (CanSeePlayer() && collider.CompareTag("Player") && !detected && follow_player)
        {
            detected = true;

            // play activation sound
            /**
             * Set atributes for transform and label before calling start(); 
             * 
             * Detection Type labels for now: PlayerDetected, PlayerNoDetected
             */
            enemyDetectionSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
            enemyDetectionSound.setParameterByNameWithLabel("DetectionType", "PlayerDetected");
            enemyDetectionSound.start();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") && detected)
        {
            detected = false;

            enemyDetectionSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
            enemyDetectionSound.setParameterByNameWithLabel("DetectionType", "PlayerNoDetected");
            enemyDetectionSound.start();
        }
    }
}
