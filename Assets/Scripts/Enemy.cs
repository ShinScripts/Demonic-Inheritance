using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public GameObject start;
    public GameObject end;
    public float speed = 2f;

    private GameObject target;
    private GameObject player;

    private NavMeshAgent agent;


    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        transform.position = start.transform.position;
        target = end;
    }

    void Update() {
        transform.LookAt(player.transform);

        bool has_hit = Physics.Raycast(transform.position + transform.forward, transform.forward, out RaycastHit hit, 200f);

        if (has_hit && hit.transform.CompareTag("Player")) {
            agent.SetDestination(player.transform.position);
            return;
        } else {
            agent.SetDestination(target.transform.position);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f) {
            ChangeTargets();
        }
    }

    void ChangeTargets() {
        target = (target == start) ? end : start;
    }
}
