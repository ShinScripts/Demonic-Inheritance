using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float speed = 2f;
    private GameObject target;
    private GameObject player;

    private NavMeshAgent agent;

    bool follow_player = false;

    bool detected = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        transform.position = start.transform.position;
        target = end;
    }

    void Update()
    {
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

        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            ChangeTargets();
        }
    }

    void ChangeTargets()
    {
        target = (target == start) ? end : start;
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
            StartCoroutine(DetectionTrigger());
        }
    }

    private IEnumerator DetectionTrigger()
    {
        detected = true;

        print("detected: " + detected);


        // play activation sound

        yield return new WaitForSeconds(3);

        detected = false;

        print("detected: " + detected);

        // play deactivation sound


        yield return null;
    }
}
