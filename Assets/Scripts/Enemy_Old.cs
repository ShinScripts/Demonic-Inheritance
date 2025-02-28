using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Old : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float speed = 2f;

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = start.transform.position;
        target = end;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) <= 0.1f)
        {
            ChangeTargets();
        }
    }

    void ChangeTargets()
    {
        target = (target == start) ? end : start;
    }
}
