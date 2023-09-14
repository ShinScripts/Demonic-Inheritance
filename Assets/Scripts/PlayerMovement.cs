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
    public AudioClip[] wall_bump_clips; //front array[0], back array[1]

    public float maxSoundPitch = 1.2f;
    public float minSoundPitch = 0.8f;

    private bool ClearToMove(bool forward = true)
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, forward ? transform.forward : transform.forward * -1, out hit, increment);

        return !(hit.collider && !hit.collider.CompareTag("Generator"));
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
                //front array[0], back array[1]
                sourceFront.PlayOneShot(wall_bump_clips[0], pitch);
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
                //front array[0], back array[1]
                sourceBack.PlayOneShot(wall_bump_clips[1], pitch);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            target_rotation -= 90f;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            target_rotation += 90f;
        }

        Quaternion current_rotation = transform.rotation;
        Quaternion target_quaternion = Quaternion.Euler(0f, target_rotation, 0f);
        transform.rotation = Quaternion.RotateTowards(current_rotation, target_quaternion, rotation_speed * Time.deltaTime * rotation_smoothness);


    }

    public void RandomizeFootstep()
    {
        int clip = Random.Range(0, footsteps.Length);
        sourceCenter.pitch = Random.Range(0.8f, 1.1f);
        sourceCenter.clip = footsteps[clip];

    }

    IEnumerator PlayFootsteps()
    {
        for (int i = 0; i < 2; i++)
        {
            isMoving = true;
            RandomizeFootstep();
            sourceCenter.PlayOneShot(sourceCenter.clip);
            yield return new WaitForSeconds(sourceCenter.clip.length + Random.Range(0.125f, 0.17f));
            isMoving = false;
        }
    }

}
