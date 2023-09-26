using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float speed = 2f;

    private GameObject target;

    private NavMeshAgent agent;
    private GameObject player;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        transform.position = start.transform.position;
        target = end;
    }

    void Update()
    {
        transform.LookAt(player.transform);

        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.forward, out hit);

        if (hit.collider)
        {
            Debug.DrawLine(transform.position, player.transform.position, Color.magenta);
            print(hit.collider);
        }

        transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, speed * Time.deltaTime);

        if (gameObject.transform.position == target.transform.position)
        {
            ChangeTargets();
        }
    }

    void ChangeTargets()
    {
        target = (target == start) ? end : start;
    }
}
