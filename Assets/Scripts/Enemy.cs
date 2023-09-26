using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float speed = 2f;

    private bool isDead;

    GameObject target;

    AudioSource source;
    public AudioClip[] footsteps;
    public float footstepDelayFrequency = 1.5f;

   
    // Start is called before the first frame update
    void Start()
    {
        transform.position = start.transform.position;
        target = end;
        source = GetComponent<AudioSource>();
        StartCoroutine(PlayFootsteps());
    }

    // Update is called once per frame
    void Update()
    {
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

    IEnumerator PlayFootsteps() {
        while (!isDead) {
            AudioClip clip = footsteps[Random.Range(0, footsteps.Length - 1)];
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length + footstepDelayFrequency);
        }

    }
}
