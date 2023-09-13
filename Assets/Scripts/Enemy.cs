using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
        transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, speed * Time.fixedDeltaTime);
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
