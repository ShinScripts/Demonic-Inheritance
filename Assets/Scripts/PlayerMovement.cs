using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float increment = 1f;

    float target_rotation = 0f;
    public float rotation_speed = 90f;
    public float rotation_smoothness = 5f;

    private bool isMoving = false;

    public SoundSource sourceFront;
    public SoundSource sourceBack;
    public AudioSource sourceCenter;
    public AudioClip[] footsteps;
    public AudioClip[] wall_bump_clips;

    public float maxSoundPitch = 1.2f;
    public float minSoundPitch = 0.8f;

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
                float pitch = Random.Range(minSoundPitch, maxSoundPitch); 
                sourceFront.PlayOneShot(wall_bump_clips[Random.Range(0, wall_bump_clips.Length)], pitch);
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
                float pitch = Random.Range(minSoundPitch, maxSoundPitch);
                sourceBack.PlayOneShot(wall_bump_clips[Random.Range(0, wall_bump_clips.Length)], pitch);
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
        sourceCenter.pitch = Random.Range(0.7f, 0.9f);
        sourceCenter.clip = footsteps[clip];

    }

    IEnumerator PlayFootsteps()
    {
        for (int i = 0; i < 3; i++)
        {
            isMoving = true;
            RandomizeFootstep();
            sourceCenter.PlayOneShot(sourceCenter.clip);
            yield return new WaitForSeconds(sourceCenter.clip.length + Random.Range(0.125f, 0.17f));
            isMoving = false;
        }
    }

}
