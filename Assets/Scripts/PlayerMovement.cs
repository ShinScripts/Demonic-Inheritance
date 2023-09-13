using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float target_rotation = 0f;
    public float rotation_speed = 90f;
    public float rotation_smoothness = 5f;

    private bool isMoving = false;

    public AudioSource source;
    public AudioClip[] footsteps;
    public AudioClip wall_clip;

    private bool ClearToMove(bool forward = true)
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, forward ? transform.forward : transform.forward * -1, out hit, increment);

        return !hit.collider;
    }

    private void Update()
    {
        if (isMoving)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (ClearToMove())
            {
                transform.localPosition += transform.forward * increment;
                StartCoroutine(PlayFootsteps());
            }
            else
            {
                print("obstacle in front");
                source.clip = wall_clip;
                source.PlayOneShot(source.clip);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (ClearToMove(false))
            {
                transform.localPosition -= transform.forward * increment;
                StartCoroutine(PlayFootsteps());
            }
            else
            {
                print("obstacle behind");
                source.clip = wall_clip;
                source.PlayOneShot(source.clip);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            target_rotation -= 90f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            target_rotation += 90f;
        }

        Quaternion current_rotation = transform.rotation;
        Quaternion target_quaternion = Quaternion.Euler(0f, target_rotation, 0f);
        transform.rotation = Quaternion.RotateTowards(current_rotation, target_quaternion, rotation_speed * Time.deltaTime * rotation_smoothness);


    }

    public void RandomizeFootstep()
    {
        int clip = Random.Range(0, footsteps.Length - 1);
        source.pitch = Random.Range(0.7f, 0.9f);
        source.clip = footsteps[clip];

    }

    IEnumerator PlayFootsteps()
    {
        for (int i = 0; i < 3; i++)
        {
            isMoving = true;
            RandomizeFootstep();
            source.PlayOneShot(source.clip);
            yield return new WaitForSeconds(source.clip.length + Random.Range(0.125f, 0.17f));
            isMoving = false;
        }
    }

}
